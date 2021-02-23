
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
using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.IServices;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SqlSugar;
using StackExchange.Redis;

namespace DPE.Core.Controllers
{

    /// <summary>
    /// EP 金币 酬金 管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/ep")]
    [Authorize(Permissions.Name)]
    public class EPController : Controller
    {

        private readonly IEPServices _EPServices;
        private readonly IEPexchangeServices _iepexchangeservices;
        private readonly IEPRecordsServices _ieprecordsservices;
        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;
        readonly ISysUserInfoServices _isysuserinfoservice;

        readonly IExchangeTotalServices _iexchangetotalservices;

        readonly IUserComplaintServices _iusercomplaintservices;

        readonly ISysUserInfoServices _sysUserInfoServices;

        readonly ISettingsServices _settingsdal;

        private readonly IUnitOfWork _iunitofwork;

        private readonly IEPRecordsRepository _ePRecordsdal;

        private readonly IEPexchangeRepository _exchangedal;

        private readonly ICompanyProfitServices _icompanyprofitservices;


        public EPController(IExchangeTotalServices iexchangetotalservices, ISysUserInfoServices isysuserinfoservice, IEPRecordsServices ieprecordsservices,
            IEPServices epspservices, IUser user, IEPexchangeServices iepexchangeservices, IUserInfoServices userInfoServices
            , IUserComplaintServices iusercomplaintservices, ISysUserInfoServices sysUserInfoServices, 
            ISettingsServices settingsdal, IUnitOfWork iunitofwork, IEPRecordsRepository ePRecordsdal,
            IEPexchangeRepository exchangedal, ICompanyProfitServices icompanyprofitservices)
        {
            _EPServices = epspservices;
            this._user = user;
            _userInfoServices = userInfoServices;
            _iepexchangeservices = iepexchangeservices;
            _ieprecordsservices = ieprecordsservices;
            _isysuserinfoservice = isysuserinfoservice;
            _iexchangetotalservices = iexchangetotalservices;
            _iusercomplaintservices = iusercomplaintservices;
            _sysUserInfoServices = sysUserInfoServices;
            _settingsdal = settingsdal;
            _iunitofwork = iunitofwork;
            _ePRecordsdal = ePRecordsdal;
            _exchangedal = exchangedal;
            _icompanyprofitservices = icompanyprofitservices;
        }

        /// <summary>
        /// 获取EP数据根据 uid
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEPByUid")]
        public async Task<MessageModel<EP>> GetEPByUid()
        {
            long uid = _user.ID;
            var data = new MessageModel<EP>();

            var userinfo = await _EPServices.QueryById(uid);
            if (userinfo != null)
            {
                data.response = userinfo;
                data.success = true;
            }

            return data;
        }

