using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DPE.Core.AuthHelper;
using DPE.Core.AuthHelper.OverWrite;
using DPE.Core.Common.Helper;
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
    /// 注册
    /// </summary>
    [Produces("application/json")]
    [Route("api/Register")]
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        readonly ISysUserInfoServices _sysUserInfoServices;
    


        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        public RegisterController(ISysUserInfoServices sysUserInfoServices)
        {
            this._sysUserInfoServices = sysUserInfoServices;
         
        }



        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="name">用戶名</param>
        /// <param name="pass">密碼</param>
        /// <param name="code">邀請碼</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public async Task<MessageModel<sysUserInfoViewModel>> Register(string name = "", string pass = "", string code = "")
        {
            string jwtStr = string.Empty;

            if (string.IsNullOrEmpty(name) )
            {
                return new MessageModel<sysUserInfoViewModel>()
                {
                    success = false,
                    code = 101
                };
            }
            if (string.IsNullOrEmpty(pass))
            {
                return new MessageModel<sysUserInfoViewModel>()
                {
                    success = false,
                    code = 102
                };
            }
            pass = MD5Helper.MD5Encrypt32(pass);

            var user = await _sysUserInfoServices.Query(d => d.uLoginName == name);
            if (user.Count > 0)
            {
                return new MessageModel<sysUserInfoViewModel>()
                {
                    success = false,
                    code = 1001
                };
              
            }
            else
            {
                long yid = 0;
                bool hasUser= long.TryParse(code, out yid);

                var yuser = await _sysUserInfoServices.Query(d => d.uID == yid);
                if (yuser.Count > 0 || string.IsNullOrEmpty(code))
                {
                    hasUser = true;
                }
                else
                {
                    hasUser = false;
                }
                if (!hasUser)
                {
                    return new MessageModel<sysUserInfoViewModel>()
                    {
                        success = false,
                        code = 1002
                    };
                }
                else
                {
                   
                    var s = await _sysUserInfoServices.Register(name, pass, yid);
                    if (s.uID > 0)
                    {
                        return new MessageModel<sysUserInfoViewModel>()
                        {
                            code = 0,
                            success = true
                        };
                    }
                    else
                    {
                        return new MessageModel<sysUserInfoViewModel>()
                        {
                            code = 1009,
                            success = false
                        };
                    }
                    
                }
            }
        }
    }
}