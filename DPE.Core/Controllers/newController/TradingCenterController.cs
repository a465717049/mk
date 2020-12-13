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

namespace DPE.Core.Controllers
{

    ///  radingCenter 服务中心
    [Produces("application/json")]
    [Route("api/radingCenter")]
    [Authorize(Permissions.Name)]
    public class TradingCenterController : Controller
    {

        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;


        public TradingCenterController(IUser user, IUserInfoServices userInfoServices)
        {
            _user = user;
            _userInfoServices = userInfoServices;
        }


        /// <summary>
        /// 服务中心
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTradingCenter")]
        public async Task<MessageModel<dynamic>> GetTradingCenter()
        {
            long uid = _user.ID;
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            return result;
        }

    }

}
