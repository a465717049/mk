using System;
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
    /// News 管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/News")]
    [Authorize(Permissions.Name)]
    public class NewsController : Controller
    {

        private readonly INewsServices _inewsservices;
        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;

        public NewsController(IUser user, INewsServices inewsservices, IUserInfoServices userInfoServices)
        {
            this._user = user;
            _userInfoServices = userInfoServices;
            _inewsservices = inewsservices;
        }

        /// <summary>
        /// 公告
        /// </summary>
        /// <param name="language">cn en</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetNews")]
        public async Task<MessageModel<NewsViewModels>> GetNews(string language = "cn")
        {
            //_user.ID
            var spinfo = await _inewsservices.Query();


            return new MessageModel<NewsViewModels>()
            {
                success = true,
                msg = "",
                response = new NewsViewModels()
                {
                    code = 0,
                    title = spinfo[0].title,
                    time = DateHelper.GetCreatetime(Convert.ToDateTime(spinfo[0].createTime)),
                    content = spinfo[0].context

                }
            };
        }


        /// <summary>
        /// 公告
        /// </summary>
        /// <param name="language">cn en</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetNewsWeb")]
        public async Task<MessageModel<dynamic>> GetNewsWeb(int type)
        {
            //_user.ID
            var spinfo = await _inewsservices.Query(x => x.isDelete != 1 && x.ntype.Value == type);


            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = (from item in spinfo
                            orderby item.createTime descending
                            select new
                            {
                                item.id,
                                item.title,
                                item.message,
                                img = item.newsimg,
                                detail = item.context,
                                date = Convert.ToDateTime(item.createTime).ToString("yyyy-MM-dd HH:mm:ss")
                            })
            };
        }

        /// <summary>
        /// 公告
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language">cn en</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetNewsDeatilWeb")]
        public async Task<MessageModel<dynamic>> GetNewsDeatilWeb(long id)
        {
            //_user.ID
            var spinfo = await _inewsservices.QueryById(id);
            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response =
                             new
                             {
                                 spinfo.id,
                                 spinfo.title,
                                 spinfo.message,
                                 img = spinfo.newsimg,
                                 detail = spinfo.context,
                                 date = Convert.ToDateTime(spinfo.createTime).ToString("yyyy-MM-dd HH:mm:ss")
                             }
            };
        }
    }
}