        /// <summary>
        /// 仓里金币列表记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEPExchange")]
        public async Task<MessageModel<ExchangeViewModels>> GetEPExchange(string language = "cn")
        {

            var spinfo = await _iepexchangeservices.GetEPExchange(_user.ID);

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

        /// <summary>
        /// 仓里酬金列表记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEPGoldExchange")]
        public async Task<dynamic> GetEPGoldExchange()
        {
            var spinfo = await _iepexchangeservices.GetEPGoldExchange(_user.ID);
            return new
            {
                success = true,
                msg = "",
                code = 0,
                response = new
                {
                    num = spinfo.Count,
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


        /// <summary>
        /// 出售EP
        /// </summary>
        /// <param name="ePSellParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EPSell")]
        public async Task<MessageModel<string>> EPSell(string jsondata)
        {

            var ePSellParams = JsonConvert.DeserializeObject<EPSellParamsModel>(jsondata);
            ePSellParams.uID = _user.ID;

            MessageModel<string> result = new MessageModel<string>();
            //判断交易密码

            return await _EPServices.EPSell(ePSellParams);
        }

        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="ePSellParams"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEpSellWeb")]
        public async Task<MessageModel<dynamic>> GetEpSellWeb(decimal amount,int type,string pwd,string goolekey )
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try 
            {
                var data = await _sysUserInfoServices.checkTrad(_user.ID, pwd);
                if (data == null)
                {
                    //交易密码错误
                    result.code = 61004;
                    result.success = false;
                    result.msg = "交易密码错误";
                    return result;
                }

                //var trangode = await _sysUserInfoServices.checkGoogleKey(_user.ID, goolekey);
                //if (trangode == null)
                //{
                //    //谷歌验证错误
                //    result.code = 61005;
                //    result.success = false;
                //    result.msg = "谷歌验证码校验失败";
                //    return result;
                //}

                var settings = (await _settingsdal.Query()).FirstOrDefault();
                decimal rate = 0;
                decimal usdRate = 6.95M;
                decimal cnRate = 6.175M;
                if (settings != null)
                {
                    usdRate = settings.ComRate.ObjToDecimal();
                    cnRate = settings.CNYRate.ObjToDecimal();
                    rate = cnRate / usdRate;
                }

                //手续费0.02
                long sxf = Convert.ToInt64((amount - (amount * Convert.ToDecimal(1 - 0.02)))) ;
                amount = amount * Convert.ToDecimal(1-0.02);

                //手续费 


                var userdata =await _userInfoServices.GetUserInfo(_user.ID);
                if (amount > userdata.EP) 
                {
                    result.code = 61005;
                    result.success = false;
                    result.msg = "提现金额不足";
                    return result;

                }


             

               EPRecords epmRecordsModel = new EPRecords()
                {
                    uID =_user.ID,
                    amount = 0-amount,
                    rate = rate,
                    RMBrate = cnRate,
                    USDTrate = usdRate,
                    status = 1,
                    statusName = "提现中",
                    usdtAddress = data.addr,
                   // trcAddress = ePSellParams.trcAddress,
                    receiveType = type,
                    phone ="",
                    createTime = DateTime.Now,
                    alipayaccount=userdata.alipayaccount,
                    alipayname = userdata.alipayname,
                    bankaddr = userdata.bankaddr,
                    bankidcard = userdata.bankidcard,
                    bankname = userdata.bankname,
                };

                _iunitofwork.BeginTran();
                var exrowsid = await _ePRecordsdal.Add(epmRecordsModel);
                if (exrowsid > 0)
                {
                    EPexchange pexchange = new EPexchange()
                    {
                        uID = _user.ID,
                        amount = 0 - amount,
                        lastTotal = userdata.EP,
                        fromID = 0,
                        recordID = exrowsid,
                        scount = 0,
                        stype = 90,
                        price = 0,
                        createTime = DateTime.Now,
                        remark = "提现EP"
                    };

                    await _icompanyprofitservices.Add(new CompanyProfit() { 
                        amount = sxf,createTime=DateTime.Now,pType=1, eType= "平台手续费",fromID=_user.ID  });

                    var changerowid = await _exchangedal.Add(pexchange);
                    if (changerowid > 0)
                    {
                        var epentity = await _EPServices.QueryById(_user.ID);
                        if (epentity != null)
                        {
                            epentity.amount = epentity.amount - amount;
                            await _EPServices.Update(epentity);

                            _iunitofwork.CommitTran();//提交事务
                        }
                        else
                        {
                            _iunitofwork.RollbackTran();//事务回滚
                            result.code = 50006;
                            result.success = false;
                            result.msg = "提交失败请稍后再试！";
                            return result;
                        }
                    }
                    else
                    {
                        _iunitofwork.RollbackTran();//事务回滚
                        result.code = 50006;
                        result.success = false;
                        result.msg = "提交失败请稍后再试！";
                        return result;
                    }

                }
                else
                {
                    _iunitofwork.RollbackTran();//事务回滚
                    result.code = 50006;
                    result.success = false;
                    result.msg = "提交失败请稍后再试！";
                    return result;
                }

                result.code = 0;
                result.success = true;
                result.msg = "提现成功！";
                return result;
            } 
            catch (Exception ex)
            {
                string a = ex.Message;
                result.code = 20001;
                result.success = false;
                result.msg = "提现失败请稍后再试！";
                return result;
            }
        }



        /// <summary>
        /// EP购买
        /// </summary>
        /// <param name="eid">ep id </param>
        /// <param name="buyid">购买 id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("EPBuy")]
        public async Task<MessageModel<string>> EPBuy(long eid)
        {
            var result = new MessageModel<string>();

            if (string.IsNullOrEmpty(eid.ToString()))
            {
                string tmpeid = HttpContext.Request.Form["eid"];
                eid = int.Parse(tmpeid);
            }
            var eprecord = await _ieprecordsservices.QueryById(eid);
            if (eprecord == null)
            {
                //不存在该记录
                result.code = 51001;
                result.success = false;
                result.msg = "不存在该记录";
                return result;
            }

            if (eprecord.status != 1)
            {
                //不是出售状态
                result.code = 51004;
                result.success = false;
                result.msg = "不是出售状态";
                return result;
            }

            var buyuser = await _userInfoServices.GetUserInfo(_user.ID);


            if (buyuser == null)
            {
                //购买者不存在
                result.code = 51002;
                result.success = false;
                result.msg = "购买者不存在";
                return result;
            }
            if (!Convert.ToBoolean(buyuser.isService))
            {
                //支付者不存在
                result.code = 51002;
                result.success = false;
                result.msg = "不是服务中心无法交易";
                return result;
            }
            //判断交易密码 

            //判断gogole验证

            //查询账号状态是否能交易

            eprecord.buyId = _user.ID;
            eprecord.buyTime = DateTime.Now;
            //2 购买
            eprecord.status = 2;
            eprecord.statusName = "交易中";

            var resultisfig = await _ieprecordsservices.Update(eprecord);
            if (!resultisfig)
            {
                //交易失败
                result.code = 51006;
                result.success = true;
                result.msg = "交易失败";
                return result;
            }

            result.code = 0;
            result.success = true;
            return result;
        }

        /// <summary>
        /// EP购买
        /// </summary>
        /// <param name="eid">ep id </param>
        /// <param name="payid">支付 id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("EPPay")]
        public async Task<MessageModel<string>> EPPay(long eid)
        {
            var result = new MessageModel<string>();
            if (string.IsNullOrEmpty(eid.ToString()))
            {
                string tmpeid = HttpContext.Request.Form["eid"];
                eid = int.Parse(tmpeid);
            }
            var eprecord = await _ieprecordsservices.QueryById(eid);

            if (eprecord == null)
            {
                //不存在该记录
                result.code = 51001;
                result.success = false;
                result.msg = "不存在该记录";
                return result;
            }

            if (eprecord.status != 2)
            {
                //不是购买状态
                result.code = 51005;
                result.success = false;
                result.msg = "不是购买状态";
                return result;
            }

            var payuser = await _userInfoServices.GetUserInfo(_user.ID);


            if (payuser == null)
            {
                //支付者不存在
                result.code = 51002;
                result.success = false;
                result.msg = "支付者不存在";
                return result;
            }

            if (!Convert.ToBoolean(payuser.isService))
            {
                //支付者不存在
                result.code = 51002;
                result.success = false;
                result.msg = "不是服务中心无法交易";
                return result;
            }


            //查询账号状态是否能交易
            eprecord.payTime = DateTime.Now;
            //2 购买
            eprecord.statusName = "已支付";
            eprecord.status = 4;
            var buyisfig = await _ieprecordsservices.Update(eprecord);
            if (buyisfig)
            {
                result.code = 0;
                result.success = true;
            }
            else
            {
                result.code = -1;
                result.success = false;
                result.msg = "交易失败";
            }


            return result;
        }


        [HttpPost]
        [Route("EPFinish")]
        public async Task<MessageModel<string>> EPFinish(long eid)
        {
            var result = new MessageModel<string>();

            if (_user.ID == 0)
            {
                result.code = 60001;
                result.msg = "用户身份已过期，请重新登录";
                result.success = false;
                return result;
            }
            if (string.IsNullOrEmpty(eid.ToString()))
            {
                string tmpeid = HttpContext.Request.Form["eid"];
                eid = int.Parse(tmpeid);
            }
            var eprecord = await _ieprecordsservices.QueryById(eid);
            if (eprecord == null)
            {
                //不存在该记录
                result.code = 51001;
                result.success = false;
                result.msg = "不存在该记录";
                return result;
            }

            if (eprecord.status != 4)
            {
                //不是完成状态
                result.code = 51005;
                result.success = false;
                result.msg = "不是完成状态";
                return result;
            }

            //2 已完成
            eprecord.statusName = "已完成";
            eprecord.status = 8;
            eprecord.confirmTime = DateTime.Now;
            var buyisfig = await _ieprecordsservices.Update(eprecord);
            if (buyisfig)
            {
                var ep = (await _EPServices.Query(a => a.uID == eprecord.buyId));
                if (ep.Count > 0)
                {
                    EPexchange ex = new EPexchange();
                    ex.uID = eprecord.buyId;
                    ex.amount = Math.Abs(eprecord.amount.Value);
                    ex.createTime = DateTime.Now;
                    ex.fromID = eprecord.uID;
                    ex.recordID = eprecord.id;
                    ex.lastTotal = ep[0].amount;
                    ex.stype = 15;
                    ex.remark = "购买EP";
                    var exc = await _iepexchangeservices.Add(ex);
                    //更新ep
                    if (exc > 0)
                    {
                        ep[0].amount = ep[0].amount + Math.Abs(eprecord.amount.Value);
                        await _EPServices.Update(ep[0]);
                    }

                }
            }
            result.code = 0;
            result.success = true;
            return result;
        }


        /// <summary>
        /// EP记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageSize">每页显示行数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="cktype">类型查询  ep rp sp dpe  irp  ad manure</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEPExchangeList")]
        public async Task<MessageModel<dynamic>> GetEPExchangeList(int type, int pageSize, int pageIndex, string cktype)
        {
            pageSize = pageSize == 0 ? 10 : pageSize;
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            var result = new PageModel<ExchangeTotal>();
            if (string.IsNullOrEmpty(cktype))
            {
                switch (type)
                {
                    case 1:
                        result = await _iexchangetotalservices.QueryPage(x => x.uid == _user.ID, pageIndex, pageSize, " createTime desc");
                        break;
                    case 2:
                        result = await _iexchangetotalservices.QueryPage(x => x.uid == _user.ID && x.amount > 0, pageIndex, pageSize, " createTime desc");
                        break;
                    case 3:
                        result = await _iexchangetotalservices.QueryPage(x => x.uid == _user.ID && x.amount < 0, pageIndex, pageSize, " createTime desc");
                        break;
                    default:
                        result = await _iexchangetotalservices.QueryPage(x => x.uid == _user.ID, pageIndex, pageSize, " createTime desc");
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case 1:
                        result = await _iexchangetotalservices.QueryPage(x => x.uid == _user.ID && x.extype == cktype, pageIndex, pageSize, " createTime desc");
                        break;
                    case 2:
                        result = await _iexchangetotalservices.QueryPage(x => x.uid == _user.ID && x.amount > 0 && x.extype == cktype, pageIndex, pageSize, " createTime desc");
                        break;
                    case 3:
                        result = await _iexchangetotalservices.QueryPage(x => x.uid == _user.ID && x.amount < 0 && x.extype == cktype, pageIndex, pageSize, " createTime desc");
                        break;
                    default:
                        result = await _iexchangetotalservices.QueryPage(x => x.uid == _user.ID && x.extype == cktype, pageIndex, pageSize, " createTime desc");
                        break;
                }
            }

            return new MessageModel<dynamic>
            {
                response = (from item in result.data
                            orderby item.createTime descending
                            select new
                            {
                                datacount = result.dataCount,
                                id = item.uid,
                                img = "head01",
                                date = item.createTime.Value.ToString("yy/MM/dd HH:mm:ss"),
                                item.lastTotal,
                                item.amount,
                                item.remark
                            })

            };
        }


        [HttpPost]
        [Route("GetEPRecordLists")]
        public async Task<MessageModel<dynamic>> GetEPRecordLists(int type, int pageSize, int pageIndex,int status)
        {
            pageSize = pageSize == 0 ? 10 : pageSize;
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            var result = new PageModel<EPRecords>();

            switch (type)
            {
                case 1:
                    result = await _ePRecordsdal.QueryPage(x => x.uID == _user.ID , pageIndex, pageSize, " createTime desc");
                    break;
                case 2:
                    result = await _ePRecordsdal.QueryPage(x => x.uID == _user.ID && x.status == 2, pageIndex, pageSize, " createTime desc");
                    break;
                case 3:
                    result = await _ePRecordsdal.QueryPage(x => x.uID == _user.ID && x.status == 1, pageIndex, pageSize, " createTime desc");
                    break;
                default:
                    result = await _ePRecordsdal.QueryPage(x => x.uID == _user.ID, pageIndex, pageSize, " createTime desc");
                    break;
            }

            return new MessageModel<dynamic>
            {
                response = (from item in result.data
                            orderby item.createTime descending
                            select new
                            {
                                datacount = result.dataCount,
                                id = item.uID,
                                img = "head01",
                                date = item.createTime.Value.ToString("yy/MM/dd HH:mm:ss"),
                                item.amount,
                                status=item.status
                            })

            };
        }


        /// <summary>
        /// EP出售记录  1 出售中 2 交易中 4 已支付    8 已完成
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageSize">每页显示行数</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEPRecordList")]
        public async Task<MessageModel<dynamic>> GetEPRecordList(int type, int pageSize = 10, int pageIndex = 1)
        {

            MessageModel<dynamic> results = new MessageModel<dynamic>();
            if (_user.ID == 0)
            {
                results.code = 60001;
                results.msg = "用户身份已过期，请重新登录";
                results.success = false;
                return results;
            }
            pageSize = pageSize == 0 ? 20 : pageSize;
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            bool isfig = (type == 4 || type == 2) ? true : false;


            var result = new PageModel<EPRecords>();
            var userinfo = await _isysuserinfoservice.QueryById(_user.ID);

            if (isfig)
            {
                result = await _ieprecordsservices.QueryPage(x => (x.buyId == _user.ID || x.uID == _user.ID) && (x.status == 2 || x.status == 4), pageIndex, pageSize, " createTime desc");
            }
            else
            {
                //判断当前是否为服务中心
                if (userinfo.isService.ObjToBool())
                {
                    if (type == 8)
                    {
                        result = await _ieprecordsservices.QueryPage(x => x.status == type && x.buyId == _user.ID, pageIndex, pageSize, " createTime desc");
                    }
                    else
                    {
                        result = await _ieprecordsservices.QueryPage(x => x.status == type, pageIndex, pageSize, " createTime desc");
                    }
                }
                else
                {
                    result = await _ieprecordsservices.QueryPage(x => x.status == type, pageIndex, pageSize, " createTime desc");
                }

            }
            var res = (from item in result.data
                       orderby item.createTime descending
                       select new
                       {
                           id = item.id,
                           euid = item.uID,
                           price = item.amount,
                           status = item.status,
                           name = item.statusName,
                           date = item.createTime
                       }).ToList<dynamic>();

            List<dynamic> response = new List<dynamic>();
            foreach (var item in res)
            {
                response.Add(new
                {
                    item.id,
                    item.euid,
                    img = (await _isysuserinfoservice.QueryById(item.euid)).uHeadImgUrl,
                    item.price,
                    item.status,
                    item.name,
                    item.date
                });
            }
            return new MessageModel<dynamic>
            {
                response = response
            };
        }

        /// <summary>
        /// GetEPUserSell
        /// </summary>
        /// <param name="pageSize">每页显示行数</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEPUserSell")]
        public async Task<MessageModel<dynamic>> GetEPUserSell(int pageSize, int pageIndex)
        {
            pageSize = pageSize == 0 ? 10 : pageSize;
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            var result = new PageModel<ExchangeTotal>();
            result = await _iexchangetotalservices.QueryPage(null, pageIndex, pageSize, " createTime desc");
            return new MessageModel<dynamic>
            {

                response =
                new
                {
                    datacount = result.dataCount,
                    data = (from item in result.data
                            orderby item.createTime descending
                            select new
                            {

                                id = item.uid,
                                img = "head01",
                                date = item.createTime.Value.ToString("yy/MM/dd HH:mm:ss"),
                                item.lastTotal,
                                item.amount,
                                item.remark
                            })
                }
            };
        }

        //财务管理 财务挂单
        [HttpPost]
        [Route("GetEPFinanceSell")]
        public async Task<MessageModel<dynamic>> GetEPFinanceSell(int pageSize, int pageIndex)
        {
            pageSize = pageSize == 0 ? 10 : pageSize;
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            var result = new PageModel<EP>();
            result = await _EPServices.QueryPage(null, pageIndex, pageSize, " uID ");

            return new MessageModel<dynamic>
            {
                response =
                new
                {
                    datacount = result.dataCount,
                    data = (from item in result.data
                            select new
                            {
                                uid = item.uID,
                                amount = _ieprecordsservices.Query(x => x.uID == item.uID).Result.Sum(x => x.amount),
                                epamount = item.amount
                            })
                }
            };
        }


        /// <summary>
        /// EP/RP转换成RP/SP
        /// </summary>
        /// <param name="oType">原类型:EP、RP</param>
        /// <param name="dType">目标类型:RP、SP</param>
        /// <param name="amount">金额</param>
        /// <param name="tpwd"></param>
        /// <param name="gcode"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TransOtherType")]
        public async Task<MessageModel<string>> TransOtherType(string oType, string dType, decimal amount, string tpwd, string gcode)
        {

            MessageModel<string> result = new MessageModel<string>();
            //判断交易密码
            var tranpwd = await _isysuserinfoservice.checkTrad(_user.ID, tpwd);
            if (tranpwd == null)
            {
                //交易密码错误
                result.code = 61004;
                result.success = false;
                result.msg = "交易密码错误";
                return result;
            }


            //判断谷歌验证
            //var trangode = await _isysuserinfoservice.checkGoogleKey(_user.ID, gcode);
            //if (trangode == null)
            //{
            //    //谷歌验证错误
            //    result.code = 61005;
            //    result.success = false;
            //    result.msg = "谷歌验证错误";
            //    return result;
            //}


            return await _EPServices.EPTransOtherType(oType, dType, tpwd, gcode, amount, _user.ID);
        }
        /// <summary>
        /// EP/RP转出
        /// </summary>
        /// <param name="oType">原类型:EP、RP</param>
        /// <param name="dType">目标类型:RP、SP</param>
        /// <param name="password">密码</param>
        /// <param name="amount">金额</param>
        /// <returns></returns>
        [HttpPost]
        [Route("EPTransOtherType")]
        public async Task<MessageModel<string>> TransToUser(string oType, long userID, string googlekey, string password, decimal amount)
        {

            return await _EPServices.TransToUser(oType, userID, password, googlekey, amount, _user.ID);
        }

        /// <summary>
        /// RP转出 
        /// </summary>
        /// <param name="touid"></param>
        /// <param name="amount"></param>
        /// <param name="tpwd"></param>
        /// <param name="gcode"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EPToEexchange")]
        public async Task<MessageModel<string>> EPToEexchange(long touid, decimal amount, string tpwd, string gcode)
        {
            var result = new MessageModel<string>();
            try
            {

                if (_user.ID == 0)
                {
                    //用户不存在
                    result.code = 61001;
                    result.success = false;
                    result.msg = "登录失效，请重新登陆！";
                    return result;
                }


                if (_user.ID == touid)
                {
                    //不能往自己的账户转EP
                    result.code = 61007;
                    result.success = false;
                    result.msg = "不能往自己账号转EP";
                    return result;
                }


                var user = await _userInfoServices.GetUserInfo(_user.ID);

                if (user.EP < amount)
                {
                    //RP不足
                    result.code = 61003;
                    result.success = false;
                    result.msg = "EP不足";
                    return result;
                }

                var touser = await _userInfoServices.GetUserInfo(touid);
                if (touser == null)
                {
                    //RP转出用户不存在
                    result.code = 61002;
                    result.success = false;
                    result.msg = "EP转出用户不存在";
                    return result;
                }

                //判断交易密码
                var tranpwd = await _isysuserinfoservice.checkTrad(_user.ID, tpwd);
                if (tranpwd == null)
                {
                    //交易密码错误
                    result.code = 61004;
                    result.success = false;
                    result.msg = "交易密码错误";
                    return result;
                }

                //判断谷歌验证
                var trangode = await _isysuserinfoservice.checkGoogleKey(_user.ID, gcode);
                if (trangode == null)
                {
                    //谷歌验证错误
                    result.code = 61005;
                    result.success = false;
                    result.msg = "谷歌验证码错误";
                    return result;
                }


                return await _iepexchangeservices.ProcessTransformToUser(_user.ID, "EP", amount, touser.uID);
            }
            catch
            {
                result.code = 61006;
                result.success = false;
                result.msg = "服务器繁忙，请稍后重试";
                return result;
            }

        }


        //获取待售信息
        [HttpPost]
        [Route("GetEPExchangeById")]
        public async Task<MessageModel<dynamic>> GetEPExchangeById(int id)
        {
            string sql = string.Format(" select a.status,a.payTime,a.confirmTime,a.buyTime,a.uID,a.buyId,amount ,createTime,usdtAddress,trcAddress,phone,ISNULL(b.tcount,0) tcount,ISNULL(c.fcount,0) fcount,ISNULL(d.appealcount,0) appealcount," +
                "USDTrate,RMBrate,abs(amount)*RMBrate as MoneyCNY,abs(amount)*RMBrate/USDTrate as MoneyUSDT  from EPRecords  a " +
                " left join (select COUNT(1) tcount,uID from EPRecords  group by uID ) b on b.uID=a.uID " +
                  " left join (select COUNT(1) appealcount,complaintuid from UserComplaint  group by complaintuid ) d on b.uID=a.uID " +
                " left join (select COUNT(1) fcount,uID from EPRecords where status=8  group by uID ) c on c.uID=a.uID where id={0}", id);
            var result = await _ieprecordsservices.QuerySql(sql);


            var res = (from item in result
                       orderby item.createTime descending
                       select new
                       {
                           uid = item.uID,
                           item.buyId,
                           item.status,
                           amount = Math.Abs(item.amount.ObjToDecimal()),
                           item.createTime,
                           item.usdtAddress,
                           item.phone,
                           item.USDTrate,
                           item.trcAddress,
                           item.RMBrate,
                           MoneyCNY = Math.Abs(item.amount.ObjToDecimal() * item.RMBrate.Value),
                           MoneyUSDT = Math.Abs(item.amount.ObjToDecimal() * item.RMBrate.Value / item.USDTrate.Value),
                           item.tcount,
                           item.fcount,
                           item.appealcount,
                           item.buyTime,
                           item.payTime,
                           item.confirmTime

                       }).ToList<dynamic>();

            List<dynamic> response = new List<dynamic>();
            foreach (var item in res)
            {
                response.Add(new
                {
                    img = (await _isysuserinfoservice.QueryById(item.uid)).uHeadImgUrl,
                    item.uid,
                    item.buyId,
                    item.status,
                    item.amount,
                    item.createTime,
                    item.usdtAddress,
                    item.trcAddress,
                    item.phone,
                    item.USDTrate,
                    item.RMBrate,
                    item.MoneyCNY,
                    item.MoneyUSDT,
                    item.tcount,
                    item.fcount,
                    item.appealcount,
                    item.buyTime,
                    item.payTime,
                    item.confirmTime
                });
            }
            return new MessageModel<dynamic>
            {
                response = response
            };
        }




        //EP转出记录
        [HttpPost]
        [Route("GetAdminEPExchangeList")]
        public async Task<MessageModel<dynamic>> GetAdminEPExchangeList()
        {


            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                if (_user.ID == 0)
                {
                    result.code = 10001;
                    result.msg = "用户信息已过期，请重新登陆";
                    result.success = false;
                    return result;
                }

                int pageindex = Convert.ToInt32(HttpContext.Request.Form["pageindex"]);
                int pagesize = Convert.ToInt32(HttpContext.Request.Form["pagesize"]);
                string key = HttpContext.Request.Form["key"];

                if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                {
                    key = "";
                }

                pagesize = pagesize == 0 ? 20 : pagesize;
                pageindex = pageindex == 0 ? 1 : pageindex;

                var data = await _iepexchangeservices.QueryPage(x => x.stype == 14 && x.amount < 0 &&
              (x.uID.ToString().Contains(key) || x.fromID.ToString().Contains(key) || x.recordID.ToString().Contains(key) || x.remark.Contains(key)), pageindex, pagesize, " createTime DESC ");

                result.response = data;
                result.code = 200;
                result.success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }

        }


