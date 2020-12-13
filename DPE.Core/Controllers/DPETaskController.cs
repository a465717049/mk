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
    /// DPETask管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/DPETask")]
    [Authorize(Permissions.Name)]
    public class DPETaskController : Controller
    {

        private readonly IDPETaskServices _idpetaskservices;
        private readonly IUserTaskServices _iusertaskservices;
        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;

        readonly IUserDataServices _iuserdataservices;

        public DPETaskController(IUserDataServices iuserdataservices, IUser user, IDPETaskServices idpetaskservices, IUserInfoServices userInfoServices, IUserTaskServices iusertaskservices)
        {

            this._user = user;
            _userInfoServices = userInfoServices;
            _idpetaskservices = idpetaskservices;
            _iusertaskservices = iusertaskservices;
            _iuserdataservices = iuserdataservices;
        }


        /// <summary>
        /// 任务列表  0 不能领取 1 已领取 2 未领取
        /// </summary>
        /// <param name="language">cn en</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDPETask")]
        public async Task<MessageModel<DPETaskViewModels>> GetDPETask(string language = "cn")
        {
            //_user.ID
            var user = await _userInfoServices.GetUserInfo(_user.ID);
            var spinfo = await _idpetaskservices.Query();
            return new MessageModel<DPETaskViewModels>()
            {
                success = true,
                msg = "",
                response = new DPETaskViewModels()
                {
                    num = _iuserdataservices.Query(x => x.uID == _user.ID).Result.First().invites.ObjToInt(),
                    list = (from item in spinfo
                            orderby item.minNum
                            select new DPETaskViewModelsList

                            {
                                id = item.id,
                                name = item.taskName,
                                url = item.taskUrl,
                                num = item.minNum,
                                status = user.honorLevel < item.taskLevel ? 0 : _iusertaskservices.Query(x => x.uID == user.uID && x.taskID == item.id).Result.Count == 0 ? 2 : 1,
                                value = item.taskvalue.ObjToInt()
                            }).ToList()
                }
            };


        }

    }
}
