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
        private readonly IUserDataServices _iuserdataservices;

        public RelationController(IUnitOfWork unitOfWork, IUser iuser, ISysUserInfoServices sysUserInfoServices, IUserInfoServices userInfoServices, IUserDataServices iuserdataservices)
        {
            _unitOfWork = unitOfWork;
            _sysUserInfoServices = sysUserInfoServices;
            _userInfoServices = userInfoServices;
            _user = iuser;
            _iuserdataservices = iuserdataservices;
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

                List<showfriend> list = new List<showfriend>();
                for (int i = 0; i < friendData.Rows.Count; i++)
                {
                    showfriend model = new showfriend();
                    model.uID = Convert.ToInt64(friendData.Rows[i]["uID"]);
                    model.NickName = friendData.Rows[i]["NickName"].ToString();
                    model.photo = friendData.Rows[i]["photo"].ToString();
                    model.friendsNum =_sysUserInfoServices.GetFriendsList(model.uID, 0).Result.Rows.Count;
                    model.performance = _iuserdataservices.QueryById(model.uID).Result.RProfit.ToString();
                    var relationData = await _sysUserInfoServices.GetRelationListbyid(model.uID);
                    int hynum = 0;
                    int jlnum = 0;
                    int zjnum = 0;
                    int zcnum = 0;
                    int dsnum = 0;
                    int hhrnum = 0;
                    for (int j = 0; j < relationData.Rows.Count; j++)
                    {
                        List<string> ss = new List<string>();
                        string index = relationData.Rows[j]["honorLevel"].ToString();
                        if (index.Equals("0"))
                        {
                            hynum = Convert.ToInt32(relationData.Rows[j]["qty"]);
                        }
                        else if (index.Equals("1"))
                        {
                            jlnum = Convert.ToInt32(relationData.Rows[j]["qty"]);
                        }
                        else if (index.Equals("2"))
                        {
                            zjnum = Convert.ToInt32(relationData.Rows[j]["qty"]);
                        }
                        else if (index.Equals("3"))
                        {
                            zcnum = Convert.ToInt32(relationData.Rows[j]["qty"]);
                        }
                        else if (index.Equals("4"))
                        {
                            dsnum = Convert.ToInt32(relationData.Rows[j]["qty"]);
                        }
                        else if (index.Equals("5"))
                        {
                            hhrnum = Convert.ToInt32(relationData.Rows[j]["qty"]);
                        }
                        ss.Add("会员" + hynum + "个");
                        ss.Add("经理" + jlnum + "个");
                        ss.Add("总监" + zjnum + "个");
                        ss.Add("总裁" + zcnum + "个");
                        ss.Add("董事" + dsnum + "个");
                        ss.Add("合伙人" + hhrnum + "个");
                        model.honors = ss;
                    }
                    list.Add(model);
                    
                }
                result.code = 0;
                result.success = true;
                result.response = list;

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
        public async Task<MessageModel<dynamic>> GetFriendsListJid(long uid)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();

            try
            {
                var friendData = await _sysUserInfoServices.GetFriendsListJid(_user.ID, uid);

                List<showfriend> list = new List<showfriend>();
                for (int i = 0; i < friendData.Rows.Count; i++)
                {
                    showfriend model = new showfriend();
                    model.uID = Convert.ToInt64(friendData.Rows[i]["uID"]);
                    model.NickName = friendData.Rows[i]["NickName"].ToString();
                    model.photo = friendData.Rows[i]["photo"].ToString();
                    model.friendsNum = _sysUserInfoServices.GetFriendsListJid(model.uID, 0).Result.Rows.Count;
                    model.performance = _iuserdataservices.QueryById(model.uID).Result.LProfit.ToString();
                    var relationData = await _sysUserInfoServices.GetRelationListbyid(model.uID);
                    int hynum = 0;
                    int jlnum = 0;
                    int zjnum = 0;
                    int zcnum = 0;
                    int dsnum = 0;
                    int hhrnum = 0;
                    for (int j = 0; j < relationData.Rows.Count; j++)
                    {
                        List<string> ss = new List<string>();
                        string index = relationData.Rows[j]["honorLevel"].ToString();
                        if (index.Equals("0"))
                        {
                            hynum = Convert.ToInt32(relationData.Rows[j]["qty"]);
                        }
                        else if (index.Equals("1"))
                        {
                            jlnum = Convert.ToInt32(relationData.Rows[j]["qty"]);
                        }
                        else if (index.Equals("2"))
                        {
                            zjnum = Convert.ToInt32(relationData.Rows[j]["qty"]);
                        }
                        else if (index.Equals("3"))
                        {
                            zcnum = Convert.ToInt32(relationData.Rows[j]["qty"]);
                        }
                        else if (index.Equals("4"))
                        {
                            dsnum = Convert.ToInt32(relationData.Rows[j]["qty"]);
                        }
                        else if (index.Equals("5"))
                        {
                            hhrnum = Convert.ToInt32(relationData.Rows[j]["qty"]);
                        }
                        ss.Add("会员" + hynum + "个");
                        ss.Add("经理" + jlnum + "个");
                        ss.Add("总监" + zjnum + "个");
                        ss.Add("总裁" + zcnum + "个");
                        ss.Add("董事" + dsnum + "个");
                        ss.Add("合伙人" + hhrnum + "个");
                        model.honors = ss;
                    }
                    list.Add(model);

                }
                result.code = 0;
                result.success = true;
                result.response = list;

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


        [HttpPost]
        public async Task<MessageModel<dynamic>> GetFriendsListbyId(long uid)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();

            try
            {
                var relationData = await _sysUserInfoServices.GetRelationListbyid(uid);

                var friendData = await _sysUserInfoServices.GetFriendsList(uid, 0);
                if (friendData == null || friendData.Rows.Count <= 0)
                {
                    result.code = 63001;
                    result.success = false;
                    return result;
                }

                result.code = 0;
                result.success = true;
                result.response = new { friendData= friendData, relationData= relationData };

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
                if (uid == 0) uid = Convert.ToInt64( HttpContext.Request.Form["uid"]);

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

public class showfriend
{

    public string NickName { get; set; }

    public string photo { get; set; }

    public long uID { get; set; }

    public string nickname { get; set; }


    public int friendsNum { get; set; }
    public string performance { get; set; }
    public List<string> honors { get; set; }

}