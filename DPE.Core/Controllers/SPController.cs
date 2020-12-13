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
    /// SP 银币管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/SP")]
    [Authorize(Permissions.Name)]
    public class SPController : Controller
    {

        private readonly ISPServices _ISPServices;
        private readonly ISPexchangeServices _ispexchangeservices;
        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;

        readonly ISplitRecordsServices _isplitrecordsservices;
        public SPController(ISplitRecordsServices isplitrecordsservices, ISPServices ispservices, IUser user, ISPexchangeServices ispexchangeservices, IUserInfoServices userInfoServices)
        {
            this._user = user;
            _ISPServices = ispservices;
            _ispexchangeservices = ispexchangeservices;
            _userInfoServices = userInfoServices;
            _isplitrecordsservices = isplitrecordsservices;
        }

        /// <summary>
        /// 获取SP数据根据 uid
        /// 【无权限】
        /// </summary>
        /// <param name="uid">uid</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("GetSPInfo")]
        public async Task<MessageModel<SP>> GetSPInfo()
        {
            var data = new MessageModel<SP>();
            var userinfo = await _ISPServices.QueryById(_user.ID);
            if (userinfo != null)

            {
                data.response = userinfo;
                data.success = true;
            }
            else
            {
                data.code = 1;
            }

            return data;
        }

        /// <summary>
        /// 仓里银币列表记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSPExchange")]
        public async Task<MessageModel<dynamic>> GetSPExchange(string language = "cn")
        {

            var spinfo = await _ispexchangeservices.GetSPExchange(_user.ID);

            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = new
                {
                    list = (from item in spinfo
                            orderby item.id descending
                            select new ExchangeViewList
                            {
                                msg = item.remark,
                                time = DateHelper.GetCreatetime(Convert.ToDateTime(item.createTime))
                            }).ToList()
                }
            };
        }

        //SplitRecords 


        /// <summary>
        /// 获取DPE数据 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSplitRecords")]
        public async Task<MessageModel<dynamic>> GetSplitRecords(long uid = 0)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                if (uid == 0)
                {
                    result.response = (from item in await _isplitrecordsservices.Query(x => x.uID == _user.ID)
                                       orderby item.createTime descending
                                       select new
                                       {
                                           uid = item.uID,
                                           splitcount = item.splitCount,
                                           splitlater = item.splitLater,
                                           splitrate = item.splitRate,
                                           splitbefore = item.splitBefore,
                                           date = Convert.ToDateTime(item.createTime).ToString("yyyy/MM/dd")
                                       });
                }
                else
                {
                    result.response = (from item in await _isplitrecordsservices.Query(x => x.uID == _user.ID)
                                       orderby item.createTime descending
                                       select new
                                       {
                                           uid = item.uID,
                                           splitcount = item.splitCount,
                                           splitlater = item.splitLater,
                                           splitrate = item.splitRate,
                                           splitbefore = item.splitBefore,
                                           date = Convert.ToDateTime(item.createTime).ToString("yyyy/MM/dd")
                                       });
                }
                return result;

            }
            catch
            {
                return result;
            }
        }

        [HttpPost]
        [Route("GetAdminSPExchange")]
        public async Task<MessageModel<dynamic>> GetAdminSPExchange()
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
                var data = await _ispexchangeservices.QueryPage(x => x.stype.ToString().Contains(stype) &&
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