        [HttpPost]
        [Route("GetAdminEPExchange")]
        public async Task<MessageModel<dynamic>> GetAdminEPExchange()
        {


            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                if (_user.ID == 0)
                {
                    result.code = 10001;
                    result.msg = "用户信息已过期，请重新登陆";
                    result.success = false;
                    return result;
                }

                int pageindex = Convert.ToInt32(HttpContext.Request.Form["pageindex"]);
                int pagesize = Convert.ToInt32(HttpContext.Request.Form["pagesize"]);
                string key = HttpContext.Request.Form["key"];
                string stype = HttpContext.Request.Form["stype"];

                if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                {
                    key = "";
                }

                if (string.IsNullOrEmpty(stype) || string.IsNullOrWhiteSpace(stype))
                {
                    stype = "";
                }
                pagesize = pagesize == 0 ? 20 : pagesize;
                pageindex = pageindex == 0 ? 1 : pageindex;
                var data = await _iepexchangeservices.QueryPage(x => x.stype.ToString().Contains(stype) &&
              (x.uID.ToString().Contains(key) || x.remark.Contains(key)), pageindex, pagesize, " createTime DESC ");

                result.response = data;
                result.code = 200;
                result.success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }

        }


        //退回
        [HttpPost]
        [Route("RollBackTran")]
        public async Task<MessageModel<dynamic>> RollBackTran()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                if (_user.ID == 0)
                {
                    result.code = 10001;
                    result.msg = "用户信息已过期，请重新登陆";
                    result.success = false;
                    return result;
                }
                string id = HttpContext.Request.Form["id"];
                string type = HttpContext.Request.Form["type"];

