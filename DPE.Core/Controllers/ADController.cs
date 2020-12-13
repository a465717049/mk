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

    /// AD管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/AD")]
    [Authorize(Permissions.Name)]
    public class ADController : Controller
    {


        private readonly IADServices _adservices;
        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;

        private readonly IIRPServices _iirpservices;


        public ADController(IADServices adspservices, IUser user, IUserInfoServices userInfoServices, IIRPServices iirpservices)
        {
            _adservices = adspservices;
            _user = user;
            _userInfoServices = userInfoServices;
            _iirpservices = iirpservices;
        }


        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SignIn")]
        public async Task<MessageModel<dynamic>> SignIn()
        {
            int tmpcode = 0;
            bool tmpsuccess = true;
            if (_user.ID.ObjToInt() == 0)
            {
                tmpcode = 1003;
                tmpsuccess = false;
            }


            if (tmpsuccess)
            {
                var isfig = await _iirpservices.SetAdSignIn(_user.ID);
                if (!isfig)
                {
                    tmpcode = 2002;
                    tmpsuccess = false;
                }
            }
            else if (tmpcode != 1003)
            {
                tmpcode = 2001;
                tmpsuccess = false;
            }

            return new MessageModel<dynamic>()
            {
                success = tmpsuccess,
                msg = "",
                code = tmpcode
            };

        }
    }
}
