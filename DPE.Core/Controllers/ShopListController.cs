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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;

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

        private IHostingEnvironment _hostingEnvironment;

        readonly IDownExcelRecordServices _idownexcelrecordservices;

        readonly IOpenShopServices _iopenshopservices;
        public ShopListController(ISysUserInfoServices isysuserinfoservices, IUnitOfWork unitOfWork, IUser user,
            IShopListServices ishoplistservices, IUserGoodsListServices iusergoodslistservices,
            IUserInfoServices userInfoServices, IDPEServices idpeservices,
            IDPEexchangeServices idpeexchangeservices, IShopBuyDetailSerivces ishopbuydetailserivces,
            IRPexchangeServices irpexchangeservices, IRPServices irpservices,
            IShoppingCartSerivces ishoppingcartserivces, IOpenShopServices iopenshopservices,
            IEPexchangeServices iepexchangeservices,
            IEPServices iepservices, IShopRoleServices ishoproleservices, 
            IShopSkuServices ishopskuservices, IShopSkuDetailServices ishopskudetailservices , IHostingEnvironment hostingEnvironment, IDownExcelRecordServices idownexcelrecordservices)
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
            _hostingEnvironment = hostingEnvironment;
            _idownexcelrecordservices = idownexcelrecordservices;
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
                        var rpinfo = (await _irpservices.Query(x => x.uID == _user.ID)).First();

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

                            decimal totalshopprice = Convert.ToDecimal(model.shoptotalnum * shopskudetialinfo.detailprice);
                            //特殊商品
                            if (shopdetail.ptype == 1)
                            {
                                if (epinfo.amount < totalshopprice && rpinfo.amount < totalshopprice)
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
                                if (dpeinfo.amount < totalshopprice)
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
                            shopmodel.price = totalshopprice;
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
                                        if (rpinfo.amount >= totalshopprice)
                                        {
                                            rpinfo.amount = rpinfo.amount - totalshopprice;
                                            if (_irpservices.Update(rpinfo).Result)
                                            {
                                                if (_irpexchangeservices.Add(new RPexchange()
                                                {
                                                    amount = -totalshopprice,
                                                    createTime = DateTime.Now,
                                                    uID = _user.ID,
                                                    lastTotal = rpinfo.amount + totalshopprice,
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
                                            epinfo.amount = epinfo.amount - totalshopprice;
                                            if (_iepservices.Update(epinfo).Result)
                                            {
                                                if (_iepexchangeservices.Add(new EPexchange()
                                                {
                                                    amount = -totalshopprice,
                                                    createTime = DateTime.Now,
                                                    uID = _user.ID,
                                                    lastTotal = epinfo.amount + totalshopprice,
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

                                     
                                    }
                                    else
                                    {
                                        dpeinfo.amount = dpeinfo.amount - totalshopprice;
                                        if (_idpeservices.Update(dpeinfo).Result)
                                        {
                                            if (_idpeexchangeservices.Add(new DPEexchange()
                                            {
                                                amount = -totalshopprice,
                                                createTime = DateTime.Now,
                                                uID = _user.ID,
                                                lastTotal = dpeinfo.amount + totalshopprice,
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
                string startdt = HttpContext.Request.Form["startdt"];
                string enddt = HttpContext.Request.Form["enddt"];
                if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                {
                    key = "";
                }

                if (string.IsNullOrEmpty(stype) || string.IsNullOrWhiteSpace(stype))
                {
                    stype = "";
                }

                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();


                if (string.IsNullOrEmpty(startdt) || string.IsNullOrWhiteSpace(startdt))
                {
                    dt1 = Convert.ToDateTime("1999-01-01");
                }
                else 
                {
                    dt1 = Convert.ToDateTime(startdt);
                }


                if (string.IsNullOrEmpty(enddt) || string.IsNullOrWhiteSpace(enddt))
                {
                    dt2 = DateTime.Now;
                }
                else 
                {
                    dt2 = Convert.ToDateTime(enddt);
                }


                pagesize = pagesize == 0 ? 20 : pagesize;
                pageindex = pageindex == 0 ? 1 : pageindex;
                var data = await _ishopbuydetailserivces.QueryPage(x => x.status.ToString().Contains(stype) &&
              (x.buyuid.ToString().Contains(key) || x.buyaddr.Contains(key)) && (x.createTime>=dt1 && x.createTime<=dt2) , pageindex, pagesize, " createTime DESC ");

                result.response = new
                {
                    dataCount = data.dataCount,
                    page = data.page,
                    pageCount = data.pageCount,
                    data = (from item in data.data
                            select new
                            {
                                item,
                                shopskudt = _ishopskudetailservices.QueryById(item.shopid).Result,
                                shopsku = _ishopskuservices.QueryById(_ishopskudetailservices.QueryById(item.shopid).Result.skuid).Result,
                                shopinfo = _ishoplistservices.QueryById(_ishopskuservices.QueryById(_ishopskudetailservices.QueryById(item.shopid).Result.skuid).Result.shopid).Result
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

                var data = await _ishopskuservices.Query(x => x.shopid == id && !x.isdelete);
                result.code = 0;
                result.response = new
                {
                    datainfo = (from item in data
                                select new
                                {
                                 skuinfo= item, 
                                 skudetailinfo=_ishopskudetailservices.Query(z=>z.skuid==item.id && !z.isdelete).Result
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
                    skudetail.isdelete = true;
                    if (! _ishopskudetailservices.Update(skudetail).Result) 
                    {
                        _unitOfWork.RollbackTran();
                        result.code = 300;
                        result.msg = "删除明细失败!请稍后再试";
                        result.success = false;
                        return result;
                    }
                }

                var skuinfo = await _ishopskuservices.QueryById(id);
                skuinfo.isdelete = true;
                if (!_ishopskuservices.Update(skuinfo).Result) 
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
                var skudt = await _ishopskudetailservices.QueryById(id);
                skudt.isdelete = true;
                if (!_ishopskudetailservices.Update(skudt).Result)
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
                    var upmodel = await _ishopskuservices.QueryById(shopsku.id);
                    upmodel.skuname = shopsku.skuname;
                    upmodel.skudesc = shopsku.skudesc;
                    addid = shopsku.id;
                    await _ishopskuservices.Update(upmodel);
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
        [Route("AddSkuAndDetail")]
        public async Task<MessageModel<dynamic>> AddSkuAndDetail()
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

                List<ShopSku> skulist = new List<ShopSku>();
                List<ShopSkuDetail> skudtlist = new List<ShopSkuDetail>();
                string strskulist = string.IsNullOrEmpty(HttpContext.Request.Form["strskulist"]) ? "" : HttpContext.Request.Form["strskulist"].ToString();
                string strskudtlist = string.IsNullOrEmpty(HttpContext.Request.Form["strskudtlist"]) ? "" : HttpContext.Request.Form["strskudtlist"].ToString();

                List<int> resultnum = new List<int>();
                List<int> resultdtnum = new List<int>();
                if (!string.IsNullOrEmpty(strskulist))
                {
                    skulist = JsonConvert.DeserializeObject<List<ShopSku>>(strskulist);
                    if (!string.IsNullOrEmpty(strskudtlist))
                    {
                        skudtlist = JsonConvert.DeserializeObject<List<ShopSkuDetail>>(strskudtlist);
                    }
                    foreach (ShopSku model in skulist)
                    {
                        model.createtime = DateTime.Now;
                        int snum = await _ishopskuservices.Add(model);
                        if (snum > 0)
                        {
                            resultnum.Add(snum);
                            foreach (ShopSkuDetail modeldt in skudtlist)
                            {
                                modeldt.createtime = DateTime.Now;
                                modeldt.skuid = snum;
                                int numdt= await _ishopskudetailservices.Add(modeldt);
                                if (numdt > 0) 
                                {
                                    resultdtnum.Add(numdt);
                                }
                            }
                        }
                    }

                }
                result.code = 200;
                result.success = true;
                result.response =new
                { //resultnum resultdtnum
                    resultnum,
                    resultdtnum
                };
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
                var ids = HttpContext.Request.Form["id"].ToString().Split(',');
                if (files.Count > 0)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        foreach (string modelid in ids)
                        {
                            int id = Convert.ToInt32(modelid);

                            var model = await _ishoplistservices.QueryById(id);
                            model.pIcon = "shopimg_" + id + ".png";
                            var resultz = _ishoplistservices.Update(model);

                            if (resultz.Result)
                            {
                                var text = HttpContext.Request.Form.Files[0].OpenReadStream();
                                string strPath = "";
                                strPath = ss + @"//shopimg//shopimg_" + id + ".png";
                                StreamHelp.StreamToFile(text, strPath);
                            }
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
                var ids = HttpContext.Request.Form["id"].ToString().Split(',');
                if (files.Count > 0)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        foreach (string modelid in ids)
                        {
                            int id = Convert.ToInt32(modelid);
                            var model = await _ishoplistservices.QueryById(id);
                            model.pDetailIcon = "shopdetailimg_" + id + ".png";
                            var resultz = _ishoplistservices.Update(model);
                            if (resultz.Result)
                            {
                                var text = HttpContext.Request.Form.Files[0].OpenReadStream();
                                string strPath = "";
                                strPath = ss + @"//shopimg//shopdetailimg_" + id + ".png";
                                StreamHelp.StreamToFile(text, strPath);

                            }
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
                var ids = HttpContext.Request.Form["id"].ToString().Split(',');
                if (files.Count > 0)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        foreach (string modelid in ids) 
                        {
                            int id = Convert.ToInt32(modelid);
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
        [Route("uploadPictureSku")]
        public async Task<MessageModel<dynamic>> uploadPictureSku()
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

                var ids = HttpContext.Request.Form["id"].ToString().Split(',');
                if (files.Count > 0)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        foreach (string modelid in ids) 
                        {
                            int id = Convert.ToInt32(modelid);
                            var model = await _ishopskuservices.QueryById(id);
                            model.skuIcon = "skuimg_" + id + ".png";
                            var resultz = _ishopskuservices.Update(model);
                            if (resultz.Result)
                            {
                                var text = HttpContext.Request.Form.Files[0].OpenReadStream();
                                string strPath = "";
                                strPath = ss + @"//shopimg//skuimg_" + id + ".png";
                                StreamHelp.StreamToFile(text, strPath);
                            }

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


        #region 导出
        [HttpPost]
        [Route("OrderOutAllPut")]
        public  IActionResult OrderOutAllPut()
        {
            try
            {
 
                var sWebRootFolder = Directory.GetCurrentDirectory() + @"//shopimg//downinfo//";
                var nowdate =DateTime.Now;
                Random rad = new Random();
                int value = rad.Next(1000, 10000);
                string sFileName = string.Format("{0}{1}{2}{3}_{4}.xlsx","订单列表详细",nowdate.Year.ToString(), nowdate.Month.ToString(), nowdate.Day.ToString(),value.ToString());
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                int pageindex = 1;
                int pagesize = 999999;
                string key = HttpContext.Request.Form["key"];
                string stype = HttpContext.Request.Form["stype"];
                string startdt = HttpContext.Request.Form["startdt"];
                string enddt = HttpContext.Request.Form["enddt"];
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                    {
                        key = "";
                    }

                    if (string.IsNullOrEmpty(stype) || string.IsNullOrWhiteSpace(stype))
                    {
                        stype = "";
                    }

                    DateTime dt1 = new DateTime();
                    DateTime dt2 = new DateTime();


                    if (string.IsNullOrEmpty(startdt) || string.IsNullOrWhiteSpace(startdt))
                    {
                        dt1 = Convert.ToDateTime("1999-01-01");
                    }
                    else
                    {
                        dt1 = Convert.ToDateTime(startdt);
                    }


                    if (string.IsNullOrEmpty(enddt) || string.IsNullOrWhiteSpace(enddt))
                    {
                        dt2 = DateTime.Now;
                    }
                    else
                    {
                        dt2 = Convert.ToDateTime(enddt);
                    }
                    var data = _ishopbuydetailserivces.QueryPage(x => x.status.ToString().Contains(stype) &&
                  (x.buyuid.ToString().Contains(key) || x.buyaddr.Contains(key)) && (x.createTime >= dt1 && x.createTime <= dt2), pageindex, pagesize, " createTime DESC ");


                    var datalist = (from item in data.Result.data
                                    select new
                                    {
                                        item,
                                        shopskudt = _ishopskudetailservices.QueryById(item.shopid).Result,
                                        shopsku = _ishopskuservices.QueryById(_ishopskudetailservices.QueryById(item.shopid).Result.skuid).Result,
                                        shopinfo = _ishoplistservices.QueryById(_ishopskuservices.QueryById(_ishopskudetailservices.QueryById(item.shopid).Result.skuid).Result.shopid).Result
                                    }).ToList();

                    // 添加worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("worksheet");
                    //添加头
                    //worksheet.Cells[1, 1].Value = "编号";
                    //worksheet.Cells[1, 2].Value = "购买者";
                    //worksheet.Cells[1, 3].Value = "购买订单号";
                    //worksheet.Cells[1, 4].Value = "购买商品";
                    //worksheet.Cells[1, 5].Value = "订单类型";
                    //worksheet.Cells[1, 6].Value = "商品规格";
                    //worksheet.Cells[1, 7].Value = "商品尺码";
                    //worksheet.Cells[1, 8].Value = "购买数量";
                    //worksheet.Cells[1, 9].Value = "购买价格";
                    //worksheet.Cells[1, 10].Value = "购买状态";
                    //worksheet.Cells[1, 11].Value = "购买名";
                    //worksheet.Cells[1, 12].Value = "手机";
                    //worksheet.Cells[1, 13].Value = "地址";
                    //worksheet.Cells[1, 14].Value = "快递单号";
                    //worksheet.Cells[1, 15].Value = "备注";
                    //worksheet.Cells[1, 16].Value = "购买时间";

                    // 店铺	平台单号	买家会员	支付金额	商品名称	商品代码	规格代码	规格名称	是否赠品	数量	价格	
                    // 商品备注	运费	买家留言	收货人	联系电话	联系手机	收货地址	省	市	区	邮编	
                    //    订单创建时间	订单付款时间	发货时间	物流单号	物流公司	卖家备注
                    //    发票种类	发票类型	发票抬头	纳税人识别号	开户行	账号	地址	电话
                    //    是否手机订单	是否货到付款	支付方式	支付交易号	真实姓名	身份证号	
                    //    仓库名称	预计发货时间	预计送达时间	订单类型	是否分销商订单	业务员
                    worksheet.Cells[1, 1].Value = "店铺";
                    worksheet.Cells[1, 2].Value = "平台单号";
                    worksheet.Cells[1, 3].Value = "买家会员";
                    worksheet.Cells[1, 4].Value = "支付金额";
                    worksheet.Cells[1, 5].Value = "商品名称";
                    worksheet.Cells[1, 6].Value = "商品代码";
                    worksheet.Cells[1, 7].Value = "规格代码";
                    worksheet.Cells[1, 8].Value = "规格名称";
                    worksheet.Cells[1, 9].Value = "是否赠品";
                    worksheet.Cells[1, 10].Value = "数量";
                    worksheet.Cells[1, 11].Value = "价格";
                    worksheet.Cells[1, 12].Value = "商品备注";
                    worksheet.Cells[1, 13].Value = "运费";
                    worksheet.Cells[1, 14].Value = "买家留言";
                    worksheet.Cells[1, 15].Value = "收货人";
                    worksheet.Cells[1, 16].Value = "联系电话";
                    worksheet.Cells[1, 17].Value = "联系手机";
                    worksheet.Cells[1, 18].Value = "收货地址";
                    worksheet.Cells[1, 19].Value = "省";
                    worksheet.Cells[1, 20].Value = "市";
                    worksheet.Cells[1, 21].Value = "区";
                    worksheet.Cells[1, 22].Value = "邮编";
                    worksheet.Cells[1, 23].Value = "订单创建时间";
                    worksheet.Cells[1, 24].Value = "订单付款时间";
                    worksheet.Cells[1, 25].Value = "发货时间";
                    worksheet.Cells[1, 26].Value = "物流单号";
                    worksheet.Cells[1, 27].Value = "物流公司";
                    worksheet.Cells[1, 28].Value = "卖家备注";
                    worksheet.Cells[1, 29].Value = "发票类型";
                    worksheet.Cells[1, 30].Value = "发票抬头";
                    worksheet.Cells[1, 31].Value = "纳税人识别号";
                    worksheet.Cells[1, 32].Value = "账号";
                    worksheet.Cells[1, 33].Value = "地址";
                    worksheet.Cells[1, 34].Value = "电话";
                    worksheet.Cells[1, 35].Value = "是否手机订单";
                    worksheet.Cells[1, 36].Value = "是否货到付款";
                    worksheet.Cells[1, 37].Value = "支付方式";
                    worksheet.Cells[1, 38].Value = "支付交易号";
                    worksheet.Cells[1, 39].Value = "真实姓名";
                    worksheet.Cells[1, 40].Value = "身份证号";
                    worksheet.Cells[1, 41].Value = "仓库名称";
                    worksheet.Cells[1, 42].Value = "预计发货时间";
                    worksheet.Cells[1, 43].Value = "预计送达时间";
                    worksheet.Cells[1, 44].Value = "订单类型";
                    worksheet.Cells[1, 45].Value = "是否分销商订单";
                    worksheet.Cells[1, 46].Value = "业务员";



                    for (int i = 0; i < datalist.Count(); i++) 
                    {
                        int j = i + 2;
                        //worksheet.Cells[j, 1].Value = datalist[i].item.id;
                        //worksheet.Cells[j, 2].Value = datalist[i].item.buyuid;
                        //worksheet.Cells[j, 3].Value = datalist[i].item.shopordernumber;
                        //worksheet.Cells[j, 4].Value = datalist[i].shopinfo.pName;
                        //worksheet.Cells[j, 5].Value = datalist[i].shopinfo.ptype;
                        //worksheet.Cells[j, 6].Value = datalist[i].shopsku.skuname;
                        //worksheet.Cells[j, 7].Value = datalist[i].shopskudt.detailname;
                        //worksheet.Cells[j, 8].Value = datalist[i].item.buyNum;
                        //worksheet.Cells[j, 9].Value = datalist[i].item.price;
                        //var tmps = "";
                        //// 未发货 配送中 确认收货 己完成
                        //int status =Convert.ToInt32(datalist[i].item.status);
                        //if (status == 1)
                        //{
                        //    tmps = "未发货";
                        //}
                        //else if (status == 2)
                        //{
                        //    tmps = "配送中";
                        //}
                        //else if (status == 3)
                        //{
                        //    tmps = "确认收货";
                        //}
                        //else
                        //{
                        //    tmps = "己完成";
                        //}
                        //worksheet.Cells[j, 10].Value = tmps;
                        //worksheet.Cells[j, 11].Value = datalist[i].item.buyname;
                        //worksheet.Cells[j, 12].Value = datalist[i].item.buyphone;
                        //worksheet.Cells[j, 13].Value = datalist[i].item.buyaddr;
                        //worksheet.Cells[j, 14].Value = datalist[i].item.trackingnumber;
                        //worksheet.Cells[j, 15].Value = datalist[i].item.reamrk;
                        //worksheet.Cells[j, 16].Value = datalist[i].item.createTime.ToString("yyyy-MM-dd hh:mm:ss");

                        var shopskudt = _ishopskudetailservices.QueryById(datalist[i].item.shopid).Result;
                        var user = _isysuserinfoservices.QueryById(datalist[i].item.buyuid).Result;
                        var shopsku = _ishopskuservices.QueryById(_ishopskudetailservices.QueryById(datalist[i].item.shopid).Result.skuid).Result;
                        var shopinfo = _ishoplistservices.QueryById(_ishopskuservices.QueryById(_ishopskudetailservices.QueryById(datalist[i].item.shopid).Result.skuid).Result.shopid).Result;
                        worksheet.Cells[j, 1].Value = "摩奇猴";
                        worksheet.Cells[j, 2].Value = datalist[i].item.shopordernumber;
                        worksheet.Cells[j, 3].Value = datalist[i].item.buyuid;
                        worksheet.Cells[j, 4].Value = datalist[i].item.price;
                        worksheet.Cells[j, 5].Value = shopinfo.pName+"-"+shopsku.skuname+"-"+shopskudt.detailname;
                        worksheet.Cells[j, 6].Value = shopinfo.id;
                        worksheet.Cells[j, 7].Value = shopsku.skuname;
                        worksheet.Cells[j, 8].Value = shopskudt.detailname;
                        worksheet.Cells[j, 9].Value = shopskudt.detailprice == 0 ? "是" : "否";
                        worksheet.Cells[j, 10].Value = datalist[i].item.buyNum;
                        worksheet.Cells[j, 11].Value = datalist[i].item.price;
                        worksheet.Cells[j, 12].Value = "";
                        worksheet.Cells[j, 13].Value = "";
                        worksheet.Cells[j, 14].Value = datalist[i].item.reamrk;
                        worksheet.Cells[j, 15].Value = datalist[i].item.buyname;
                        worksheet.Cells[j, 16].Value = datalist[i].item.buyphone;
                        worksheet.Cells[j, 17].Value = datalist[i].item.buyphone;
                        worksheet.Cells[j, 18].Value = datalist[i].item.buyaddr;
                        worksheet.Cells[j, 19].Value = "";
                        worksheet.Cells[j, 20].Value = "";
                        worksheet.Cells[j, 21].Value = "";
                        worksheet.Cells[j, 22].Value = "";
                        worksheet.Cells[j, 23].Value = datalist[i].item.createTime.ToString("yyyy-MM-dd hh:mm:ss");
                        worksheet.Cells[j, 24].Value = datalist[i].item.createTime.ToString("yyyy-MM-dd hh:mm:ss");
                        worksheet.Cells[j, 25].Value = datalist[i].item.sendoutdate;
                        worksheet.Cells[j, 26].Value = datalist[i].item.trackingnumber;
                        worksheet.Cells[j, 27].Value = datalist[i].item.company;
                        worksheet.Cells[j, 28].Value = datalist[i].item.reamrk;
                        worksheet.Cells[j, 29].Value = "";
                        worksheet.Cells[j, 30].Value = "";
                        worksheet.Cells[j, 31].Value = "";
                        worksheet.Cells[j, 32].Value = "";
                        worksheet.Cells[j, 33].Value = "";
                        worksheet.Cells[j, 34].Value = "";
                        worksheet.Cells[j, 35].Value = "";
                        worksheet.Cells[j, 36].Value = "";
                        worksheet.Cells[j, 37].Value = "";
                        worksheet.Cells[j, 38].Value = "";
                        worksheet.Cells[j, 39].Value = user.uRealName;
                        worksheet.Cells[j, 40].Value = "";
                        worksheet.Cells[j, 41].Value = "";
                        worksheet.Cells[j, 42].Value = "";
                        worksheet.Cells[j, 43].Value = "";
                        worksheet.Cells[j, 44].Value = shopinfo.ptype==1?"复购商品":"普通商品";
                        worksheet.Cells[j, 45].Value = "";
                        worksheet.Cells[j, 46].Value = "";
                    }
                    package.Save();
                   _idownexcelrecordservices.Add(new DownExcelRecord() { downdate=DateTime.Now, downname=sFileName, isdelete=false, downtype="buyorder" } );
                }
                var returnFile = File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                returnFile.FileDownloadName = sFileName;
                
                return returnFile;
            
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        [HttpPost]
        [Route("GetDownExcelList")]
        public async Task<MessageModel<dynamic>> GetDownExcelList()
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
                var data = await _idownexcelrecordservices.QueryPage(x =>
              (x.downname.ToString().Contains(key) || x.downtype.Contains(key)), pageindex, pagesize, " id DESC ");

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
        [Route("DeleteDownExcelList")]  
        public async Task<MessageModel<dynamic>> DeleteDownExcelList()
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

                if (!string.IsNullOrEmpty(id)) 
                {
                    var downmodel =await _idownexcelrecordservices.QueryById(id);
                    var sWebRootFolder = Directory.GetCurrentDirectory() + @"//shopimg//downinfo//"+ downmodel.downname;

                    if (System.IO.File.Exists(Path.GetFullPath(sWebRootFolder)))
                    {
                        System.IO.File.Delete(Path.GetFullPath(sWebRootFolder));
                    }
                    await _idownexcelrecordservices.Delete(downmodel);
                }
                result.code = 0;
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
        [Route("uploadShopdetialexcel")]
        public async Task<MessageModel<dynamic>> uploadShopdetialexcel()
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
                int resultnum = 0;
                if (files.Count > 0)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (ExcelPackage package = new ExcelPackage(files[0].OpenReadStream()))
                        {

                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            //2:平台单号   26 ：物流单号  27：物流公司   trackingnumber company
                            int rowCount = worksheet.Dimension.Rows;
                            int ColCount = worksheet.Dimension.Columns;
                            if (rowCount > 1) 
                            {
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var ordernumber = worksheet.Cells[row, 2].Value;
                                    var sendoutdate = worksheet.Cells[row, 25].Value;
                                    var trackingnumber = worksheet.Cells[row, 26].Value;
                                    var company = worksheet.Cells[row, 27].Value;
                                    if (ordernumber != null && trackingnumber !=null && company!=null) 
                                    {
                                        var shopdetail = await _ishopbuydetailserivces.Query(x => x.shopordernumber.Equals(ordernumber.ToString()));
                                        if (shopdetail.Count > 0) 
                                        {
                                            ShopBuyDetail model = shopdetail.First();
                                            if (string.IsNullOrEmpty(model.trackingnumber) && model.status==1) 
                                            {
                                                model.trackingnumber = trackingnumber.ToString();
                                                model.company = company.ToString();
                                                model.sendoutdate = sendoutdate != null ? Convert.ToDateTime(sendoutdate) : DateTime.Now;
                                                model.status = 5;
                                                await _ishopbuydetailserivces.Update(model);
                                                resultnum++;
                                            }
                                        }
                                    }
                                }
                            }
                            
                        }
                    }
                    //return "添加成功";
                }
                result.code = 200;
                result.success = true;
                result.msg = "更新" + resultnum + "条数据!";
                result.response = resultnum;
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

        #endregion

    }
}
