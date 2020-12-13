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
    [Route("api/FAQ")]
    [Authorize(Permissions.Name)]
    public class FAQController : Controller
    {

        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;
        readonly IFAQServices _ifaqservices;


        public FAQController(IUser user, IUserInfoServices userInfoServices, IFAQServices ifaqservices)
        {
            _user = user;
            _userInfoServices = userInfoServices;
            _ifaqservices = ifaqservices;
        }


        /// <summary>
        /// GetFAQ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFAQ")]
        public async Task<MessageModel<dynamic>> GetFAQ()
        {
            var result = await _ifaqservices.Query();
            return new MessageModel<dynamic>
            {
                success = true,
                msg = "",
                code = 0,
                response = (from item in result
                            select new
                            {
                                id = item.id,
                                questions = item.Questions,
                                answer = item.Answer
                            })
            };
        }


        /// <summary>
        /// AddFAQ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddFAQ")]
        public async Task<MessageModel<dynamic>> AddFAQ(string questions, string answer)
        {
            FAQ faq = new FAQ() { Questions = questions, Answer = answer };
            var result = await _ifaqservices.Add(faq);
            return new MessageModel<dynamic>
            {
                success = true,
                msg = "",
                code = 0
            };
        }
    }


}
