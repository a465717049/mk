using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using DPE.Core.AuthHelper.OverWrite;
using DPE.Core.Common.Helper;
using DPE.Core.Common.HttpContextUser;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.AuthHelper;
using DPE.Core.IServices;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace DPE.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class RelationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfoServices _userInfoServices;
        private readonly ISysUserInfoServices _sysUserInfoServices;
        private readonly IUser _user;

        public RelationController(IUnitOfWork unitOfWork, IUser iuser, ISysUserInfoServices sysUserInfoServices, IUserInfoServices userInfoServices)
        {
            _unitOfWork = unitOfWork;
            _sysUserInfoServices = sysUserInfoServices;
            _userInfoServices = userInfoServices;
            _user = iuser;
        }

        /// <summary>
        /// 获取家族账号清单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> GetFimalyList(long uid)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();

            try
            {

                var friendData = await _sysUserInfoServices.GetFimalyList(_user.ID, uid);
                if (friendData == null || friendData.Rows.Count <= 0)
                {
                    result.code = 63001;
                    result.success = false;
                    return result;
                }

                result.code = 0;
                result.success = true;
                result.response = friendData;

                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.code = 1007;
                result.msg = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// 朋友清单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> GetFriendsList(long uid)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();

            try
            {


                var friendData = await _sysUserInfoServices.GetFriendsList(_user.ID, uid);
                if (friendData == null || friendData.Rows.Count <= 0)
                {
                    result.code = 63001;
                    result.success = false;
                    return result;
                }

                result.code = 0;
                result.success = true;
                result.response = friendData;

                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.code = 1007;
                result.msg = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// 朋友关係
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> GetRelationList(long uid)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();

            try
            {
                if (uid == 0)
                {
                    Stream sm = HttpContext.Request.Body;
                    if (sm.Length > 0)
                    {
                        byte[] buff = new byte[sm.Length];
                        sm.Read(buff, 0, buff.Length);
                        string str = Encoding.UTF8.GetString(buff);
                        long.TryParse(str.Split("=")[1], out uid);
                    }
                }
                if (uid == 0) uid = _user.ID;

                var friendData = await _sysUserInfoServices.GetRelationList(uid, _user.ID);
                if (friendData == null || friendData.Rows.Count <= 0)
                {
                    result.code = 63001;
                    result.success = false;
                    return result;
                }

                result.code = 0;
                result.success = true;
                result.response = friendData;

                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.code = 1007;
                result.msg = ex.Message;
                return result;
            }

        }
    }
}