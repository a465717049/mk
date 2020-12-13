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
    /// IRP管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/IRP")]
    [Authorize(Permissions.Name)]
    public class IRPController : Controller
    {
        private readonly IIRPServices _IIRPServices;
        readonly IUserInfoServices _userInfoServices;
        readonly IUser _user;

        public IRPController(IIRPServices iirpservices, IUserInfoServices userInfoServices, IUser user)
        {
            _IIRPServices = iirpservices;
            _userInfoServices = userInfoServices;
            _user = user;
        }
        /// <summary>
        /// 获取IRP数据根据 uid
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<IRP>> GetIRPByUid()
        {
            var user = await _userInfoServices.GetUserInfo(_user.ID);
            var data = new MessageModel<IRP>();

            var userinfo = await _IIRPServices.QueryById(user.uID);
            if (userinfo != null)
            {
                data.response = userinfo;
                data.success = true;
            }

            return data;
        }



    }
}
