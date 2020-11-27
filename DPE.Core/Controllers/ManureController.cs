using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DPE.Core.AuthHelper;
using DPE.Core.AuthHelper.OverWrite;
using DPE.Core.Common.Helper;
using DPE.Core.Common.HttpContextUser;
using DPE.Core.IServices;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.AOP;

namespace DPE.Core.Controllers
{


    /// <summary>
    /// 肥料 管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/manure")]
    [Authorize(Permissions.Name)]
    public class ManureController : Controller
    {


        private readonly IManureexchangeServices _imanureexchangeservices;
        private readonly IManureServices _imanureservices;
        private readonly IDPEServices _idpeservices;
        private readonly IUserAppleStatusServices _iuserapplestatusservices;
        private readonly IUserAppleConsumeServices _iuserappleconsumeservices;
        private readonly ISplitServices _isplitservices;
        private readonly IFriendsServices _ifriendsservices;
        private readonly ISettingsServices _isettingsservices;
        private readonly IUnitOfWork _iunitofwork;
        private readonly IStockServices _istockservices;
        readonly IUser _user;

        readonly IUserInfoServices _iuserinfoservices;
        private readonly ISysUserInfoServices _isysuserinfoservices;
        private readonly ISplitRecordsServices _isplitrecordsservices;
        
        public ManureController(ISysUserInfoServices isysuserinfoservices,IUnitOfWork unitOfWork, IUser user, IManureexchangeServices imanureexchangeservices, IManureServices imanureservices, IDPEServices idpeservices,
            IUserAppleStatusServices iuserapplestatusservices, IUserAppleConsumeServices iuserappleconsumeservices, ISplitServices isplitservices,
            IFriendsServices ifriendsservices, ISettingsServices isettingsservices, IStockServices istockservices, ISplitRecordsServices isplitrecordsservices, IUserInfoServices iuserinfoservices)
        {
            this._user = user;
            this._imanureexchangeservices = imanureexchangeservices;
            this._imanureservices = imanureservices;
            this._idpeservices = idpeservices;
            this._iuserapplestatusservices = iuserapplestatusservices;
            this._iuserappleconsumeservices = iuserappleconsumeservices;
            this._isplitservices = isplitservices;
            this._ifriendsservices = ifriendsservices;
            this._isettingsservices = isettingsservices;
            this._iunitofwork = unitOfWork;
            this._istockservices = istockservices;
            this._isplitrecordsservices = isplitrecordsservices;
            _iuserinfoservices = iuserinfoservices;
            _isysuserinfoservices = isysuserinfoservices;

        }

