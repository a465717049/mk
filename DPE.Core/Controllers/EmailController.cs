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
    ///邮箱 银币管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/Email")]
    [Authorize(Permissions.Name)]
    public class EmailController : Controller
    {

        private readonly IEMailServices _iemailservices;
        private readonly IUserEmailServices _iuseremailservices;
        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;

        public EmailController(IUserInfoServices iuserinfoservices, IUser user, IEMailServices iemailservices, IUserEmailServices iuseremailservices)
        {
            _iemailservices = iemailservices;
            this._user = user;
            _userInfoServices = iuserinfoservices;
            _iuseremailservices = iuseremailservices;
        }


        /// <summary>
        /// 邮件列表
        /// <param name="language">cn en</param>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEamilList")]
        public async Task<MessageModel<EmailListViewModels>> GetEamilList(string language = "cn")
        {
            var emaillist  = await _iemailservices.QuerySql(string.Format("select  a.* from Email a where not  exists (select 1 from UserEmail b where a.id=b.eid and b.isDelete=1 and b.uid={0}) ", _user.ID));

            return new MessageModel<EmailListViewModels>()
            {
                success = true,
                msg = "",
                response = new EmailListViewModels()
                {
                    list = (from item in emaillist
                            orderby item.createTime
                            select new EmailListViewModelsList
                            {
                                content = item.context,
                                id = item.id,
                                title = item.title,
                                status =  _iuseremailservices.ckUserEmailStatus(_user.ID,item.id).Result.ObjToInt(),
                                time = DateHelper.GetCreatetime(Convert.ToDateTime(item.createTime)),
                                source = "系统"
                            }).ToList()
                }
            };
        }

        /// <summary>
        /// 邮件查看
        /// </summary>
        /// <param name="id">邮件id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmailInfo")]
        public async Task<MessageModel<dynamic>> GetEmailInfo(int id)
        {
            //_user.ID
            //     var user = await _userInfoServices.GetUserInfo(_user.ID);
            //_user.ID
            var email = await _iuseremailservices.Query(x => x.eid == id && x.isDelete != 1);

            int tmpcode = 0;


            UserEmail userEmail = new UserEmail() { eid=0,status=0, isDelete=0, uId=0 };
            if (email.Count == 0 ) 
            {
                if (_user.ID != 0)
                {
                    userEmail.eid = id;
                    userEmail.status = 1;
                    userEmail.isDelete = 0;
                    userEmail.uId = _user.ID;
                    await _iuseremailservices.Add(userEmail);
                }
                else 
                {
                    tmpcode = 8001;
                }
            
            }
            else
            {
                userEmail = email.First();
                userEmail.status = 1;
                //已讀
                await  _iuseremailservices.Update(userEmail," uId="+_user.ID);
            }

            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = new
                {
                    code = tmpcode,
                    id = userEmail.eid,
                    status = userEmail.status
                }
            };
        }


        /// <summary>
        /// 邮件删除
        /// </summary>
        /// <param name="id">邮件id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteEmail")]
        public async Task<MessageModel<dynamic>> DeleteEmail(int id)
        {
            
            var isfig = await _iuseremailservices.DeleteEmail(id,_user.ID);
            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                code=0,
                response = new
                {
                    id = id
                }
            };

        }

        /// <summary>
        /// 一件邮件删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteEmailAll")]
        public async Task<MessageModel<dynamic>> DeleteEmailAll()
        {
            var user = await _userInfoServices.GetUserInfo(_user.ID);

            var myemaillist = (await _iuseremailservices.Query(x=>x.uId==_user.ID)).Select(x=>x.eid);

            var emaillist = (await _iemailservices.Query(x => !(myemaillist).Contains(x.id))).ToList();

            List<UserEmail> list = new List<UserEmail>();
            foreach (var item in emaillist)
            {
                list.Add(new UserEmail() { eid = item.id, isDelete = 1, status = 1 , uId=_user.ID }) ;

            }
            await  _iuseremailservices.Add(list);
            var isfig = await _iuseremailservices.DeleteEmailAll(user.uID);
            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                code = 0
            };

        }


    }
}
