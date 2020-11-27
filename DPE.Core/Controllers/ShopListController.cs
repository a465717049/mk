using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DPE.Core.AuthHelper;
using DPE.Core.AuthHelper.OverWrite;
using DPE.Core.Common.Helper;
using DPE.Core.Common.HttpContextUser;
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

namespace DPE.Core.Controllers
{
    /// <summary>
    /// Shop 商店管理【无权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/Shop")]
    [Authorize(Permissions.Name)]
    public class ShopListController : Controller
    {


        private readonly IShopListServices _ishoplistservices;
        private readonly IUserGoodsListServices _iusergoodslistservices;
        readonly IUser _user;
        readonly ISysUserInfoServices _isysuserinfoservices;
        readonly IUserInfoServices _userInfoServices;
        readonly IEPServices _iepservices;
        readonly IEPexchangeServices _iepexchangeservices;


        private readonly IUnitOfWork _unitOfWork;

        public ShopListController(ISysUserInfoServices isysuserinfoservices,IUnitOfWork unitOfWork,IUser user, IShopListServices ishoplistservices, IUserGoodsListServices iusergoodslistservices, IUserInfoServices userInfoServices, IEPServices iepservices, IEPexchangeServices iepexchangeservices)
        {
            this._user = user;
            _userInfoServices = userInfoServices;
            _ishoplistservices = ishoplistservices;
            _iusergoodslistservices = iusergoodslistservices;
            _iepservices = iepservices;
            _iepexchangeservices = iepexchangeservices;
            _unitOfWork = unitOfWork;
            _isysuserinfoservices = isysuserinfoservices;
        }



        /// <summary>
        /// 商店列表 商品清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShopList")]
        public async Task<MessageModel<ShopListViewModels>> GetShopList(string language="cn")
        {
            //_user.ID
       //     var user = await _userInfoServices.GetUserInfo(_user.ID);
            var spinfo = await _ishoplistservices.Query();
            return new MessageModel<ShopListViewModels>()
            {
                success = true,
                msg = "",
                response = new ShopListViewModels()
                {
                    list = (from item in spinfo
                            orderby item.createTime descending
                            select new ShopListViewModelsList
                            {
                                id =  item.id.ObjToInt(),
                                name = item.pName,
                                num = item.pNum.ObjToInt(),
                                own_num = _iusergoodslistservices.GetUserShopOwnNum(_user.ID,item.id).Result.Sum(x=>x),
                                icon_url = item.pIcon,
                                price = Convert.ToInt32(item.price)
                            }).ToList()
                }
            };
        }



        /// <summary>
        /// 仓里商品订单列表记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserGoodsList")]
        public async Task<MessageModel<dynamic>> GetUserGoodsList(string language="cn")
        {
            //_user.ID

            var spinfo = await _iusergoodslistservices.GetUserGoodsList(_user.ID);

            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = new 
                {   
                    num=spinfo.Count,
                    list = (from item in spinfo
                            orderby item.id descending
                            select new ExchangeViewList
                            {
                                msg =string.Format("购买:{0}",(_ishoplistservices.Query(x=>x.id==item.shopID)).Result.First().pName),
                                time = DateHelper.GetCreatetime(Convert.ToDateTime(item.createTime))
                            }).ToList()
                }
            };


        }