        /// <summary>
        /// 仓里肥料列表记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetManureExchange")]
        public async Task<MessageModel<ExchangeViewModels>> GetManureExchange()
        {
            
            MessageModel<ExchangeViewModels> result = new MessageModel<ExchangeViewModels>();

            try
            {
                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }
                var sp = await _imanureservices.QueryById(uid);
                var spinfo = await _imanureexchangeservices.GetManureExchange(uid);

                return new MessageModel<ExchangeViewModels>()
                {
                    success = true,
                    msg = "",
                    response = new ExchangeViewModels()
                    {
                        list = (from item in spinfo
                                orderby item.id descending
                                select new ExchangeViewList
                                {
                                    msg = item.remark,
                                    time = DateHelper.GetCreatetime(Convert.ToDateTime(item.createTime))
                                }).ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                result.success = false;
                result.code = 1007;
                result.msg = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// 请求播种
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ApplySow")]
        public async Task<MessageModel<dynamic>> ApplySow()
        {
            long uid = _user.ID;
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            if (uid == 0)
            {
                result.success = false;
                result.code = 1003;
                return result;
            }

            var appleStatus = await _iuserapplestatusservices.GetUserAppleStatus(uid);
            if (appleStatus == null)
            {
                result.success = false;
                result.code = 13001;
                return result;
            }
            if (appleStatus.Status != 1)
            {
                result.success = false;
                result.code = 13002;
                return result;
            }

            try
            {
                _iunitofwork.BeginTran();//开启事务
                appleStatus.Status = 2;
                bool updateStatus = await _iuserapplestatusservices.UpdateUserAppleStatus(appleStatus);
                if (updateStatus)
                {
                    result.code = 0;
                    result.success = true;
                    result.response = new
                    {
                        status = appleStatus.Status
                    };
                    _iunitofwork.CommitTran();
                }
                else
                {
                    result.code = 13003;
                    result.success = false;
                    _iunitofwork.RollbackTran();//提交事务
                }
            }
            catch (Exception ex)
            {
                _iunitofwork.RollbackTran();//回滚事务
                result.code = 1007;
                result.success = false;
                result.msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 请求扩建农场数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ApplyFEData")]
        public async Task<MessageModel<ApplyFEDataViewModels>> ApplyFEData()
        {
            
            MessageModel<ApplyFEDataViewModels> result = new MessageModel<ApplyFEDataViewModels>();

            try
            {
                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }

                var data = await _idpeservices.GetDPEData(uid);
                if (data == null)
                {
                    result.code = 15001;
                    result.success = false;
                    return result;
                }

                result.code = 0;
                result.success = true;
                result.response = new ApplyFEDataViewModels()
                {
                    day = 0,
                    seed = (int)data.amount
                };

                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.code = 1007;
                result.msg = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// 好友列表刷新
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("FriendsRefresh")]
        public async Task<MessageModel<FriendsViewModelList>> FriendsRefresh()
        {
            MessageModel<FriendsViewModelList> result = new MessageModel<FriendsViewModelList>();

            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }

                var friendsData = await _ifriendsservices.GetFriends(uid);
                if (friendsData == null || friendsData.Count == 0)
                {
                    result.code = 17001;
                    result.success = false;
                    return result;
                }


                result.code = 0;
                result.success = true;
                result.response = new FriendsViewModelList() { list = FridensListConvertToViewModelList(friendsData) };

                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.code = 1007;
                result.msg = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// 邀请好友
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Invitation")]
        public async Task<MessageModel<dynamic>> Invitation()
        {
            long uid = _user.ID;
            if (uid == 0)
            {
                return new MessageModel<dynamic>()
                {
                    success = false,
                    code = 1003
                };
            }
            string url = (await _isettingsservices.GetSettings()).InvitationUrl + "?uid=" + uid;
            return new MessageModel<dynamic>()
            {
                success = true,
                code = 0,
                msg = "",
                response = new
                {
                    url = url
                }
            };
        }

        /// <summary>
        /// 请求苹果的总数和单价
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ApplyAppleCountAndPrice")]
        public async Task<MessageModel<dynamic>> ApplyAppleCountAndPrice()
        {
            long uid = _user.ID;
            MessageModel<dynamic> result = new MessageModel<dynamic>() {
                success = false,
                code = 0,
                response = null
            };
            if (uid == 0)
            {
                result.code = 1003;
                result.success = false;
                return result;
            }

            //获取单价
            var stockData = await _istockservices.GetStock();
            if (stockData == null)
            {
                result.code = 24001; //苹果数量为空
                result.success = false;
                return result;
            }

            //获取苹果数量
            var appleData = await _idpeservices.GetDPEData(uid);
            if (appleData == null)
            {
                result.code = 24002; //单价数据为空
                result.success = false;
                return result;
            }

            result.code = 0;
            result.success = true;
            result.response = new
            {
                apple = appleData.amount,
                price = stockData.price
            };

            return result;
        }


        /// <summary>
        /// 好友施肥
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("FriendsFertilizer")]
        public async Task<MessageModel<FriendsFertilizerViewModel>> FriendsFertilizer(string uid,int idx)
        {
            MessageModel<FriendsFertilizerViewModel> result = new MessageModel<FriendsFertilizerViewModel>();
            int errCount = 0;
            try
            {
                _iunitofwork.BeginTran();//开始事务
                long selfUId = _user.ID;
                
                if (selfUId == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }
                if (string.IsNullOrEmpty(uid))
                {
                    result.success = false;
                    result.code = 19001;
                    return result;
                }

                //获取本人肥料
                Manure manure = await _imanureservices.GetOne(selfUId);
                if (manure.amount == null || manure.amount <= 0)
                {
                    result.success = false;
                    result.code = 19002; //肥料不足
                    return result;
                }

                //获取好友及其浇水信息
                Split split = await _isplitservices.GetOneByUId(long.Parse(uid));
                if (split == null)
                {
                    result.success = false;
                    result.code = 19003; //未找到好友
                    return result;
                }

                manure.amount -= 1;//本人肥料-1
                split.PCount += 1;//好友被施肥次数+1

                //更新本人肥料数量并记录
                bool manureUpdate = await _imanureservices.UpdateManure(manure);
                int manurechangeAdd = await _imanureexchangeservices.AddManureExchange(new Manureexchange()
                {
                    uID = selfUId,
                    amount = - 1,
                    lastTotal = manure.amount + 1,
                    fromID = long.Parse(uid),
                    createTime = DateTime.Now,
                    stype = 1,
                    remark =string.Format("幫{0}施肥了", (await _iuserinfoservices.GetUserInfo(long.Parse(uid))).uNickName )
                });
                if (!manureUpdate) errCount++; //更新失败，记录+1
                if(manurechangeAdd <= 0) errCount++; //新增记录失败，记录+1

                //更新好友被施肥次数并记录
                bool splitUpdate = await _isplitservices.UpdateSplit(split);
                int manurechangeSub = await _imanureexchangeservices.AddManureExchange(new Manureexchange()
                {
                    uID = long.Parse(uid),
                    amount = 1,
                    lastTotal = split.PCount - 1,
                    fromID = selfUId,
                    createTime = DateTime.Now,
                    stype = 2,
                    remark = string.Format("{0}幫你施肥了", (await _iuserinfoservices.GetUserInfo(long.Parse(uid))).uNickName)
                });

                if (!splitUpdate) errCount++; //更新失败，记录+1
                if (manurechangeSub <= 0) errCount++; //新增记录失败，记录+1

                //获取施肥后的好友信息
                Friends friends = null;

                if (split.PCount >= 10) //如果已为此好友的施肥超过10次，则换刷新好友列表
                {
                    friends = await _ifriendsservices.GetOne(selfUId);
                }
                else
                {
                    friends = (await _ifriendsservices.GetFriends(selfUId)).Find(c => c.uId == split.uID);
                }

                if (friends == null) 
                {
                    result.success = false;
                    result.code = 19003; //未找到好友
                    return result;
                }

                if (errCount == 0)
                {
                    _iunitofwork.CommitTran();//提交事务
                    result.code = 0;
                    result.success = true;
                    result.response = new FriendsFertilizerViewModel()
                    {
                        idx = idx,
                        player_data = FridensConvertToViewModel(friends)
                    };
                }
                else
                {
                    _iunitofwork.RollbackTran();//回滚
                    result.code = 19004;
                    result.success = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                _iunitofwork.RollbackTran();//回滚事务
                result.success = false;
                result.code = 1007;
                result.msg = ex.Message;
                return result;
            }
            

        }

        /// <summary>
        /// 好友列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFriendsList")]
        public async Task<MessageModel<FriendsViewModelList>> GetFriendsList()
        {
            long uid = _user.ID;
            MessageModel<FriendsViewModelList> result = new MessageModel<FriendsViewModelList>();
            if (uid == 0)
            {
                result.success = false;
                result.code = 1003;
                return result;
            }

            var friendsData = await _ifriendsservices.GetFriends(uid);
            result.code = 0;
            result.success = true;
            result.response = new FriendsViewModelList() { list = FridensListConvertToViewModelList(friendsData) };

            return result;
        }

        /// <summary>
        /// 请求出售苹果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ApplySellApple")]
        public async Task<MessageModel<dynamic>> ApplySellApple(long uid,int num)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            if (uid == 0)
            {
                result.success = false;
                result.code = 1003;
                return result;
            }

            if (num == 0)
            {
                result.success = true;
                result.response = new
                {
                    num = num,
                    coin = num,
                    gold = num 
                };
                result.code = 0;
                return result;
            }
            if (num < 0)
            {
                result.success = false;
                result.code = 14001;
                return result;
            }
            var setting = await _isettingsservices.GetSettings();
            if (setting == null)
            {
                result.success = false;
                result.code = 14002;
                return result;
            }

            #region 调用队列
            string name = "ApplySellApple";
            string str = uid + "," + num;
            RabbitMQClient client = new RabbitMQClient();
            client.PushMessage(name,str);
            #endregion

            result.code = 0;
            result.success = true;
            result.response = new {
                num = num,
                coin = num * setting.SPRate,
                gold = num * setting.EPRate
            };
            return result;
        }

        /// <summary>
        /// 出售股票
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SellStock")]
        public async Task<MessageModel<dynamic>> SellStock(long uid, int num, string tpwd, string gcode)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            
            try
            {
                if (_user.ID == 0)
                {
                    //用户不存在
                    result.code = 61001;
                    result.success = false;
                    result.msg = "登錄已經過期，請重新登陸";
                    return result;
                }
                var userinfo = await _isysuserinfoservices.QueryById(uid);

                if(userinfo.fromMainID!=_user.ID&& _user.ID!=uid)
                {
                    result.code = 61001;
                    result.success = false;
                    result.msg = "登錄已經過期，請重新登陸";
                    return result;
                }

                var dpe = await _idpeservices.GetDPEData(uid);
                if (dpe.amount< num)
                {
                    //用户不存在
                    result.code = 61001;
                    result.success = false;
                    result.msg = "賬戶金額不足！請重試！";
                    return result;
                }
                //判断交易密码
                var tranpwd = await _isysuserinfoservices.checkTrad(uid, tpwd);
                if (tranpwd == null)
                {
                    //交易密码错误
                   result.code = 61004;
                    result.success = false;
                    result.msg = "交易密碼錯誤";
                    return result;
                }

                //判断谷歌验证
                var trangode = await _isysuserinfoservices.checkGoogleKey(uid, gcode);
                if (trangode == null)
                {
                    //谷歌验证错误
                    result.code = 61005;
                    result.success = false;
                    result.msg = "谷歌驗證碼校驗錯誤";
                    return result;
                }

                var tmpapply=await ApplySellApple(uid,num);

                if (tmpapply.code != 0) 
                {
                    result.code = 61006;
                    result.success = false;
                    result.msg = "服務器繁忙，請稍後再試";
                    return result;

                }

                return result;
            }
            catch
            {
                return result;
            }
        }


        /// <summary>
        /// 是否播放蝗虫
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ApplyPlayLocust")]
        public async Task<MessageModel<dynamic>> ApplyPlayLocust()
        {
            long uid = _user.ID;
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            if (uid == 0)
            {
                result.success = false;
                result.code = 1003;
                return result;
            }

            var appleConsumeData = await _iuserappleconsumeservices.GetUserAppleConsumeByType(uid,1);
            if (appleConsumeData == null || appleConsumeData.status == null)
            {
                result.code = 22001;
                result.success = false;
                return result;
            }

            result.code = 0;
            result.success = true;
            result.response = new
            {
                status = appleConsumeData.status
            };

            return result;
        }

        /// <summary>
        /// 请求苹果状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ApplyAppleStatus")]
        public async Task<MessageModel<dynamic>> ApplyAppleStatus()
        {
            long uid = _user.ID;
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            if (uid == 0)
            {
                result.success = false;
                result.code = 1003;
                return result;
            }

            var appStatusData = await _iuserapplestatusservices.GetUserAppleStatus(uid);
            if (appStatusData == null || appStatusData.Status == null)
            {
                result.code = 22001;
                result.success = false;
                return result;
            }

            result.code = 0;
            result.success = true;
            result.response = new
            {
                status = appStatusData.Status
            };

            return result;
        }

        /// <summary>
        /// 修改苹果状态
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("updateAppleStatus")]
        public async Task<MessageModel<dynamic>> updateAppleStatus(int status)
        {
            long uid = _user.ID;
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            if (uid == 0)
            {
                result.success = false;
                result.code = 1003;
                return result;
            }

            var appStatusData = await _iuserapplestatusservices.GetUserAppleStatus(uid);
            if (appStatusData == null || appStatusData.Status == null)
            {
                result.code = 22001;
                result.success = false;
                return result;
            }
            appStatusData.Status = status;
            bool b= await _iuserapplestatusservices.UpdateUserAppleStatus(appStatusData);
            if (!b) {
                result.code = -1;
                result.success = false;
                result.response = new
                {
                    status = appStatusData.Status
                };
            }
            result.code = 0;
            result.success = true;
            result.response = new
            {
                status = appStatusData.Status
            };

            return result;
        }
        /// <summary>
        /// 请求扩建农场
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ApplyFE")]
        public async Task<MessageModel<dynamic>> ApplyFE()
        {
            long uid = _user.ID;
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            if (uid == 0)
            {
                result.success = false;
                result.code = 1003;
                return result;
            }

            var dpeData = await _idpeservices.GetDPEData(uid);
            if (dpeData == null || dpeData.amount <= 0)
            {
                result.code = 23001;
                result.success = false;
                return result;
            }

            var isFE = await _iuserapplestatusservices.GetUserAppleStatus(uid);
            if (isFE != null)
            {
                result.code = 23003;
                result.success = false;
                return result;
            }

            int num = await _iuserapplestatusservices.AddUserAppleStatus(new UserAppleStatus() { 
                uID = uid,
                amount = 1,
                Status = 1 
            });

            if (num <= 0)
            {
                result.code = 23002;
                result.success = false;
                return result;
            }

            result.code = 0;
            result.success = true;
            result.response = new
            {
                status = 1
            };

            return result;
        }

        [HttpPost]
        [Route("FridensListConvertToViewModelList")]
        public List<FriendsViewModel> FridensListConvertToViewModelList(List<Friends> friendslist)
        {
            List<FriendsViewModel> list = new List<FriendsViewModel>();
            foreach (var item in friendslist)
            {
                list.Add(FridensConvertToViewModel(item));
            }
            return list;
        }

        [HttpPost]
        [Route("FridensConvertToViewModel")]
        public FriendsViewModel FridensConvertToViewModel(Friends friends)
        {
            FriendsViewModel viewModel = new FriendsViewModel();
            viewModel.uid = friends.uId.ToString();
            viewModel.nickname = friends.uNickName;
            viewModel.num = friends.PCount;
            viewModel.sex = friends.sex;
            return viewModel;
        }



        /// <summary>
        /// 蘋果生成記錄
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAppleRecord")]
        public async Task<MessageModel<dynamic>> GetAppleRecord()
        {

            var result = await _isplitrecordsservices.Query(x=> x.uID == _user.ID);
            return new MessageModel<dynamic>
            {
                success = true,
                msg = "",
                code = 0,
                response = (from item in result
                            select new
                            {
                               item
                            })
            };
            
        }


        /// <summary>
        /// 游戲接口數據整合
        /// </summary>
        /// <returns>用户信息</returns>
        [HttpPost]
        [Route("GetUserInfoAll")]
        public async Task<MessageModel<dynamic>> GetUserInfoAll()
        {
            var user = await _iuserinfoservices.GetUserInfo(_user.ID);
            var friendsData = await _ifriendsservices.GetFriends(_user.ID);

            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = new
                {
                    userdata = new
                    {
                        apple = user.DPE.Value,
                        autonym = user.isSetIDNumber.Value ? 1 : 0,
                        coin = user.IRP.Value,
                        coin_location = user.addr,
                        create_time = DateHelper.GetCreatetime(user.uCreateTime),
                        farmers = user.uStatus,
                        gold = user.EP.Value,
                        gold_zu = user.AD.Value,
                        lv_name = user.honorLevel.Value,
                        manure = user.Manure.Value,
                        nickname = user.uNickName,
                        seed = user.SP.Value,
                        sex = user.sex.Value,
                        signin = user.Signin,
                        uid = user.uID.ToString(),
                        friend = user.friends.ObjToInt(),
                        rp = user.RP.Value,
                        lprofit = user.LProfit.Value,
                        rprofit = user.RProfit.Value
                    },
                    friendlist = FridensListConvertToViewModelList(friendsData),
                    applestatus = ApplyAppleStatus().Result.response
                }
            };
        }

    }
}