                if (string.IsNullOrEmpty(id))
                {
                    result.code = 10001;
                    result.msg = "无法找到记录";
                    result.success = false;
                    return result;
                }

                var data = await _iepexchangeservices.RoolBackThisTran(Convert.ToInt64(id), type);
                result.code = data ? 200 : 500;
                result.success = data ? true : false;
                return result;
            }
            catch (Exception ex)
            {
                result.code = 70001;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }
        }



        [HttpPost]
        [Route("GetAdminEPRecordList")]
        public async Task<MessageModel<dynamic>> GetAdminEPRecordList(int type, int pageSize = 10, int pageIndex = 1)
        {

            MessageModel<dynamic> results = new MessageModel<dynamic>();
            if (_user.ID == 0)
            {
                results.code = 60001;
                results.msg = "用户身份已过期，请重新登录";
                results.success = false;
                return results;
            }
            int pageindex = Convert.ToInt32(HttpContext.Request.Form["pageindex"]);
            int pagesize = Convert.ToInt32(HttpContext.Request.Form["pagesize"]);
            string key = HttpContext.Request.Form["key"];
            string status = HttpContext.Request.Form["status"];

            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            pagesize = pagesize == 0 ? 20 : pagesize;
            pageindex = pageindex == 0 ? 1 : pageindex;



            var zzlist =await _iepexchangeservices.QuerySql("select  * from tmpuid");
            List<long?> uidlist = new List<long?>();
            if (zzlist.Count > 0) 
            {
                uidlist = zzlist.Select(x => x.uID).ToList();
            }

            var data = await _ieprecordsservices.QueryPage(x =>  !uidlist.Contains(x.uID) &&
           (x.status.ToString().Contains(status) && (x.uID.ToString().Contains(key) || x.buyId.ToString().Contains(key) || x.id.ToString().Contains(key))), pageindex, pagesize, " createTime DESC ");

            return new MessageModel<dynamic>
            {
                response = data
            };
        }


        //投诉
        [HttpPost]
        [Route("Usercomplaint")]
        public async Task<MessageModel<dynamic>> AdminUsercomplaint(int id, long complaintuid)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                if (_user.ID == 0)
                {
                    result.code = 10001;
                    result.msg = "用户信息已过期，请重新登陆";
                    result.success = false;
                    return result;
                }
                if (id == 0)
                {
                    result.code = 10001;
                    result.msg = "无法找到记录";
                    result.success = false;
                    return result;
                }
                var quresult = await _iusercomplaintservices.Query(x => x.uid == _user.ID && x.complaintuid == complaintuid && x.complaintuidtype.Equals("ep交易"));
                if (quresult.Count > 0)
                {
                    result.code = 10001;
                    result.msg = "您已经投诉过该用户了，请勿重新投诉";
                    result.success = false;
                    return result;
                }
                else
                {
                    await _iusercomplaintservices.Add(new UserComplaint { uid = _user.ID, complaintuid = complaintuid, complaintuidtype = "ep交易", createtime = DateTime.Now });
                }
                result.code = 200;
                result.success = true;
                result.msg = "已收到您的投诉";
                return result;
            }
            catch (Exception ex)
            {
                result.code = 70001;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }
        }

    }
}