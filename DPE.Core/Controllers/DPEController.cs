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
    /// DPE管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/DPE")]
    [Authorize(Permissions.Name)]
    public class DPEController : Controller
    {

        private readonly IDPEServices _DPEServices;
        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;
        readonly IDPEexchangeServices _idpeexchangeservices;
        readonly IDPERecordsServices _idperecordsservices;

        readonly IBuyDPEHistoryServices _ibuydpehistoryservices;
        readonly IBuyDPEListServices _ibuydpelistservices;


        public DPEController(IBuyDPEListServices ibuydpelistservices, IBuyDPEHistoryServices ibuydpehistoryservices, IDPEServices dpeservices,
            IUser user, IUserInfoServices userInfoServices, IDPEexchangeServices idpeexchangeservices, IDPERecordsServices idperecordsservices)
        {
            _DPEServices = dpeservices;
            _user = user;
            _userInfoServices = userInfoServices;
            _idpeexchangeservices = idpeexchangeservices;
            _ibuydpehistoryservices = ibuydpehistoryservices;
            _ibuydpelistservices = ibuydpelistservices;
            _idperecordsservices = idperecordsservices;
        }

        /// <summary>
        /// 获取DPE数据 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDPEEexchange")]
        public async Task<MessageModel<dynamic>> GetDPEEexchange(int pageIndex, int pageSize)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {

                var data = await _idpeexchangeservices.QueryPage(a => a.amount > 0, pageIndex, pageSize, "createTime desc");
                result.response = (from item in data.data
                                   select new
                                   {
                                       uid = item.uID,
                                       item.amount,
                                       lasttotal = item.amount * item.price,
                                       date = Convert.ToDateTime(item.createTime).ToString("yy/MM/dd HH:mm")
                                   });


            }
            catch
            {
                return result;
            }
            return result;
        }

        /// <summary>
        /// 获取股票数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetBuyDPEHistory")]
        public async Task<MessageModel<dynamic>> GetBuyDPEHistory(int pagesize = 10, int pageindex = 1)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                if (_user.ID == 0)
                {
                    result.code = 10001;
                    result.msg = "身份已过期，请重新登录";
                    result.success = false;
                    return result;
                }

                var spinfo = await _ibuydpehistoryservices.QueryPage(x => x.uID == _user.ID, pageindex, pagesize, "createTime");
                result.response = (from item in spinfo.data
                                   orderby item.createTime descending
                                   select new
                                   {

                                   }); ;
                return result;

            }
            catch
            {
                return result;
            }

        }

        //财务管理 财务挂单
        [HttpPost]
        [Route("GetDPERecord")]
        public async Task<MessageModel<dynamic>> GetDPERecord(int pageSize, int pageIndex)
        {
            pageSize = pageSize == 0 ? 10 : pageSize;
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            var result = new PageModel<DPE.Core.Model.Models.DPE>();
            result = await _DPEServices.QueryPage(null, pageIndex, pageSize, " uID ");

            return new MessageModel<dynamic>
            {
                response =
                new
                {
                    datacount = result.dataCount,
                    data = (from item in result.data
                            select new
                            {
                                uid = item.uID,
                                amount = _idperecordsservices.Query(x => x.uID == item.uID).Result.Sum(x => x.amount),
                                dpeamount = item.amount
                            })
                }
            };
        }


        /// <summary>
        /// 获取Apple serarch
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSerarchApple")]
        public async Task<MessageModel<dynamic>> GetSerarchApple(long uid = 0, string option = "")
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                if (uid == 0)
                {
                    uid = _user.ID;
                    result.response = await _DPEServices.QueryPage(x => x.uID == uid, 1, 1);
                }
                else
                {
                    if (option.Equals("down"))
                    {
                        result.response = await _DPEServices.QueryPage(x => x.uID > uid, 1, 1);
                    }
                    else if (option.Equals("up"))
                    {
                        result.response = await _DPEServices.QueryPage(x => x.uID < uid, 1, 1, " uID desc");
                    }
                    else
                    {
                        result.response = await _DPEServices.QueryPage(x => x.uID == uid, 1, 1);
                    }

                }
                return result;

            }
            catch
            {
                return result;
            }

        }

        /// <summary>
        /// 获取排队记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetBuyDPEList")]
        public async Task<MessageModel<dynamic>> GetBuyDPEList(long uid = 0, int pagesize = 10, int pageindex = 1)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {

                if (_user.ID == 0)
                {
                    result.code = 10001;
                    result.msg = "登录已经过期，请重新登陆！";
                    result.success = false;
                    return result;
                }
                if (uid == -1)
                {
                    uid = _user.ID;
                }
                //      var spinfo = await _ibuydpelistservices.QueryPage(x => 1==1, pageindex, pagesize);
                var list = await _ibuydpelistservices.QuerySql("select uID,amount,createTime from BuyDPEList with(nolock) order by uID");
                int index = 0;
                var response = new List<dynamic>();
                foreach (var item in list)
                {
                    if (uid != 0 && item.uID == uid)
                    {
                        response.Add(new
                        {
                            index,
                            id = item.uID,
                            invest = item.amount,
                            total = list.Count,
                            date = Convert.ToDateTime(item.createTime).ToString("yy/MM/dd HH:mm:ss")
                        });
                    }
                    else if (uid == 0)
                    {
                        response.Add(new
                        {
                            index = index,
                            id = item.uID,
                            invest = item.amount,
                            total = list.Count,
                            date = Convert.ToDateTime(item.createTime).ToString("yy/MM/dd HH:mm:ss")
                        });
                    }
                    index = index + 1;
                }

                result.response = response;


                return result;

            }
            catch
            {
                return result;
            }

        }



    }
}
