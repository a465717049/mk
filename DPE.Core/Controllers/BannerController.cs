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
    /// Banner 管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/Banner")]
    [Authorize(Permissions.Name)]
    public class BannerController : Controller
    {

        private readonly IBannerServices _ibannerservices;
        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;

        public BannerController(IUser user, IBannerServices ibannerservices, IUserInfoServices userInfoServices)
        {
            this._user = user;
            _userInfoServices = userInfoServices;
            _ibannerservices = ibannerservices;
        }

        /// <summary>
        /// 滚屏公告
        /// </summary>
        /// <param name="language">cn en</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetBanner")]
        public async Task<MessageModel<dynamic>> GetBanner(string language = "cn")
        {
            //_user.ID


            var spinfo = await _ibannerservices.Query(a => a.status == 0);

            //foreach (var item in spinfo)
            //{
            //    item.status = 1;
            //    await _ibannerservices.Update(item);
            //}
            //list = spinfo.Where(x => (DateTime.Now - Convert.ToDateTime(x.createtime)).TotalSeconds <= 1).Select(x => x.cnText).ToList() 
            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                code = 0,
                response = new { list = spinfo.Select(x => x.cnText).ToList() }
            };
        }
    }
}
