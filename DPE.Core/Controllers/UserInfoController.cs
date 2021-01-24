using System;
using System.Collections.Generic;
using System.Data;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
using SqlSugar;

namespace DPE.Core.Controllers
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Produces("application/json")]
    [Route("api/UserInfo")]
    [Authorize(Permissions.Name)]
    public class UserInfoController : Controller
    {
        readonly IUserInfoServices _userInfoServices;
        readonly IEPServices _EPServices;
        readonly IEPexchangeServices _EPexchangeServices;
        readonly ISPexchangeServices _SPexchangeServices;
        readonly IUser _user;
        readonly PermissionRequirement _requirement;
        readonly ISysUserInfoServices _isysuserinfoservices;
        /// <summary>
        /// 构造函数注入
        /// </summary>

        public UserInfoController(ISysUserInfoServices isysuserinfoservices,IEPServices EPServices, IUserInfoServices userInfoServices, IEPexchangeServices ePexchangeServices, ISPexchangeServices spexchangeServices, PermissionRequirement requirement, IUser user)
        {
            _userInfoServices = userInfoServices;
            _user = user;
            _requirement = requirement;
            _EPServices = EPServices;
            _EPexchangeServices = ePexchangeServices;
            _SPexchangeServices = spexchangeServices;
            _isysuserinfoservices = isysuserinfoservices;
        }



        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        [HttpPost]
        [Route("GetUserInfo")]
        public async Task<MessageModel<UserInfoViewModel>> GetUserInfo()
        {
            string jwtStr = string.Empty;
            if (_user.ID == 0)
            {
                return new MessageModel<UserInfoViewModel>()
                {
                    success = false,
                    code = 403,
                    msg = "",
                };
            }
            var user = await _userInfoServices.GetUserInfo(_user.ID);

            return new MessageModel<UserInfoViewModel>()
            {
                success = true,
                msg = "",
                response = new UserInfoViewModel()
                {
                    apple = user.DPE.Value,
                    autonym = user.isSetIDNumber.Value ? 1 : 0,
                    coin = user.IRP.Value,
                    coin_location = user.addr,
                    trc_address = user.trcAddress,
                    create_time = DateHelper.GetCreatetime(user.uCreateTime),
                    farmers = user.uStatus,
                    gold = user.EP.Value,
                    gold_zu = user.AD.Value,
                    lv_name = user.honorLevel.Value,
                    manure = user.Manure.Value,
                    nickname = user.uNickName,
                    photo = user.Photo,
                    seed = user.SP.Value,
                    sex = user.sex.Value,
                    signin = user.Signin,
                    isBindGoogle = user.isBindGoogle,
                    service = user.isService.Value,
                    uid = user.uID.ToString(),
                    friend = user.friends.ObjToInt(),
                    weekly = user.weeklyMoney.Value,
                    rp = user.RP.Value,
                    sum = user.Sum.Value,
                    dynamicTotal = user.DynamicTotal.Value,
                    lprofit = user.LProfit.Value,
                    rprofit = user.RProfit.Value,
                    isSetIDNumber = user.isSetIDNumber,
                    alipayaccount = user.alipayaccount,
                    alipayname = user.alipayname,
                    bankaddr = user.bankaddr,
                    bankidcard = user.bankidcard,
                    bankname = user.bankname
                }
            };
        }


        /// <summary>
        /// 获取EP数据根据 uid
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("checkEPUser")]
        public async Task<MessageModel<dynamic>> checkEPUser(long uid, string type)
        {
            var data = new MessageModel<dynamic>();

            var userinfo = (await _EPServices.checkEPUser(uid, _user.ID, type));
            if (userinfo != "")
            {
                data.response = new { enable = true, name = userinfo };
                data.success = true;
            }
            else
            {

                data.response = new { enable = false };
                data.success = true;
            }

            return data;
        }
        /// <summary>
        /// 获取EP数据根据 uid
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("checkUser")]
        public async Task<MessageModel<dynamic>> checkUser(long uid)
        {
            
            var data = new MessageModel<dynamic>();
            try
            {
                var userinfo = await _userInfoServices.GetUserInfo(uid);
                if (userinfo.uID > 0)
                {
                    data.response = new { enable = true };
                    data.success = true;
                    data.code = 0;

                }
                else
                {

                    data.response = new { enable = false };
                    data.success = true;
                    data.code = -1;
                }
                return data;
            }
            catch
            {
                data.response = new { enable = false };
                data.success = true;
                data.code = -1;
                return data;
            }
        }

        /// <summary>
        /// 获取用户总收益
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShouru")]
        public async Task<MessageModel<dynamic>> GetShouru()
        {
            var data = new MessageModel<dynamic>();

            var ep = (await _EPexchangeServices.Query(a => a.uID == _user.ID && a.amount > 0 && a.stype == 4));
            var sp = (await _SPexchangeServices.Query(a => a.uID == _user.ID && a.amount > 0 && a.stype == 4));
            if (ep.Count > 0 && sp.Count > 0)
            {
                decimal sum = 0;
                foreach (var item in ep)
                {
                    sum += item.amount.Value;
                }
                foreach (var item in sp)
                {
                    sum += item.amount.Value;
                }
                data.response = new { sum };
                data.success = true;
            }
            else
            {

                data.response = new { sum = 0 };
                data.success = true;
            }

            return data;
        }


        //获取用户所有信息
        [Route("GetALLUserInfo")]
        public async Task<MessageModel<dynamic>> GetALLUserInfo(int pageindex, int pagesize, string key = "",string utid = "",
            string ulevel = "", string uhonur = "", string ustatus = "", string startdate = "", string enddate = "")
        {
            pageindex = pageindex == 0 ? 1 : pageindex;
            pagesize = pagesize == 0 ? 20 : pagesize;
            string jwtStr = string.Empty;
            if (_user.ID == 0)
            {
                return new MessageModel<dynamic>()
                {
                    success = false,
                    code = 403,
                    msg = "",
                };
            }
            var user = await _userInfoServices.GetAllUserInfo(pageindex, pagesize, key, utid, ulevel,uhonur,ustatus,startdate,enddate, "uID");

            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response =

                new
                {
                    datacount = user.dataCount,
                    data = (from item in user.data
                            orderby item.uID
                            select new
                            {
                                apple = item.DPE.Value,
                                autonym = item.isSetIDNumber.Value ? 1 : 0,
                                coin = item.IRP.Value,
                                coin_location = item.addr,
                                create_time = item.uCreateTime.ToString("yyyy-MM-dd"),
                                farmers = item.uStatus,
                                gold = item.EP.Value,
                                gold_zu = item.AD.Value,
                                lv_name = item.honorLevel.Value,
                                manure = item.Manure.Value,
                                nickname = item.uNickName,
                                photo = item.Photo,
                                seed = item.SP.Value,
                                sex = item.sex.Value,
                                signin = item.Signin,
                                isBindGoogle = item.isBindGoogle,
                                service = item.isService.Value,
                                uid = item.uID.ToString(),
                                friend = item.friends.ObjToInt(),
                                weekly = item.weeklyMoney.Value,
                                rp = item.RP.Value,
                                lprofit = item.LProfit.Value,
                                rprofit = item.RProfit.Value,
                                ison = item.isSon,
                                jid = item.jid,
                                tid = item.tid,
                                isDelete = item.isDelete,
                                phone=item.userphone,
                                urealname = item.UrealName
                            }).ToList<dynamic>()
                }
            };

        }


        //获取GetUserInfoWeek
        [Route("GetUserInfoWeek")]
        public async Task<MessageModel<dynamic>> GetUserInfoWeek()
        {

            if (_user.ID == 0)
            {
                return new MessageModel<dynamic>()
                {
                    success = false,
                    code = 403,
                    msg = "",
                };
            }
            StringBuilder buider = new StringBuilder();
            buider.AppendLine("SELECT COUNT(a.uID) total,b.maintotal,c.sontotal,d.daymaintotal,e.daysontotal,f.weekmaintotal,g.weeksontotal FROM dbo.sysUserInfo a ");
            buider.AppendLine("LEFT JOIN (SELECT COUNT(uID) maintotal FROM dbo.sysUserInfo WHERE isSon=0) b ON 1=1");
            buider.AppendLine("LEFT JOIN (SELECT COUNT(uID) sontotal FROM dbo.sysUserInfo WHERE isSon=1) c ON 1=1");
            buider.AppendLine("LEFT JOIN (SELECT COUNT(uID) daymaintotal FROM dbo.sysUserInfo WHERE isSon=0 AND CAST(uCreateTime AS date)=CAST(GETDATE() AS DATE)) d ON 1=1");
            buider.AppendLine("LEFT JOIN (SELECT COUNT(uID) daysontotal FROM dbo.sysUserInfo WHERE isSon=1 AND CAST(uCreateTime AS date)=CAST(GETDATE() AS DATE)) e ON 1=1");
            buider.AppendLine("LEFT JOIN (SELECT COUNT(uID) weekmaintotal FROM dbo.sysUserInfo WHERE isSon=0 AND CAST(uCreateTime AS DATE)>=CAST(DATEADD(wk, DATEDIFF(wk,0,getdate()), 0) AS DATE) ) f ON 1=1");
            buider.AppendLine("LEFT JOIN (SELECT COUNT(uID) weeksontotal FROM dbo.sysUserInfo WHERE isSon=1 AND CAST(uCreateTime AS date)>=CAST(DATEADD(wk, DATEDIFF(wk,0,getdate()), 0) AS DATE) ) g ON 1=1");
            buider.AppendLine("GROUP BY  b.maintotal,c.sontotal,d.daymaintotal,e.daysontotal,f.weekmaintotal,g.weeksontotal");

            var result = await _userInfoServices.QueryTable(buider.ToString());


            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = new
                {
                    list = new
                    {
                        total = result.Rows[0]["total"],
                        maintotal = result.Rows[0]["maintotal"],
                        sontotal = result.Rows[0]["sontotal"],
                        daymaintotal = result.Rows[0]["daymaintotal"],
                        daysontotal = result.Rows[0]["daysontotal"],
                        weekmaintotal = result.Rows[0]["weekmaintotal"],
                        weeksontotal = result.Rows[0]["weeksontotal"]
                    }
                }
            };
        }


    }
}