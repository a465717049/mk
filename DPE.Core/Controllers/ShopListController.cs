using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        readonly IDPEServices _idpeservices;
        readonly IDPEexchangeServices _idpeexchangeservices;
        readonly IShopBuyDetailSerivces _ishopbuydetailserivces;
        readonly IRPexchangeServices _irpexchangeservices;
        readonly IRPServices _irpservices;
        readonly IShoppingCartSerivces _ishoppingcartserivces;
        private readonly IUnitOfWork _unitOfWork;
        readonly IEPexchangeServices _iepexchangeservices;
        readonly IEPServices _iepservices;

        readonly IShopRoleServices _ishoproleservices;
        readonly IShopSkuServices _ishopskuservices;
        readonly IShopSkuDetailServices _ishopskudetailservices;


        readonly IOpenShopServices _iopenshopservices;
        public ShopListController(ISysUserInfoServices isysuserinfoservices, IUnitOfWork unitOfWork, IUser user,
            IShopListServices ishoplistservices, IUserGoodsListServices iusergoodslistservices,
            IUserInfoServices userInfoServices, IDPEServices idpeservices,
            IDPEexchangeServices idpeexchangeservices, IShopBuyDetailSerivces ishopbuydetailserivces,
            IRPexchangeServices irpexchangeservices, IRPServices irpservices,
            IShoppingCartSerivces ishoppingcartserivces, IOpenShopServices iopenshopservices,
            IEPexchangeServices iepexchangeservices,
            IEPServices iepservices, IShopRoleServices ishoproleservices, IShopSkuServices ishopskuservices, IShopSkuDetailServices ishopskudetailservices)
        {
            this._user = user;
            _userInfoServices = userInfoServices;
            _ishoplistservices = ishoplistservices;
            _iusergoodslistservices = iusergoodslistservices;
            _idpeservices = idpeservices;
            _idpeexchangeservices = idpeexchangeservices;
            _unitOfWork = unitOfWork;
            _isysuserinfoservices = isysuserinfoservices;
            _ishopbuydetailserivces = ishopbuydetailserivces;
            _irpexchangeservices = irpexchangeservices;
            _irpservices = irpservices;
            _ishoppingcartserivces = ishoppingcartserivces;
            _iopenshopservices = iopenshopservices;
            _iepexchangeservices = iepexchangeservices;
            _iepservices = iepservices;
            _ishoproleservices = ishoproleservices;
            _ishopskuservices = ishopskuservices;
            _ishopskudetailservices = ishopskudetailservices;
        }




        /// <summary>
        /// 商店列表 商品清单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShopList")]
        public async Task<MessageModel<ShopListViewModels>> GetShopList(int ptype=0,string language = "cn")
        {
            //_user.ID
            //     var user = await _userInfoServices.GetUserInfo(_user.ID);

            var role = await _ishoproleservices.Query(x => x.uId == _user.ID);
            if (role.Count <= 0)
            {
                role.Add(new ShopRole() { uId = _user.ID, shopRoleId = 1 });
            }
            if (role.Find(x => x.shopRoleId == 1) ==null)
            {
                role.Add(new ShopRole() { uId = _user.ID, shopRoleId = 1 });
            }
            var roleid = role.Select(x => x.shopRoleId);
            var spinfo = await _ishoplistservices.Query(x => x.ptype == ptype && roleid.Contains(x.Shopgroup.Value) && x.isDelete == false) ;
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
        /// 购买商品 商品购买
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
                //查询商品货量是否足够
                if (shop.pNum <= 0)
                {
                    tmpcode = 16001;
                }

                //查询是否有货品
                if (result.Count == 0)
                {
                    tmpcode = 16002;
                }

                //金币不足
                if (user.EP < shop.price)
                {
                    tmpcode = 16003;
                }

                if (tmpcode != 0)
                {
                    returnresult.code = tmpcode;
                    return returnresult;
                }

                //金币 EP
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
                    //商品适量减1
                    shop.pNum = shop.pNum - 1;
                    await _ishoplistservices.Update(shop);

                    //更新EP记录 
                    var myep = await _idpeservices.QueryById(_user.ID);
                    decimal tmpepamount = myep.amount.ObjToDecimal();
                    myep.amount = myep.amount - shop.price.Value;
                    await _idpeservices.Update(myep);

                    //插入ep记录
                    await _idpeexchangeservices.Add(new DPEexchange()
                    {
                        amount = shop.price,
                        createTime = DateTime.Now,
                        fromID = _user.ID,
                        uID = _user.ID,
                        lastTotal = tmpepamount,
                        remark = "购买商品",
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

                        if (shopdetail.ptype == 0) 
                        {
                           
                        }
                        var dpeinfo = (await _idpeservices.Query(x => x.uID == _user.ID)).First();
                        var epinfo = (await _iepservices.Query(x => x.uID == _user.ID)).First();

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

                            //特殊商品
                            if (shopdetail.ptype == 1)
                            {
                                if (epinfo.amount < Convert.ToDecimal(model.shoptotalnum * shopdetail.price))
                                {
                                    result.code = 1002;
                                    result.msg = "金额不够,无法购买商品";
                                    result.success = false;
                                    _unitOfWork.RollbackTran();
                                    return result;
                                }
                            }
                            else 
                            {
                                if (dpeinfo.amount < Convert.ToDecimal(model.shoptotalnum * shopdetail.price))
                                {
                                    result.code = 1002;
                                    result.msg = "产品分不够,无法购买商品";
                                    result.success = false;
                                    _unitOfWork.RollbackTran();
                                    return result;
                                }
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
                                    if (shopdetail.ptype == 1)
                                    {
                                        epinfo.amount = epinfo.amount - Convert.ToDecimal(model.shoptotalnum * shopdetail.price);
                                        if (_iepservices.Update(epinfo).Result)
                                        {
                                            if (_iepexchangeservices.Add(new EPexchange()
                                            {
                                                amount = -Convert.ToDecimal(model.shoptotalnum * shopdetail.price),
                                                createTime = DateTime.Now,
                                                uID = _user.ID,
                                                lastTotal = epinfo.amount + Convert.ToDecimal(model.shoptotalnum * shopdetail.price),
                                                stype = 88,
                                                remark = "购买商品",
                                                fromID = _user.ID
                                            }).Result > 0)
                                            {
                                            }
                                         
                                            if (_ishoppingcartserivces.DeleteById(model.id).Result)
                                            {
                                                successnum++;
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
                                    }
                                    else
                                    {
                                        dpeinfo.amount = dpeinfo.amount - Convert.ToDecimal(model.shoptotalnum * shopdetail.price);
                                        if (_idpeservices.Update(dpeinfo).Result)
                                        {
                                            if (_idpeexchangeservices.Add(new DPEexchange()
                                            {
                                                amount = -Convert.ToDecimal(model.shoptotalnum * shopdetail.price),
                                                createTime = DateTime.Now,
                                                uID = _user.ID,
                                                lastTotal = dpeinfo.amount + Convert.ToDecimal(model.shoptotalnum * shopdetail.price),
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

                                    }
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





        [HttpPost]
        [Route("BuyGoodsbyweb")]
        public async Task<MessageModel<dynamic>> BuyGoodsbyweb(string addr, string phone, string name, string remark)
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
                        //获取skudetail
                        var shopskudetialinfo = await _ishopskudetailservices.QueryById(model.shopid);

                        var shopskuinfo = await _ishopskuservices.QueryById(shopskudetialinfo.skuid);

                        var shopdetail = await _ishoplistservices.QueryById(shopskuinfo.shopid);


                        var dpeinfo = (await _idpeservices.Query(x => x.uID == _user.ID)).First();
                        var epinfo = (await _iepservices.Query(x => x.uID == _user.ID)).First();

                        if (shopskudetialinfo != null)
                        {
                            if ((shopskudetialinfo.detailnum - model.shoptotalnum) < 0)
                            {
                                result.code = 1002;
                                result.msg = shopskudetialinfo.detailname + "库存不足";
                                result.success = false;
                                _unitOfWork.RollbackTran();
                                return result;
                            }

                            //特殊商品
                            if (shopdetail.ptype == 1)
                            {
                                if (epinfo.amount < Convert.ToDecimal(model.shoptotalnum * shopskudetialinfo.detailprice))
                                {
                                    result.code = 1002;
                                    result.msg = "金额不够,无法购买商品";
                                    result.success = false;
                                    _unitOfWork.RollbackTran();
                                    return result;
                                }
                            }
                            else
                            {
                                if (dpeinfo.amount < Convert.ToDecimal(model.shoptotalnum * shopskudetialinfo.detailprice))
                                {
                                    result.code = 1002;
                                    result.msg = "产品分不够,无法购买商品";
                                    result.success = false;
                                    _unitOfWork.RollbackTran();
                                    return result;
                                }
                            }
                            ShopBuyDetail shopmodel = new ShopBuyDetail();
                            shopmodel.shopid = model.shopid;
                            shopmodel.buyNum = model.shoptotalnum;
                            shopmodel.buyuid = _user.ID;
                            shopmodel.createTime = DateTime.Now;
                            shopmodel.price = Convert.ToDecimal(model.shoptotalnum * shopskudetialinfo.detailprice);
                            shopmodel.status = 1;
                            shopmodel.buyaddr = addr;
                            shopmodel.buyname = name;
                            shopmodel.buyphone = phone;
                            shopmodel.reamrk = remark;
                            shopmodel.shopordernumber = creatOrderNumber();

                            var addresult = await _ishopbuydetailserivces.Add(shopmodel);
                            if (addresult > 0) 
                            {

                                shopskudetialinfo.detailnum -= model.shoptotalnum;
                                if (_ishopskudetailservices.Update(shopskudetialinfo).Result)
                                {
                                    if (shopdetail.ptype == 1)
                                    {
                                        epinfo.amount = epinfo.amount - Convert.ToDecimal(model.shoptotalnum * shopskudetialinfo.detailprice);
                                        if (_iepservices.Update(epinfo).Result)
                                        {
                                            if (_iepexchangeservices.Add(new EPexchange()
                                            {
                                                amount = -Convert.ToDecimal(model.shoptotalnum * shopskudetialinfo.detailprice),
                                                createTime = DateTime.Now,
                                                uID = _user.ID,
                                                lastTotal = epinfo.amount + Convert.ToDecimal(model.shoptotalnum * shopskudetialinfo.detailprice),
                                                stype = 88,
                                                remark = "购买商品",
                                                fromID = _user.ID
                                            }).Result > 0)
                                            {
                                            }

                                            if (_ishoppingcartserivces.DeleteById(model.id).Result)
                                            {
                                                successnum++;
                                            }
                                            else
                                            {
                                                result.code = 1002;
                                                result.msg = shopskudetialinfo.detailname + "结算异常请稍后再试";
                                                result.success = false;
                                                _unitOfWork.RollbackTran();
                                                return result;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dpeinfo.amount = dpeinfo.amount - Convert.ToDecimal(model.shoptotalnum * shopskudetialinfo.detailprice);
                                        if (_idpeservices.Update(dpeinfo).Result)
                                        {
                                            if (_idpeexchangeservices.Add(new DPEexchange()
                                            {
                                                amount = -Convert.ToDecimal(model.shoptotalnum * shopskudetialinfo.detailprice),
                                                createTime = DateTime.Now,
                                                uID = _user.ID,
                                                lastTotal = dpeinfo.amount + Convert.ToDecimal(model.shoptotalnum * shopskudetialinfo.detailprice),
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
                                                    result.msg = shopskudetialinfo.detailname + "结算异常请稍后再试";
                                                    result.success = false;
                                                    _unitOfWork.RollbackTran();
                                                    return result;
                                                }
                                            }
                                            else
                                            {
                                                result.code = 1002;
                                                result.msg = shopskudetialinfo.detailname + "结算异常请稍后再试";
                                                result.success = false;
                                                _unitOfWork.RollbackTran();
                                                return result;
                                            }
                                        }

                                    }
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
        /// 商品明细
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShopDeatilList")]
        public async Task<MessageModel<dynamic>> GetShopDeatilList(long shopid)
        {
            //_user.ID
            //     var user = await _userInfoServices.GetUserInfo(_user.ID);
            var spinfo = await _ishoplistservices.Query(x => x.id == shopid);
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

        [HttpPost]
        [Route("GetShopSkuList")]
        public async Task<MessageModel<dynamic>> GetShopSkuList(long shopid)
        {
            //_user.ID
            //     var user = await _userInfoServices.GetUserInfo(_user.ID);
            var spinfo = await _ishopskuservices.Query(x => x.shopid == shopid);
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

        [HttpPost]
        [Route("GetShopSkuDetailList")]
        public async Task<MessageModel<dynamic>> GetShopSkuDetailList(long id)
        {
            //_user.ID
            //     var user = await _userInfoServices.GetUserInfo(_user.ID);
            var spinfo = await _ishopskudetailservices.Query(x => x.skuid == id);
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
                spinfo = await _ishoplistservices.Query();
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
                    dataCount = data.dataCount,
                    page = data.page,
                    pageCount = data.pageCount,
                    data = (from item in data.data
                            select new
                            {
                                item,
                                shopname = _ishoplistservices.QueryById(item.shopid).Result.pName

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
        public async Task<MessageModel<dynamic>> AddGoodsweb(int shopid, int num, string option = "")
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var addshop = await _ishoppingcartserivces.Query(x => x.id == shopid && x.uid == _user.ID);
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
                    await _ishoppingcartserivces.Add(new ShoppingCart() { shopid = shopid, shoptotalnum = num, uid = _user.ID });
                   
                }
                result.code = 0;
                result.msg = "添加成功";
                result.success = true;
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


        //购物车修改
        [HttpPost]
        [Route("AddGoodsbyweb")]
        public async Task<MessageModel<dynamic>> AddGoodsbyweb(int shopid, int num, string option = "")
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var addshop = await _ishoppingcartserivces.Query(x => x.id == shopid && x.uid == _user.ID);
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
                    await _ishoppingcartserivces.Add(new ShoppingCart() { shopid = shopid, shoptotalnum = num, uid = _user.ID });

                }
                result.code = 0;
                result.msg = "添加成功";
                result.success = true;
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



        //获取我的订单
        [HttpPost]
        [Route("GetMyShopList")]
        public async Task<MessageModel<dynamic>> GetMyShopList(string ordernumber = "")
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var data = await _ishopbuydetailserivces.Query(x => x.buyuid == _user.ID);
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
                var data = await _ishopbuydetailserivces.Query(x => x.buyuid == _user.ID && x.id == id);
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
                                    shopnum = item.buyNum,
                                    shopprice = item.price,
                                    trackingnumber = item.trackingnumber,
                                    company = item.company,
                                    status = item.status,
                                    remark = item.reamrk,
                                    shopsku = _ishopskudetailservices.QueryById(item.shopid).Result,
                                    shopskudetail = _ishopskuservices.QueryById(_ishopskudetailservices.QueryById(item.shopid).Result.skuid).Result,
                                    shopdetail = _ishoplistservices.QueryById(_ishopskuservices.QueryById(_ishopskudetailservices.QueryById(item.shopid).Result.skuid).Result.shopid).Result
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


        [HttpPost]
        [Route("GetOpenShopMyweb")]
        public async Task<MessageModel<dynamic>> GetOpenShopMyweb()
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
                var data = await _iopenshopservices.QueryPage(x => 
              (x.nickname.ToString().Contains(key) || x.username.Contains(key) || x.userphone.Contains(key)), pageindex, pagesize, " createTime DESC ");

                result.response = new
                {
                    dataCount = data.dataCount,
                    page = data.page,
                    pageCount = data.pageCount,
                    data = data.data
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
        [Route("GetShopListMyweb")]
        public async Task<MessageModel<dynamic>> GetShopListMyweb()
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
                var data = await _ishoplistservices.QueryPage(x => x.isDelete==false &&
              (x.pName.ToString().Contains(key) || x.pDesc.Contains(key) || x.price.ToString().Contains(key)), pageindex, pagesize, " createTime DESC ");
                result.response = new
                {
                    dataCount = data.dataCount,
                    page = data.page,
                    pageCount = data.pageCount,
                    data = data.data
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
        [Route("GetShopSkuInfoMyweb")]
        public async Task<MessageModel<dynamic>> GetShopSkuInfoMyweb()
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
                long id =Convert.ToInt64(HttpContext.Request.Form["id"]);

                var data = await _ishopskuservices.Query(x => x.shopid == id);
                result.code = 0;
                result.response = new
                {
                    datainfo = (from item in data
                                select new
                                {
                                 skuinfo= item,
                                 skudetailinfo=_ishopskudetailservices.Query(z=>z.skuid==item.id).Result
                                }).ToList()
                };
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
        [Route("DeleteSkuInfoMyweb")]
        public async Task<MessageModel<dynamic>> DeleteSkuInfoMyweb()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            _unitOfWork.BeginTran();
            try
            {
                if (_user.ID == 0)
                {
                    result.code = 10001;
                    result.msg = "用户信息已过期，请重新登陆";
                    result.success = false;
                    return result;
                }
                long id = Convert.ToInt64(HttpContext.Request.Form["id"]);
                var detailinfo =await _ishopskudetailservices.Query(x => x.skuid == id);
                foreach (ShopSkuDetail skudetail in detailinfo) 
                {
                    if (! _ishopskudetailservices.Delete(skudetail).Result) 
                    {
                        _unitOfWork.RollbackTran();
                        result.code = 300;
                        result.msg = "删除明细失败!请稍后再试";
                        result.success = false;
                        return result;
                    }
                }

                if (!_ishopskuservices.DeleteById(id).Result) 
                {
                    _unitOfWork.RollbackTran();
                    result.code = 300;
                    result.msg = "删除商品sku失败!请稍后再试";
                    result.success = false;
                    return result;
                }
                _unitOfWork.CommitTran();

                result.code = 200;
                result.success = true;
                return result;
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTran();
                result.code = 500;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }
        }

        [HttpPost]
        [Route("DeleteSkudetailInfoMyweb")]
        public async Task<MessageModel<dynamic>> DeleteSkudetailInfoMyweb()
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
                long id = Convert.ToInt64(HttpContext.Request.Form["id"]);
                if (!_ishopskudetailservices.DeleteById(id).Result)
                {
                    result.code = 300;
                    result.msg = "删除明细失败!请稍后再试";
                    result.success = false;
                    return result;
                }

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
        [Route("DeleteShopListMyweb")]
        public async Task<MessageModel<dynamic>> DeleteShopListMyweb()
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
                long id = Convert.ToInt64(HttpContext.Request.Form["id"]);
                var data = await _ishoplistservices.Query(x =>x.id==id);
                if (data.Count > 0) 
                {
                    var model = data.First();
                    model.isDelete = true;

                    var buylist = await _ishopbuydetailserivces.Query(x => x.shopid == id);
                    if (buylist.Count > 0) 
                    {
                        foreach (ShopBuyDetail detail in buylist) 
                        {
                            await _ishopbuydetailserivces.DeleteById(detail.id);
                        }
                    }
                    await _ishoplistservices.Update(model);
                }
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
        [Route("AddShopListMyweb")]
        public async Task<MessageModel<dynamic>> AddShopListMyweb()
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

                ShopList shop = new ShopList();
                shop.id =string.IsNullOrEmpty( HttpContext.Request.Form["id"])?0:Convert.ToInt64(HttpContext.Request.Form["id"]);
                shop.isDelete = false;  //id minLevel price pNum pName pDesc pIcon status ptype priceType Shopgroup
                shop.minLevel=string.IsNullOrEmpty( HttpContext.Request.Form["idminLevel"])?0:Convert.ToInt32(HttpContext.Request.Form["minLevel"]);
                shop.price= string.IsNullOrEmpty(HttpContext.Request.Form["price"]) ? 0 : Convert.ToDecimal(HttpContext.Request.Form["price"]);
                shop.pNum = string.IsNullOrEmpty(HttpContext.Request.Form["pNum"]) ? 0 : Convert.ToInt32(HttpContext.Request.Form["pNum"]);
                shop.pName = string.IsNullOrEmpty(HttpContext.Request.Form["pName"]) ? "" : HttpContext.Request.Form["pName"].ToString();
                shop.pDesc = string.IsNullOrEmpty(HttpContext.Request.Form["pDesc"]) ? "" : HttpContext.Request.Form["pDesc"].ToString();
                shop.pIcon = string.IsNullOrEmpty(HttpContext.Request.Form["pIcon"]) ? "" : HttpContext.Request.Form["pIcon"].ToString();
                shop.createTime = DateTime.Now;
                shop.status = 1;// string.IsNullOrEmpty(HttpContext.Request.Form["status"]) ? 0 : Convert.ToInt32(HttpContext.Request.Form["status"]);
                shop.ptype = string.IsNullOrEmpty(HttpContext.Request.Form["ptype"]) ? 0 : Convert.ToInt32(HttpContext.Request.Form["ptype"]);
                shop.priceType = 1;// string.IsNullOrEmpty(HttpContext.Request.Form["priceType"]) ? 0 : Convert.ToInt32(HttpContext.Request.Form["priceType"]);
                shop.Shopgroup = string.IsNullOrEmpty(HttpContext.Request.Form["Shopgroup"]) ? 0 : Convert.ToInt32(HttpContext.Request.Form["Shopgroup"]);

                long shoopid = 0;
                if (shop.id == 0)
                {
                    shoopid = await _ishoplistservices.Add(shop);
                    //var model = await _ishoplistservices.QueryById(shoopid);
                    //model.pIcon = "shopimg_"+ shoopid + ".png";
                    //await _ishoplistservices.Update(model);
                }
                else 
                {
                    var upmodel = await _ishoplistservices.QueryById(shop.id);
                    shoopid = shop.id;
                    shop.pIcon = upmodel.pIcon;
                    shop.pDetailIcon = upmodel.pDetailIcon;
                    shop.isDelete = upmodel.isDelete;
                    shop.createTime = upmodel.createTime;
                    await _ishoplistservices.Update(shop);
                }
                result.code = 200;
                result.success = true;
                result.response = shoopid;
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
        [Route("AddSkuDetailMyweb")]
        public async Task<MessageModel<dynamic>> AddSkuDetailMyweb()
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

                ShopSkuDetail shopskudetail = new ShopSkuDetail();
                shopskudetail.id = string.IsNullOrEmpty(HttpContext.Request.Form["id"]) ? 0 : Convert.ToInt64(HttpContext.Request.Form["id"]);
                shopskudetail.skuid = string.IsNullOrEmpty(HttpContext.Request.Form["skuid"]) ? 0 : Convert.ToInt64(HttpContext.Request.Form["skuid"].ToString());
                shopskudetail.detaildesc = string.IsNullOrEmpty(HttpContext.Request.Form["detaildesc"]) ? "" : HttpContext.Request.Form["detaildesc"].ToString(); 
                shopskudetail.detailicon = string.IsNullOrEmpty(HttpContext.Request.Form["detailicon"]) ? "" : HttpContext.Request.Form["detailicon"].ToString();
                shopskudetail.detailname = string.IsNullOrEmpty(HttpContext.Request.Form["detailname"]) ? "" : HttpContext.Request.Form["detailname"].ToString();
                shopskudetail.detailnum = string.IsNullOrEmpty(HttpContext.Request.Form["detailnum"]) ? 0 : Convert.ToInt32(HttpContext.Request.Form["detailnum"]);
                shopskudetail.detailprice = string.IsNullOrEmpty(HttpContext.Request.Form["detailprice"]) ? 0:Convert.ToDecimal(HttpContext.Request.Form["detailprice"]);
 
                long addid = 0;
                if (shopskudetail.id == 0)
                {
                    shopskudetail.createtime = DateTime.Now;
                    addid = await _ishopskudetailservices.Add(shopskudetail);
                }
                else
                {
                    var upmodel = await _ishopskudetailservices.QueryById(shopskudetail.id);
                    addid = shopskudetail.id;
                    shopskudetail.detailicon = upmodel.detailicon;
                    await _ishoplistservices.Update(shopskudetail);
                }
                result.code = 200;
                result.success = true;
                result.response = addid;
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
        [Route("AddSkuMyweb")]
        public async Task<MessageModel<dynamic>> AddSkuMyweb()
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

                ShopSku shopsku = new ShopSku();
                shopsku.id = string.IsNullOrEmpty(HttpContext.Request.Form["id"]) ? 0 : Convert.ToInt64(HttpContext.Request.Form["id"]);
                shopsku.shopid = string.IsNullOrEmpty(HttpContext.Request.Form["shopid"]) ? 0:Convert.ToInt64(HttpContext.Request.Form["shopid"].ToString());
                shopsku.skudesc = string.IsNullOrEmpty(HttpContext.Request.Form["skudesc"]) ? "" : HttpContext.Request.Form["skudesc"].ToString();
                shopsku.skuIcon = string.IsNullOrEmpty(HttpContext.Request.Form["skuIcon"]) ? "" : HttpContext.Request.Form["skuIcon"].ToString();
                shopsku.skuname = string.IsNullOrEmpty(HttpContext.Request.Form["skuname"]) ? "" : HttpContext.Request.Form["skuname"].ToString();

                long addid = 0;
                if (shopsku.id == 0)
                {
                    shopsku.createtime = DateTime.Now;
                    addid = await _ishopskuservices.Add(shopsku);
                }
                else
                {
                    addid = shopsku.id;
                    await _ishopskuservices.Update(shopsku);
                }
                result.code = 200;
                result.success = true;
                result.response = addid;
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
        [Route("uploadPicture")]
        public async Task<MessageModel<dynamic>> uploadPicture()
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
                var ss = Directory.GetCurrentDirectory();
                var files = HttpContext.Request.Form.Files;
                int id =Convert.ToInt32(HttpContext.Request.Form["id"]);
                if (files.Count>0)
                {
                    using (HttpClient client = new HttpClient())
                    {

                        var model = await _ishoplistservices.QueryById(id);
                        model.pIcon = "shopimg_" + id + ".png";
                       var resultz =  _ishoplistservices.Update(model);

                        if (resultz.Result) 
                        {
                            var text = HttpContext.Request.Form.Files[0].OpenReadStream();
                            string strPath = "";
                            strPath = ss + @"//shopimg//shopimg_" + id + ".png";
                            StreamHelp.StreamToFile(text, strPath);
                        }
                  

                      
                    }
                    //return "添加成功";
                }
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
        [Route("uploadPictureDetail")]
        public async Task<MessageModel<dynamic>> uploadPictureDetail()
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
                var ss = Directory.GetCurrentDirectory();
                var files = HttpContext.Request.Form.Files;
                int id = Convert.ToInt32(HttpContext.Request.Form["id"]);
                if (files.Count > 0)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var model = await _ishoplistservices.QueryById(id);
                        model.pDetailIcon = "shopdetailimg_" + id + ".png";
                       var resultz=  _ishoplistservices.Update(model);
                        if (resultz.Result)
                        {
                            var text = HttpContext.Request.Form.Files[0].OpenReadStream();
                            string strPath = "";
                            strPath = ss + @"//shopimg//shopdetailimg_" + id + ".png";
                            StreamHelp.StreamToFile(text, strPath);

                        }

                    }
                    //return "添加成功";
                }
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
        [Route("uploadPictureSkuDetail")]
        public async Task<MessageModel<dynamic>> uploadPictureSkuDetail()
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
                var ss = Directory.GetCurrentDirectory();
                var files = HttpContext.Request.Form.Files;
                int id = Convert.ToInt32(HttpContext.Request.Form["id"]);
                if (files.Count > 0)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var model = await _ishopskudetailservices.QueryById(id);
                        model.detailicon = "skudetailimg_" + id + ".png";
                        var resultz = _ishopskudetailservices.Update(model);
                        if (resultz.Result)
                        {
                            var text = HttpContext.Request.Form.Files[0].OpenReadStream();
                            string strPath = "";
                            strPath = ss + @"//shopimg//skudetailimg_" + id + ".png";
                            StreamHelp.StreamToFile(text, strPath);

                        }

                    }
                    //return "添加成功";
                }
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
        [Route("ApplyOpenShopMyweb")]
        public async Task<MessageModel<dynamic>> ApplyOpenShopMyweb()
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
                long openid = Convert.ToInt64( HttpContext.Request.Form["openid"]);
                var data = await _iopenshopservices.Query(x => x.openid == openid && x.openstatus==0);
                if (data.Count > 0)
                {
                    var model = data.First();
                    model.openstatus = 1;
                    await _iopenshopservices.Update(model);
                    result.code = 200;
                    result.success = true;
                    return result;
                }
                else 
                {
                    result.code = 500;
                    result.success = false;
                    return result;
                }
               
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }


        }


        //获取购物车
        [HttpPost]
        [Route("GetShopCartsweb")]
        public async Task<MessageModel<dynamic>> GetShopCartsweb(int ptype=0)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var data = await _ishoppingcartserivces.Query(x => x.uid == _user.ID );
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
                                    id = item.id,
                                    shopid = item.shopid,
                                    shopnum = item.shoptotalnum,
                                    shopdetail = _ishoplistservices.QueryById(item.shopid).Result
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


        //获取购物车
        [HttpPost]
        [Route("GetShopCartsbyweb")]
        public async Task<MessageModel<dynamic>> GetShopCartsbyweb(int ptype = 0)
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
                                    id = item.id,
                                    shopid = item.shopid,
                                    shopnum = item.shoptotalnum,
                                    shopsku=_ishopskudetailservices.QueryById(item.shopid).Result,
                                    shopskudetail =_ishopskuservices.QueryById(_ishopskudetailservices.QueryById(item.shopid).Result.skuid).Result,
                                    shopdetail = _ishoplistservices.QueryById(_ishopskuservices.QueryById(_ishopskudetailservices.QueryById(item.shopid).Result.skuid).Result.shopid).Result
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

        //申请开店
        [HttpPost]
        [Route("ApplyOpenShop")]
        public async Task<MessageModel<dynamic>> ApplyOpenShop(long uid,string nickname,string username,string userphone)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var data = await _iopenshopservices.Query(x => x.openid == uid);
                if (data.Count<=0)
                {
                    if (_iopenshopservices.Add(new OpenShop
                    {
                        openstatus = 0,
                        applyid = _user.ID,
                        createtime = DateTime.Now,
                        nickname = nickname,
                        username = username,
                        userphone = userphone,
                        openid=uid
                    }).Result > 0)
                    {
                        result.code = 0;
                        result.msg = "恭喜您提交成功!请等待专员与您联系!";
                        result.success = true;
                    }
                    else
                    {
                        result.code = 10001;
                        result.msg = "请稍后再试";
                        result.success = false;
                    }
                }
                else
                {
                    result.code = 10001;
                    result.msg = "您已经提交过申请!请不要重复提交!";
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
    }
}
