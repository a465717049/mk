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


namespace DPE.Core.Controllers
{
    /// <summary>
    /// DPEProfit 利润计算器【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/DPEProfit")]
    [Authorize(Permissions.Name)]
    public class DPEProfitController : Controller
    {

        private readonly IDPEProfitServices _idpeprofitservices;
        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;

        public DPEProfitController(IUser user, IDPEProfitServices idpeprofitservices, IUserInfoServices userInfoServices)
        {

            this._user = user;
            _userInfoServices = userInfoServices;
            _idpeprofitservices = idpeprofitservices;
        }


        /// <summary>
        /// 利润计算器
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SumDPEProfit")]
        public async Task<MessageModel<DPEProfitViewModels>> SumDPEProfit()
        {
            var list = await _idpeprofitservices.Query();

            return new MessageModel<DPEProfitViewModels>()
            {
                success = true,
                msg = "",
                response = new DPEProfitViewModels()
                {
                    list = list.Select(x => string.Format("{0}.{1}", x.mouth.ToString(),Convert.ToInt32(x.rate).ToString())).ToList()
                }
            };
        }
    }
}
