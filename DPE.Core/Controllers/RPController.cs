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
using OracleInternal.Secure.Network;

namespace DPE.Core.Controllers
{

    /// <summary>
    /// RP管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/RP")]
    [Authorize(Permissions.Name)]
    public class RPController : Controller
    {
        readonly IUserInfoServices _userInfoServices;
        readonly IUser _user;
        private readonly IRPServices _IRPServices;
        private readonly IRPexchangeServices _irpexchangeservices;
        readonly ISysUserInfoServices _isysuserinfoservice;


        public RPController(ISysUserInfoServices isysuserinfoservice, IRPServices iRPpservices, IUserInfoServices userInfoServices, IUser user, IRPexchangeServices irpexchangeservices)
        {
            _IRPServices = iRPpservices;
            _userInfoServices = userInfoServices;
            _user = user;
            _irpexchangeservices = irpexchangeservices;
            _isysuserinfoservice = isysuserinfoservice;
        }

        /// <summary>
        /// 获取RP数据 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRPEexchange")]
        public async Task<MessageModel<dynamic>> GetRPEexchange()
        {
            var spinfo = await _irpexchangeservices.Query(x => x.uID == _user.ID);
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
        /// RP转出 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RPToEexchange")]
        public async Task<MessageModel<string>> RPToEexchange(long touid, decimal amount, string tpwd, string gcode)
        {
            var result = new MessageModel<string>();
            try
            {
                if (_user.ID == 0)
                {
                    //用户不存在
                    result.code = 61001;
                    result.success = false;
                    result.msg = "用户不存在";
                    return result;
                }

                var user = await _userInfoServices.GetUserInfo(_user.ID);



                if (user.RP < amount)
                {
                    //RP不足
                    result.code = 61003;
                    result.success = false;
                    result.msg = "RP不足";
                    return result;
                }

                var touser = await _userInfoServices.GetUserInfo(touid);
                if (touser == null)
                {
                    //RP转出用户不存在
                    result.code = 61002;
                    result.success = false;
                    result.msg = "RP转出用户不存在";
                    return result;
                }


                //判断交易密码
                var tranpwd = await _isysuserinfoservice.checkTrad(_user.ID, tpwd);
                if (tranpwd == null)
                {
                    //交易密码错误
                    result.code = 61004;
                    result.success = false;
                    result.msg = "交易密码错误";
                    return result;
                }
                //判断谷歌验证
                var trangode = await _isysuserinfoservice.checkGoogleKey(_user.ID, gcode);
                if (trangode == null)
                {
                    //谷歌验证错误
                    result.code = 61005;
                    result.success = false;
                    result.msg = "谷歌验证错误";
                    return result;
                }


                return await _irpexchangeservices.ProcessTransformToUser(_user.ID, "RP", amount, touser.uID);
            }
            catch
            {
                result.code = 61006;
                result.success = false;
                result.msg = "服务开小差了请稍后再试";
                return result;
            }
        }

        //EP转出记录
        [HttpPost]
        [Route("GetAdminRPExchangeList")]
        public async Task<MessageModel<dynamic>> GetAdminRPExchangeList()
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

                var data = await _irpexchangeservices.QueryPage(x => x.stype == 13 && x.amount < 0 &&
                 (x.uID.ToString().Contains(key) || x.fromID.ToString().Contains(key) || x.remark.Contains(key)), pageindex, pagesize, " createTime DESC ");

                result.response = data;
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
        [Route("GetAdminRPExchange")]
        public async Task<MessageModel<dynamic>> GetAdminRPExchange()
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
                var data = await _irpexchangeservices.QueryPage(x => x.stype.ToString().Contains(stype) &&
              (x.uID.ToString().Contains(key) || x.remark.Contains(key)), pageindex, pagesize, " createTime DESC ");

                result.response = data;
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
    }
}
