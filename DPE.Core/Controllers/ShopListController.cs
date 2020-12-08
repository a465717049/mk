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
        readonly IShopBuyDetailSerivces _ishopbuydetailserivces;
        readonly IRPexchangeServices _irpexchangeservices;
        readonly IRPServices _irpservices;
        readonly IShoppingCartSerivces _ishoppingcartserivces;
        private readonly IUnitOfWork _unitOfWork;


        public ShopListController(ISysUserInfoServices isysuserinfoservices, IUnitOfWork unitOfWork, IUser user,
            IShopListServices ishoplistservices, IUserGoodsListServices iusergoodslistservices,
            IUserInfoServices userInfoServices, IEPServices iepservices,
            IEPexchangeServices iepexchangeservices, IShopBuyDetailSerivces ishopbuydetailserivces,
            IRPexchangeServices irpexchangeservices, IRPServices irpservices, IShoppingCartSerivces ishoppingcartserivces)
        {
            this._user = user;
            _userInfoServices = userInfoServices;
            _ishoplistservices = ishoplistservices;
            _iusergoodslistservices = iusergoodslistservices;
            _iepservices = iepservices;
            _iepexchangeservices = iepexchangeservices;
            _unitOfWork = unitOfWork;
            _isysuserinfoservices = isysuserinfoservices;
            _ishopbuydetailserivces = ishopbuydetailserivces;
            _irpexchangeservices = irpexchangeservices;
            _irpservices = irpservices;
            _ishoppingcartserivces = ishoppingcartserivces;
        }



        /// <summary>
        /// 商店列表 商品清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShopList")]
        public async Task<MessageModel<ShopListViewModels>> GetShopList(string language = "cn")
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



        /// <summary>
        /// 仓里商品订单列表记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserGoodsList")]
        public async Task<MessageModel<dynamic>> GetUserGoodsList(string language = "cn")
        {
            //_user.ID

            var spinfo = await _iusergoodslistservices.GetUserGoodsList(_user.ID);

            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = new
                {
                    num = spinfo.Count,
                    list = (from item in spinfo
                            orderby item.id descending
                            select new ExchangeViewList
                            {
                                msg = string.Format("购买:{0}", (_ishoplistservices.Query(x => x.id == item.shopID)).Result.First().pName),
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
                        stype = 99
                    });

                }
                else
                {
                    _unitOfWork.RollbackTran();
                    tmpcode = 1004;
                    return returnresult;
                }


                returnresult = new MessageModel<dynamic>()
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
        public async Task<MessageModel<dynamic>> BuyGoodsweb(string addr, string phone, string name, string remark)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            int successnum = 0;
            try
            {
                //结算购物车
                if (_user.ID > 0)
                {
                    _unitOfWork.BeginTran();
                    var mycart = await _ishoppingcartserivces.Query(x => x.uid == _user.ID);
                    List<ShopBuyDetail> addlist = new List<ShopBuyDetail>();
                    foreach (ShoppingCart model in mycart)
                    {
                        var shopdetail = await _ishoplistservices.QueryById(model.shopid);
                        var rpinfo = (await _irpservices.Query(x => x.uID == _user.ID)).First();
                        if (shopdetail != null)
                        {
                            if ((shopdetail.pNum - model.shoptotalnum) < 0)
                            {
                                result.code = 1002;
                                result.msg = shopdetail.pName + "库存不足";
                                result.success = false;
                                _unitOfWork.RollbackTran();
                                return result;
                            }

                            if (rpinfo.amount < Convert.ToDecimal(model.shoptotalnum * shopdetail.price))
                            {
                                result.code = 1002;
                                result.msg = "金额不不够";
                                result.success = false;
                                _unitOfWork.RollbackTran();
                                return result;
                            }
                            ShopBuyDetail shopmodel = new ShopBuyDetail();
                            shopmodel.shopid = model.shopid;
                            shopmodel.buyNum = model.shoptotalnum;
                            shopmodel.buyuid = _user.ID;
                            shopmodel.createTime = DateTime.Now;
                            shopmodel.price = Convert.ToDecimal(model.shoptotalnum * _ishoplistservices.QueryById(model.shopid).Result.price);
                            shopmodel.status = 1;
                            shopmodel.buyaddr = addr;
                            shopmodel.buyname = name;
                            shopmodel.buyphone = phone;
                            shopmodel.reamrk = remark;
                            shopmodel.shopordernumber = creatOrderNumber();

                            var addresult = await _ishopbuydetailserivces.Add(shopmodel);
                            if (addresult > 0)
                            {

                                shopdetail.pNum -= model.shoptotalnum;
                                if (_ishoplistservices.Update(shopdetail).Result)
                                {
                                    rpinfo.amount = rpinfo.amount - Convert.ToDecimal(model.shoptotalnum * shopdetail.price);
                                    if (_irpservices.Update(rpinfo).Result)
                                    {
                                        if (_irpexchangeservices.Add(new RPexchange()
                                        {
                                            amount = -Convert.ToDecimal(model.shoptotalnum * shopdetail.price),
                                            createTime = DateTime.Now,
                                            uID = _user.ID,
                                            lastTotal = rpinfo.amount + Convert.ToDecimal(model.shoptotalnum * shopdetail.price),
                                            stype = 88,
                                            remark = "购买商品",
                                            fromID = _user.ID
                                        }).Result > 0)
                                        {
                                            if (_ishoppingcartserivces.DeleteById(model.id).Result)
                                            {
                                                successnum++;
                                            }
                                            else
                                            {
                                                result.code = 1002;
                                                result.msg = shopdetail.pName + "结算异常请稍后再试";
                                                result.success = false;
                                                _unitOfWork.RollbackTran();
                                                return result;
                                            }
                                        }
                                        else
                                        {
                                            result.code = 1002;
                                            result.msg = shopdetail.pName + "结算异常请稍后再试";
                                            result.success = false;
                                            _unitOfWork.RollbackTran();
                                            return result;
                                        }
                                    }
                                    else
                                    {
                                        result.code = 1002;
                                        result.msg = shopdetail.pName + "结算异常请稍后再试";
                                        result.success = false;
                                        _unitOfWork.RollbackTran();
                                        return result;
                                    };
                                }
                                else
                                {
                                    result.code = 1002;
                                    result.msg = shopdetail.pName + "更新商品数量有误请稍后再试";
                                    result.success = false;
                                    _unitOfWork.RollbackTran();
                                    return result;
                                }

                            }
                            else
                            {
                                result.code = 1002;
                                result.msg = shopdetail.pName + "结算异常请稍后再试";
                                result.success = false;
                                _unitOfWork.RollbackTran();
                                return result;
                            }

                        }
                    }
                }
                result.code = 0;
                result.msg = "成功结算:" + successnum.ToString() + "件商品";
                result.success = false;
                _unitOfWork.CommitTran();
                return result;

            }
            catch
            {
                result.code = 10001;
                result.msg = "请稍后再试";
                result.success = false;
                _unitOfWork.RollbackTran();
                return result;
            }

        }

        public string creatOrderNumber() 
        {

            Random ran = new Random();
            string tmpordernumber = string.Format("MK" + DateTime.Now.ToString("yyyyMMdd") + ran.Next(1000, 10000).ToString());
            if (_ishopbuydetailserivces.Query(x => x.shopordernumber.Equals(tmpordernumber)).Result.Count > 0)  
            {
                creatOrderNumber();
            }
            return tmpordernumber;
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


        [HttpPost]
        [Route("GetAdminBuyShopList")]
        public async Task<MessageModel<dynamic>> GetAdminBuyShopList()
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
                var data = await _ishopbuydetailserivces.QueryPage(x => x.status.ToString().Contains(stype) &&
              (x.buyuid.ToString().Contains(key) || x.buyaddr.Contains(key)), pageindex, pagesize, " createTime DESC ");

                result.response = new
                {
                     dataCount=data.dataCount,
                     page=data.page,
                     pageCount=data.pageCount,
                     data= (from item in data.data
                           select new
                           {
                             item,
                             shopname=_ishoplistservices.QueryById(item.shopid).Result.pName

                           })
            };
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
        [Route("AddTruckOrdersweb")]
        public async Task<MessageModel<dynamic>> AddTruckOrdersweb()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                int detailid = Convert.ToInt32(HttpContext.Request.Form["detailid"]);
                string order = HttpContext.Request.Form["order"];

                var shopdetail = await _ishopbuydetailserivces.QueryById(detailid);
                if (shopdetail != null)
                {
                    shopdetail.trackingnumber = order;
                    await _ishopbuydetailserivces.Update(shopdetail);
                    result.code = 0;
                    result.msg = "操作成功";
                    result.success = true;

                }
                else
                {
                    result.code = 10001;
                    result.msg = "商品信息有误";
                    result.success = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                result.code = 10001;
                result.msg = "请稍后再试";
                result.success = false;
                return result;
            }
        }

        [HttpPost]
        [Route("ChangeOrdersweb")]
        public async Task<MessageModel<dynamic>> ChangeOrdersweb()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                int detailid = Convert.ToInt32(HttpContext.Request.Form["detailid"]);
                int status = Convert.ToInt32(HttpContext.Request.Form["status"]);
                var shopdetail = await _ishopbuydetailserivces.QueryById(detailid);
                if (shopdetail != null)
                {
                    shopdetail.status = status;
                    await _ishopbuydetailserivces.Update(shopdetail);
                    result.code = 0;
                    result.msg = "操作成功";
                    result.success = true;

                }
                else
                {
                    result.code = 10001;
                    result.msg = "商品信息有误";
                    result.success = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                result.code = 10001;
                result.msg = "请稍后再试";
                result.success = false;
                return result;
            }
        }

        //添加购物车
        [HttpPost]
        [Route("AddGoodsweb")]
        public async Task<MessageModel<dynamic>> AddGoodsweb(int shopid,int num,string option="")
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var shopdetail = await _ishoplistservices.QueryById(shopid);
                if (shopdetail != null)
                {
                    var addshop = await _ishoppingcartserivces.Query(x => x.shopid == shopid && x.uid == _user.ID);
                    if (addshop.Count() > 0)
                    {
                        var model = addshop.First();
                        if (string.IsNullOrEmpty(option))
                        {
                            model.shoptotalnum += num;
                        }
                        else
                        {
                            model.shoptotalnum -= num;
                        }

                        if (model.shoptotalnum <= 0)
                        {
                            await _ishoppingcartserivces.Delete(model);
                        }
                        else 
                        {
                            await _ishoppingcartserivces.Update(model);
                        }
                    }
                    else 
                    {
                        await _ishoppingcartserivces.Add(new ShoppingCart() { shopid = shopid, shoptotalnum = num, uid=_user.ID }); ;
                    }
                    result.code = 0;
                    result.msg = "添加成功";
                    result.success = true;

                }
                else 
                {
                    result.code = 10001;
                    result.msg = "商品信息有误";
                    result.success = false;
                }
                  
                return result;
            }
            catch(Exception ex)
            {
                string a = ex.Message;
                result.code = 10001;
                result.msg = "请稍后再试";
                result.success = false;
                return result;
            }

        }


        //获取我的订单
        [HttpPost]
        [Route("GetMyShopList")]
        public async Task<MessageModel<dynamic>> GetMyShopList(string ordernumber="")
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var data = await _ishopbuydetailserivces.Query(x => x.buyuid == _user.ID );
                result.code = 0;
                result.response = new
                {
                    list = data
                };
                return result;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                result.code = 10001;
                result.msg = "请稍后再试";
                result.success = false;
                return result;
            }

        }

        [HttpPost]
        [Route("GetShopDetailsMyweb")]
        public async Task<MessageModel<dynamic>> GetShopDetailsMyweb(long id)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var data = await _ishopbuydetailserivces.Query(x => x.buyuid == _user.ID && x.id==id);
                result.code = 0;
                result.response = new
                {
                    datainfo = (from item in data
                                select new
                                {
                                    shopordernumber = item.shopordernumber,
                                    shopidid = item.shopid,
                                    id = item.shopid,
                                    buyaddr = item.buyaddr,
                                    phonename = item.buyname + "  " + item.buyphone,
                                    shopname = _ishoplistservices.QueryById(item.shopid).Result.pName,
                                    shopnum = item.buyNum,
                                    shopprice = item.price,
                                    trackingnumber = item.trackingnumber,
                                    company = item.company,
                                    status = item.status,
                                    remark = item.reamrk
                                }).ToList()
                };
                return result;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                result.code = 10001;
                result.msg = "请稍后再试";
                result.success = false;
                return result;
            }

        }




        //获取购物车
        [HttpPost]
        [Route("GetShopCartsweb")]
        public async Task<MessageModel<dynamic>> GetShopCartsweb()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var data = await _ishoppingcartserivces.Query(x => x.uid == _user.ID);
                result.code = 0;
                result.response = new
                {
                    count = data.Count() > 0 ? data.Sum(x => x.shoptotalnum) : 0,
                    data = new 
                    {
                        list = (from item in data
                                orderby item.id descending
                                select new 
                                {
                                   uid = item.uid,
                                   id=item.id,
                                   shopid =item.shopid,
                                   shopnum=item.shoptotalnum,
                                   shopdetail= _ishoplistservices.QueryById(item.shopid).Result
                                }).ToList()
                    }
                };
                return result;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                result.code = 10001;
                result.msg = "请稍后再试";
                result.success = false;
                return result;
            }

        }

        //获取最后一次使用的地址
        [HttpPost]
        [Route("GetShopaddr")]
        public async Task<MessageModel<dynamic>> GetShopaddr()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var data = await _ishopbuydetailserivces.Query(x => x.buyuid == _user.ID, " createTime desc ");
                result.code = 0;
                var descdata = data.First();
                result.response = new
                {
                    data = descdata
                };
                return result;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                result.code = 10001;
                result.msg = "请稍后再试";
                result.success = false;
                return result;
            }

        }
    }
}
