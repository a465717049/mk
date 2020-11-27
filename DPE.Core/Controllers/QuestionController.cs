using System;
using System.Collections.Generic;
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
    [Route("api/Question")]
    [Authorize(Permissions.Name)]
    public class QuestionController : Controller
    {

        private readonly IQuestionServices _iQuestionServices;
        private readonly IAnswerServices _iAnswerServices;
        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;
        readonly ISysUserInfoServices _isysuserinfoservices;

        public QuestionController(ISysUserInfoServices isysuserinfoservices, IUser user, IUserInfoServices userInfoServices, IQuestionServices QuestionServices, IAnswerServices AnswerServices)
        {
            this._user = user;
            this._userInfoServices = userInfoServices;
            _iQuestionServices = QuestionServices;
            _iAnswerServices = AnswerServices;
            _isysuserinfoservices = isysuserinfoservices;
        }

        /// <summary>
        /// 获取我的问题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMyQuestion")]
        public async Task<MessageModel<dynamic>> GetMyQuestion()
        {
            //_user.ID
            var answer = await _iAnswerServices.Query(a => a.uID == _user.ID);

            if (answer.Count > 0)
            {
                Random rd = new Random();
                int a = rd.Next(0, answer.Count);
                var question = await _iQuestionServices.QueryById(answer[a].qID);

                dynamic d = new
                {
                    qId = answer[a].qID,
                    question = question.Question_CN

                };
                return new MessageModel<dynamic>()
                {
                    success = true,
                    msg = "",
                    response = d

                };
            }
            else
            {
                return new MessageModel<dynamic>()
                {
                    success = false,
                    msg = "",
                    code = 50004

                };
            }

        }
        [HttpPost]
        [Route("GetMyQuestionList")]
        public async Task<MessageModel<dynamic>> GetMyQuestionList()
        {
            //_user.ID
            var answer = await _iAnswerServices.Query(a => a.uID == _user.ID);
            object[] ids = new object[answer.Count];
            for (int i = 0; i < answer.Count; i++)
            {
                ids[i] = answer[i].qID;
            }
            var question = await _iQuestionServices.QueryByIDs(ids);

            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = question

            };
        }
        [HttpPost]
        [Route("GetAllQuestionList")]
        public async Task<MessageModel<dynamic>> GetAllQuestionList()
        {

            var question = await _iQuestionServices.Query();

            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = question

            };
        }


        [HttpPost]
        [Route("CheckQuestionAll")]
        public async Task<MessageModel<dynamic>> CheckQuestionAll(long uid, string answerslist)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var ss = JsonHelper.ParseFormByJson<List<Answer>>(answerslist);
                if (ss.Count <= 0)
                {
                    result.code = 60002;
                    result.msg = "請重新提交!";
                    result.success = false;
                    return result;
                }


                var qisfig = await _iAnswerServices.Query(x => x.uID == uid);

                if (qisfig.Count < 3)
                {
                    result.code = 60002;
                    result.msg = "請先完善安全問題!";
                    result.success = false;
                    return result;
                }

                var resultlist = qisfig.Where(a => !ss.Exists(t => a.qID == t.qID && a.answer == MD5Helper.MD5Encrypt32(t.answer))).ToList();

                if (resultlist.Count > 0)
                {
                    result.code = 60002;
                    result.msg = "驗證失敗，請重新輸入";
                    result.success = false;
                    return result;
                }

                var userinfo = await _isysuserinfoservices.QueryById(uid);
                userinfo.uLoginPWD = MD5Helper.MD5Encrypt32("123456");
                await _isysuserinfoservices.Update(userinfo);

                return result;

            }
            catch
            {
                result.code = 60001;
                result.msg = "请稍后再试!";
                result.success = false;
                return result;
            }

        }




        [HttpPost]
        [Route("GetQuestionListByID")]
        public async Task<MessageModel<dynamic>> GetQuestionListByID(long uid)
        {
            //_user.ID
            var answer = await _iAnswerServices.Query(a => a.uID == uid);
            object[] ids = new object[answer.Count];
            for (int i = 0; i < answer.Count; i++)
            {
                ids[i] = answer[i].qID;
            }
            var question = await _iQuestionServices.QueryByIDs(ids);

            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = question

            };
        }

        [HttpPost]
        [Route("SetQuestionList")]
        public async Task<MessageModel<dynamic>> SetQuestionList(string tpwd, string idcard, string answerslist)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var ss = JsonHelper.ParseFormByJson<List<Answer>>(answerslist);

                var qisfig = await _iQuestionServices.Query();
                if (ss.Count < 3)
                {
                    result.code = 60002;
                    result.msg = "請完善后提交!";
                    result.success = false;
                    return result;
                }

                var tranpwd = await _isysuserinfoservices.checkTrad(_user.ID, tpwd);
                if (tranpwd == null)
                {
                    //交易密码错误
                    result.code = 61004;
                    result.success = false;
                    result.msg = "交易密碼錯誤";
                    return result;
                }

                var idcardisfig = await _isysuserinfoservices.Query(x => x.uID == _user.ID && x.IDNumber == MD5Helper.MD5Encrypt32(idcard));
                if (idcardisfig.Count <= 0)
                {
                    //交易密码错误
                    result.code = 61004;
                    result.success = false;
                    result.msg = "身份證校驗失敗";
                    return result;
                }


                var addisfg = await _iAnswerServices.Query(x => x.uID == _user.ID);
                if (addisfg.Count == 3)
                {
                    int index = 0;
                    foreach (Answer answer in ss)
                    {
                        var tmpmodel = addisfg[index];
                        tmpmodel.answer = MD5Helper.GenerateMD5(answer.answer);
                        tmpmodel.qID = answer.qID;
                        tmpmodel.createTime = DateTime.Now;
                        await _iAnswerServices.Update(tmpmodel);
                        index++;
                    }
                }
                else
                {
                    foreach (Answer item in addisfg)
                    {
                        await _iAnswerServices.DeleteAnswer(item);
                    }
                    foreach (Answer answer in ss)
                    {
                        answer.uID = _user.ID;
                        answer.answer = MD5Helper.GenerateMD5(answer.answer);
                        answer.createTime = DateTime.Now;
                        await _iAnswerServices.Add(answer);
                    }
                }
                return result;

            }
            catch
            {
                result.code = 60001;
                result.msg = "請稍後再試!";
                result.success = false;
                return result;
            }
        }
    }
}