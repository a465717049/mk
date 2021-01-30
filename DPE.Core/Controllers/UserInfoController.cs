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
using OfficeOpenXml;
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

        readonly IDownExcelRecordServices _idownexcelrecordservices;
        /// <summary>
        /// 构造函数注入
        /// </summary>

        public UserInfoController(ISysUserInfoServices isysuserinfoservices,IEPServices EPServices, IUserInfoServices userInfoServices, 
            IEPexchangeServices ePexchangeServices, ISPexchangeServices spexchangeServices,
            PermissionRequirement requirement, IUser user, IDownExcelRecordServices idownexcelrecordservices)
        {
            _userInfoServices = userInfoServices;
            _user = user;
            _requirement = requirement;
            _EPServices = EPServices;
            _EPexchangeServices = ePexchangeServices;
            _SPexchangeServices = spexchangeServices;
            _isysuserinfoservices = isysuserinfoservices;
            _idownexcelrecordservices=idownexcelrecordservices;
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
            string ulevel = "", string uhonur = "", string ustatus = "", string startdate = "", string enddate = "", string stid = "", string etid = "", string sjid = "", string ejid = "")
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
            var user = await _userInfoServices.GetAllUserInfo(pageindex, pagesize, key, utid, ulevel,uhonur,ustatus,startdate,enddate, "uID", stid, etid, sjid, ejid);

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


        [HttpPost]
        [Route("GetALLUserInfoExcel")]
        public async Task<IActionResult> GetALLUserInfoExcel()
        {
            try
            {

                string key = HttpContext.Request.Form["key"]; 
                string utid = HttpContext.Request.Form["utid"];
                string ulevel =  HttpContext.Request.Form["ulevel"];
                string uhonur =  HttpContext.Request.Form["uhonur"];
                string ustatus = HttpContext.Request.Form["ustatus"];
                string startdate = HttpContext.Request.Form["startdate"];
                string enddate = HttpContext.Request.Form["enddate"];

                string type = HttpContext.Request.Form["type"];

                string stid = HttpContext.Request.Form["stid"];
                string etid = HttpContext.Request.Form["etid"];
                string sjid = HttpContext.Request.Form["sjid"];
                string ejid = HttpContext.Request.Form["ejid"];
                string jwtStr = string.Empty;

                var user = await _userInfoServices.GetAllUserInfo(1, 999999, key, utid, ulevel, uhonur, ustatus, startdate, enddate, "uID",stid,etid,sjid,ejid);

                if (string.IsNullOrEmpty(type))
                {
                    type = "会员列表";
                }
                else 
                {
                    if (type.Equals("tid"))
                    {
                        type = "安置业绩";
                    }
                   else if (type.Equals("jid"))
                    {
                        type = "推荐业绩";
                    }

                }

                var sWebRootFolder = Directory.GetCurrentDirectory() + @"//shopimg//downinfo//";
                var nowdate = DateTime.Now;
                Random rad = new Random();
                int value = rad.Next(1000, 10000);
                string sFileName = string.Format("{0}{1}{2}{3}_{4}.xlsx", type, nowdate.Year.ToString(), nowdate.Month.ToString(), nowdate.Day.ToString(), value.ToString());
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // 添加worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("worksheet");
                    //添加头
                    worksheet.Cells[1, 1].Value = "会员编号";
                    worksheet.Cells[1, 2].Value = "昵称";
                    worksheet.Cells[1, 3].Value = "注册日期";
                    worksheet.Cells[1, 4].Value = "职位";
                    worksheet.Cells[1, 5].Value = "等级";
                    worksheet.Cells[1, 6].Value = "推荐id";
                    worksheet.Cells[1, 7].Value = "安置id";
                    worksheet.Cells[1, 8].Value = "EP";
                    worksheet.Cells[1, 9].Value = "RP";
                    worksheet.Cells[1, 10].Value = "DPE";
                    worksheet.Cells[1, 11].Value = "好友数";
                    worksheet.Cells[1, 12].Value = "L";
                    worksheet.Cells[1, 13].Value = "R";
                    worksheet.Cells[1, 14].Value = "状态";


                    for (int i = 0; i < user.data.Count(); i++)
                    {
                        int j = i + 2;
                        worksheet.Cells[j, 1].Value = user.data[i].uID;
                        worksheet.Cells[j, 2].Value = user.data[i].uNickName;
                        worksheet.Cells[j, 3].Value = user.data[i].uCreateTime.ToString("yyyy-MM-dd hh:mm:ss");
                        string us = "";
                        if (user.data[i].honorLevel == 0) us = "会员";
                        if (user.data[i].honorLevel == 1) us = "经理";
                        if (user.data[i].honorLevel == 2) us = "总监";
                        if (user.data[i].honorLevel == 3) us = "总裁";
                        if (user.data[i].honorLevel == 4) us = "董事";
                        if (user.data[i].honorLevel == 5) us = "合伙人";
                        worksheet.Cells[j, 4].Value = us;
                        string lv = "";
                        if (user.data[i].uStatus == 1) lv = "初级会员";
                        if (user.data[i].uStatus == 2) lv = "中级会员";
                        if (user.data[i].uStatus == 3) lv = "高级会员";
                        worksheet.Cells[j, 5].Value = lv;
                        worksheet.Cells[j, 6].Value = user.data[i].tid;
                        worksheet.Cells[j, 7].Value = user.data[i].jid;
                        worksheet.Cells[j, 8].Value = user.data[i].EP;
                        worksheet.Cells[j, 9].Value = user.data[i].RP;
                        worksheet.Cells[j, 10].Value = user.data[i].DPE;
                        worksheet.Cells[j, 11].Value = user.data[i].friends;
                        worksheet.Cells[j, 12].Value = user.data[i].LProfit;
                        worksheet.Cells[j, 13].Value = user.data[i].RProfit;
                        worksheet.Cells[j, 14].Value = user.data[i].isDelete==0? "正常":"锁定";

                    }
                    package.Save();
                    await _idownexcelrecordservices.Add(new DownExcelRecord() { downdate = DateTime.Now, downname = sFileName, isdelete = false, downtype = "userout" });
                }
                var returnFile = File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                returnFile.FileDownloadName = sFileName;

                return returnFile;

            }
            catch (Exception ex)
            {
                return null;
            }

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