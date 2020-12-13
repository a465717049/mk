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
    /// FAQ管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/IndexData")]
    [Authorize(Permissions.Name)]
    public class IndexDataController : Controller
    {

        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;


        public IndexDataController(IUser user, IUserInfoServices userInfoServices)
        {
            _user = user;
            _userInfoServices = userInfoServices;
        }


        /// <summary>
        ///  首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetIndexData")]
        public async Task<MessageModel<dynamic>> GetIndexData()
        {

            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                code = 0
            };
        }
    }
}
