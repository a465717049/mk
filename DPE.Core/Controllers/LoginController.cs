using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DPE.Core.AuthHelper;
using DPE.Core.AuthHelper.OverWrite;
using DPE.Core.Common;
using DPE.Core.Common.Helper;
using DPE.Core.IServices;
using DPE.Core.Model;
using DPE.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Newtonsoft.Json;

namespace DPE.Core.Controllers
{
    /// <summary>
    /// 登录管理【无权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/Login")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        readonly ISysUserInfoServices _sysUserInfoServices;
        readonly IUserRoleServices _userRoleServices;
        readonly IRoleServices _roleServices;
        readonly PermissionRequirement _requirement;
        private readonly IRoleModulePermissionServices _roleModulePermissionServices;


        readonly IRedisCacheManager _redisCacheManager;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        /// <param name="requirement"></param>
        /// <param name="roleModulePermissionServices"></param>
        public LoginController(IRedisCacheManager irediscachemanager, ISysUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices, IRoleServices roleServices, PermissionRequirement requirement, IRoleModulePermissionServices roleModulePermissionServices)
        {
            this._sysUserInfoServices = sysUserInfoServices;
            this._userRoleServices = userRoleServices;
            this._roleServices = roleServices;
            _requirement = requirement;
            _roleModulePermissionServices = roleModulePermissionServices;
            _redisCacheManager = irediscachemanager;
        }



        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="pass">密码</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [Route("Login")]
        public async Task<MessageModel<TokenInfoViewModel>> Login(string name = "", string pass = "")
        {
            string jwtStr = string.Empty;
            if (string.IsNullOrEmpty(name))
            {
                Stream sm = HttpContext.Request.Body;
                byte[] buff = new byte[sm.Length];
                sm.Read(buff, 0, buff.Length);
                string str = Encoding.UTF8.GetString(buff);
                dynamic dy = JsonConvert.DeserializeObject(str);
                name = dy.name;
                pass = dy.pass;
            }

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
            {
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = false,
                    msg = "用户名或者密码不能为空",
                    code = 1008
                };
            }

            pass = MD5Helper.MD5Encrypt32(pass);

            var user = await _sysUserInfoServices.Query(d => d.uLoginName == name && d.uLoginPWD == pass && d.isDelete == false);
            long uid = 0;
            if (user.Count == 0 && long.TryParse(name, out uid))
            {
                user = await _sysUserInfoServices.Query(d => d.uID == uid && d.isDelete == false);
            }
            if (user.Count > 0)
            {
                if (user.FindAll(a => a.uLoginPWD == pass).Count == 0)
                {
                    return new MessageModel<TokenInfoViewModel>()
                    {
                        success = false,
                        msg = "用户密码不正确",
                        code = 1008
                    };
                }

                var userRoles = await _sysUserInfoServices.GetUserRoleNameStr(name, pass);
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(JwtRegisteredClaimNames.Jti, user.FirstOrDefault().uID.ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
                claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));


                // ids4和jwt切换
                // jwt
                if (!Permissions.IsUseIds4)
                {
                    var data = await _roleModulePermissionServices.RoleModuleMaps();
                    var list = (from item in data
                                where item.IsDeleted == false
                                orderby item.Id
                                select new PermissionItem
                                {
                                    Url = item.Module?.LinkUrl,
                                    Role = item.Role?.Name.ObjToString(),
                                }).ToList();

                    _requirement.Permissions = list;
                }


                var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);

                try
                {
                    string redisuid = "userid:" + user.FirstOrDefault().uID.ToString();
                    string redistoken = MD5Helper.MD5Encrypt32(token.token);
                    if (_redisCacheManager.Get(redisuid))
                    {
                        string tmptoken = _redisCacheManager.GetValue(redisuid);
                        _redisCacheManager.SetString(redisuid, redistoken, TimeSpan.FromMinutes(300));
                        _redisCacheManager.Remove(tmptoken);
                    }
                    _redisCacheManager.SetString(redisuid, redistoken, TimeSpan.FromMinutes(300));
                    _redisCacheManager.SetString(redistoken, redisuid, TimeSpan.FromMinutes(300));
                }
                catch
                {

                }


                return new MessageModel<TokenInfoViewModel>()
                {
                    success = true,
                    msg = "登录成功",
                    response = token
                };
            }
            else
            {
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = false,
                    code = 1009,
                    msg = "用户登录失败",
                };

            }
        }

        /// <summary>
        /// 请求刷新Token（以旧换新）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<MessageModel<TokenInfoViewModel>> RefreshToken(string token = "")
        {
            string jwtStr = string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = false,
                    msg = "token无效，请重新登录！",
                };
            }
            var tokenModel = JwtHelper.SerializeJwt(token);
            if (tokenModel != null && tokenModel.Uid > 0)
            {
                var user = await _sysUserInfoServices.QueryById(tokenModel.Uid);
                if (user != null)
                {
                    var userRoles = await _sysUserInfoServices.GetUserRoleNameStr(user.uLoginName, user.uLoginPWD);
                    //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                    var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.uLoginName),
                    new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ObjToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
                    claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

                    //用户标识
                    var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                    identity.AddClaims(claims);

                    var refreshToken = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                    return new MessageModel<TokenInfoViewModel>()
                    {
                        success = true,
                        msg = "获取成功",
                        response = refreshToken
                    };
                }
            }

            return new MessageModel<TokenInfoViewModel>()
            {
                success = false,
                msg = "认证失败！",
            };
        }



    }
}