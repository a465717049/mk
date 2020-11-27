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
    /// DPEActivities 管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/DPEActivities")]
    [Authorize(Permissions.Name)]
    public class DPEActivitiesController :Controller
    {

        private readonly IDPEActivitiesServices _idpeactivitiesservices;
        private readonly IUserActivitiesServices _iuseractivitiesservices;
        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;

        public DPEActivitiesController(IUser user, IDPEActivitiesServices idpeactivitiesservices, IUserInfoServices userInfoServices, IUserActivitiesServices iuseractivitiesservices)
        {

            this._user = user;
            _userInfoServices = userInfoServices;
            _idpeactivitiesservices = idpeactivitiesservices;
            _iuseractivitiesservices = iuseractivitiesservices;
        }

        /// <summary>
        /// 活动列表
        /// </summary>
        /// <param name="language">cn en</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDPEActivities")]
        public async Task<MessageModel<DPEActivitiesViewModels>> GetDPEActivities(string language = "cn")
        {
            //_user.ID
            var spinfo = await _idpeactivitiesservices.Query();
            return new MessageModel<DPEActivitiesViewModels>()
            {
                success = true,
                msg = "",
                response = new DPEActivitiesViewModels()
                {
                    list = (from item in spinfo
                            orderby item.id descending
                            select new DPEActivitiesViewModelsList
                            {
                                id = item.id.ObjToInt(),
                                name = item.context,
                                num = item.qty.ObjToInt(),
                                gold = Convert.ToInt32( item.amount),
                                status = _iuseractivitiesservices.CkUserActivities(_user.ID, item.id).Result.ObjToInt()
                            }).ToList()
                }
            };

        }

    }
}
