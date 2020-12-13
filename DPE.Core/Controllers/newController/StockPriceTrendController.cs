using System;
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
using SqlSugar;

namespace DPE.Core.Controllers
{

    ///  StockPriceTrend管理
    [Produces("application/json")]
    [Route("api/StockPriceTrend")]
    [Authorize(Permissions.Name)]
    public class StockPriceTrendController : Controller
    {

        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;
        readonly IStockServices _istockservices;
        readonly IBuyDPEListServices _ibuydpelistservices;

        readonly IEPexchangeServices _iepexchangeservices;

        readonly IDPEexchangeServices _idpeexchangeservices;

        public StockPriceTrendController(IDPEexchangeServices idpeexchangeservices, IEPexchangeServices iepexchangeservices, IBuyDPEListServices ibuydpelistservices, IUser user, IUserInfoServices userInfoServices, IStockServices istockservices)
        {
            _user = user;
            _userInfoServices = userInfoServices;
            _istockservices = istockservices;
            _ibuydpelistservices = ibuydpelistservices;
            _iepexchangeservices = iepexchangeservices;
            _idpeexchangeservices = idpeexchangeservices;
        }


        /// <summary>
        /// 价格曲綫
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetStockPriceTrend")]
        public async Task<MessageModel<dynamic>> GetStockPriceTrend()
        {
            var totalamount = (await _ibuydpelistservices.Query()).Sum(x => x.amount);
            var totalwithdraw = (await _iepexchangeservices.Query(x => x.uID == _user.ID && (x.stype == 1 || x.stype == 2 || x.stype == 3))).Sum(x => x.amount);
            var invest = (await _idpeexchangeservices.Query(x => x.uID == _user.ID && x.stype == 11 && x.amount > 0)).Sum(x => x.amount);

            var resull = await _istockservices.QuerySql(
                string.Format("select top 30 CAST(a.createTime as date)createTime,max(price)price from stock a join(" +
                        " select  max(createTime)createTime from stock group by CAST(createTime as date) ) b on a.createTime = b.createTime" +
                        " group by  CAST(a.createTime as date)" +
                        " order by CAST(a.createTime as date)  desc")
                );

            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = new
                {
                    totalamount = totalamount,
                    totalwithdraw = totalwithdraw,
                    invest = invest,
                    list = resull
                }
            };
        }

    }
}