        /// <summary>
        /// 购买商品 商品購買
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="name">姓名</param>
        /// <param name="iphone">手机号</param>
        /// <param name="source">地址</param> 
        /// <returns></returns>
        [HttpPost]
        [Route("BuyGoods")]
        public async Task<MessageModel<dynamic>> BuyGoods(long id, string name, string iphone, string source)
        {
            int tmpcode = 1004;
            MessageModel<dynamic> returnresult = new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                code = tmpcode,
                response = new
                {
                    id = id,
                    own_num = 0
                }
            };
            try
            {
                tmpcode = 0;
             
                var result = await _ishoplistservices.Query(x => x.id == id);
                var user = await _userInfoServices.GetUserInfo(_user.ID);
                _unitOfWork.BeginTran();

                ShopList shop = new ShopList();

                if (_user.ID == 0)
                {
                    tmpcode = 1003;
                }

                shop = result.First();
                //查詢商品貨量是否足夠
                if (shop.pNum <= 0)
                {
                    tmpcode = 16001;
                }

                //查詢是否有貨品
                if (result.Count == 0)
                {
                    tmpcode = 16002;
                }

                //金幣不足
                if (user.EP < shop.price)
                {
                    tmpcode = 16003;
                }

                if (tmpcode != 0) 
                {
                    returnresult.code = tmpcode;
                    return returnresult;
                }

                //金幣 EP
                UserGoodsList usergood = new UserGoodsList()
                {
                    num = 1,
                    address = source,
                    createTime = DateTime.Now,
                    phone = iphone,
                    shopID = id,
                    status = 0,
                    uID = _user.ID,
                    price = shop.price
                };

                var goodresult = await _iusergoodslistservices.Add(usergood);

                if (goodresult > 0)
                {
                    //商品適量減1
                    shop.pNum = shop.pNum - 1;
                    await _ishoplistservices.Update(shop);

                    //更新EP記錄 
                    var myep = await _iepservices.QueryById(_user.ID);
                    decimal tmpepamount = myep.amount.ObjToDecimal();
                    myep.amount = myep.amount - shop.price;
                    await _iepservices.Update(myep);

                    //插入ep記錄
                    await _iepexchangeservices.Add(new EPexchange()
                    {
                        amount = shop.price,
                        createTime = DateTime.Now,
                        fromID = _user.ID,
                        uID = _user.ID,
                        lastTotal = tmpepamount,
                        recordID = _user.ID,
                        remark = "購買商品",
                        price = shop.price,
                        scount = 0,
                        stype =99
                    });

                }
                else 
                {
                    _unitOfWork.RollbackTran();
                    tmpcode = 1004;
                    return returnresult;
                }

               
                returnresult= new MessageModel<dynamic>()
                {
                    success = true,
                    msg = "",
                    code = tmpcode,
                    response = new
                    {
                        id = id,
                        own_num = _iusergoodslistservices.GetUserShopOwnNum(_user.ID, id).Result.Sum(x => x)
                    }
                };
                _unitOfWork.CommitTran();

                return returnresult;
            }
            catch 
            {
                 
                _unitOfWork.RollbackTran();
                return returnresult;
            }
          
        }


        [HttpPost]
        [Route("BuyGoodsweb")]
        public async Task<MessageModel<dynamic>> BuyGoodsweb(long id)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                result.code = 10001;
                result.msg = "疫情影响暂未开放";
                result.success = false;
                return result;
            }
            catch
            {
                result.code = 10001;
                result.msg = "疫情影响暂未开放";
                result.success = false;
                return result;
            }

        }


        /// <summary>
        /// 商品明細
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShopDeatilList")]
        public async Task<MessageModel<dynamic>> GetShopDeatilList(long shopid)
        {
            //_user.ID
            //     var user = await _userInfoServices.GetUserInfo(_user.ID);
            var spinfo = await _ishoplistservices.Query(x=>x.id== shopid );
            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = new 
                {
                    list = spinfo
                }
            };
        }


        /// <summary>
        /// 商品模糊查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShopDeatilLike")]
        public async Task<MessageModel<dynamic>> GetShopDeatilLike(string key)
        {
            //_user.ID
            //     var user = await _userInfoServices.GetUserInfo(_user.ID);
            List<ShopList> spinfo = new List<ShopList>();

            if (string.IsNullOrEmpty(key))
            {
                spinfo = await _ishoplistservices.Query(x => x.pName.Contains(key) || x.pDesc.Contains(key) || x.id.ToString().Contains(key));
            }
            else 
            {
                spinfo =await _ishoplistservices.Query();
            }
           
             
      
            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = new ShopListViewModels()
                {
                    list = (from item in spinfo
                            orderby item.createTime descending
                            select new ShopListViewModelsList
                            {
                                id = item.id.ObjToInt(),
                                name = item.pName,
                                num = item.pNum.ObjToInt(),
                                own_num = _iusergoodslistservices.GetUserShopOwnNum(_user.ID, item.id).Result.Sum(x => x),
                                icon_url = item.pIcon,
                                price = Convert.ToInt32(item.price)
                            }).ToList()
                }
            };
        }


    }
}
