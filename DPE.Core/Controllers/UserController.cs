using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DPE.Core.AuthHelper.OverWrite;
using DPE.Core.Common;
using DPE.Core.Common.Helper;
using DPE.Core.Common.HttpContextUser;
using DPE.Core.Common.HttpRestSharp;
using DPE.Core.Common.LogHelper;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.IServices;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using OracleInternal.Secure.Network;

namespace DPE.Core.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        readonly ISysUserInfoServices _sysUserInfoServices;
        readonly IUserRoleServices _userRoleServices;
        readonly IRoleServices _roleServices;
        private readonly IUser _user;
        private readonly ILogger<UserController> _logger;
        readonly IUserInfoServices _userInfoServices;
        readonly IQuestionServices _iquestionservices;
        readonly IAnswerServices _ianswerservices;
        readonly IUserFeedbackServices _iuserfeedbackservices;
        readonly IAdminReplyServices _iadminreplyservices;

        readonly IDisposeFeedbackServices _idisposefeedbackservices;

        readonly IAnswerServices _answerServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="userInfoServices"></param>
        /// <param name="iquestionservices"></param>
        /// <param name="ianswerservices"></param>
        /// <param name="iuserfeedbackservices"></param>
        /// <param name="iadminreplyservices"></param>
        public UserController(IUnitOfWork unitOfWork, ISysUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices, IRoleServices roleServices, IUser user, ILogger<UserController> logger, IUserInfoServices userInfoServices,
            IQuestionServices iquestionservices, IAnswerServices ianswerservices, IUserFeedbackServices iuserfeedbackservices, IAdminReplyServices iadminreplyservices,
            IDisposeFeedbackServices idisposefeedbackservices, IAnswerServices answerServices)
        {
            _unitOfWork = unitOfWork;
            _sysUserInfoServices = sysUserInfoServices;
            _userRoleServices = userRoleServices;
            _roleServices = roleServices;
            _user = user;
            _logger = logger;
            _userInfoServices = userInfoServices;
            _iquestionservices = iquestionservices;
            _ianswerservices = ianswerservices;
            _iuserfeedbackservices = iuserfeedbackservices;
            _iadminreplyservices = iadminreplyservices;
            _idisposefeedbackservices = idisposefeedbackservices;
            _answerServices = answerServices;
        }

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<MessageModel<PageModel<sysUserInfo>>> Get(int page = 1, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }
            int intPageSize = 50;


            var data = await _sysUserInfoServices.QueryPage(a => a.isDelete != true && a.uStatus >= 0 && ((a.uLoginName != null && a.uLoginName.Contains(key)) || (a.uRealName != null && a.uRealName.Contains(key)) || (a.uLoginName != null && a.uID.ToString().Contains(key))), page, intPageSize, " uID desc ");


            #region MyRegion

            // 这里可以封装到多表查询，此处简单处理
            var allUserRoles = await _userRoleServices.Query(d => d.IsDeleted == false);
            var allRoles = await _roleServices.Query(d => d.IsDeleted == false);

            var sysUserInfos = data.data;
            foreach (var item in sysUserInfos)
            {
                var currentUserRoles = allUserRoles.Where(d => d.UserId == item.uID).Select(d => d.RoleId).ToList();
                item.RIDs = currentUserRoles;
                item.RoleNames = allRoles.Where(d => currentUserRoles.Contains(d.Id)).Select(d => d.Name).ToList();
            }

            data.data = sysUserInfos;
            #endregion


            return new MessageModel<PageModel<sysUserInfo>>()
            {
                msg = "获取成功",
                success = data.dataCount >= 0,
                response = data
            };

        }

        // GET: api/User/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public string Get(string id)
        {
            _logger.LogError("test wrong");
            return "value";
        }

        // GET: api/User/5
        /// <summary>
        /// 获取用户详情根据token
        /// 【无权限】
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<sysUserInfo>> GetInfoByToken(string token)
        {
            var data = new MessageModel<sysUserInfo>();
            if (!string.IsNullOrEmpty(token))
            {
                var tokenModel = JwtHelper.SerializeJwt(token);
                if (tokenModel != null && tokenModel.Uid > 0)
                {
                    var userinfo = await _sysUserInfoServices.QueryById(tokenModel.Uid);
                    if (userinfo != null)
                    {
                        data.response = userinfo;
                        data.success = true;
                        data.msg = "获取成功";
                    }
                }

            }
            return data;
        }

        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="sysUserInfo"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] sysUserInfo sysUserInfo)
        {
            var data = new MessageModel<string>();

            sysUserInfo.uLoginPWD = MD5Helper.MD5Encrypt32(sysUserInfo.uLoginPWD);
            sysUserInfo.uRemark = _user.Name;

            var id = await _sysUserInfoServices.Add(sysUserInfo);
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        /// <summary>
        /// 更新用户与角色
        /// </summary>
        /// <param name="sysUserInfo"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] sysUserInfo sysUserInfo)
        {
            // 这里使用事务处理

            var data = new MessageModel<string>();
            try
            {
                _unitOfWork.BeginTran();

                if (sysUserInfo != null && sysUserInfo.uID > 0)
                {
                    if (sysUserInfo.RIDs.Count > 0)
                    {
                        // 无论 Update Or Add , 先删除当前用户的全部 U_R 关系
                        var usreroles = (await _userRoleServices.Query(d => d.UserId == sysUserInfo.uID)).Select(d => d.Id.ToString()).ToArray();
                        if (usreroles.Count() > 0)
                        {
                            var isAllDeleted = await _userRoleServices.DeleteByIds(usreroles);
                        }

                        // 然后再执行添加操作
                        var userRolsAdd = new List<UserRole>();
                        sysUserInfo.RIDs.ForEach(rid =>
                        {
                            userRolsAdd.Add(new UserRole(sysUserInfo.uID, rid));
                        });

                        await _userRoleServices.Add(userRolsAdd);

                    }

                    data.success = await _sysUserInfoServices.Update(sysUserInfo);

                    _unitOfWork.CommitTran();

                    if (data.success)
                    {
                        data.msg = "更新成功";
                        data.response = sysUserInfo?.uID.ObjToString();
                    }
                }
            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTran();
                _logger.LogError(e, e.Message);
            }

            return data;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _sysUserInfoServices.QueryById(id);
                userDetail.isDelete = true;
                data.success = await _sysUserInfoServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = userDetail?.uID.ObjToString();
                }
            }

            return data;
        }



        /// <summary>
        /// 设置男女
        /// 【无权限】
        /// </summary>
        /// <param name="sex"> 0 男 1 女</param>
        /// <returns>设置男女</returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> SetUserSex(int sex)
        {

            int tmpcode = 0;
            bool tmpsuccess = true;
            if (_user.ID.ObjToInt() == 0)
            {
                tmpcode = 1003;
                tmpsuccess = false;
            }

            if (tmpsuccess)
            {
                var isfig = await _sysUserInfoServices.SetUserInfoSex(_user.ID, sex);
            }
            else if (tmpcode != 1003)
            {
                tmpcode = 31001;
                tmpsuccess = false;
            }

            return new MessageModel<dynamic>()
            {
                success = tmpsuccess,
                msg = "",
                code = tmpcode,
                response = new { sex = sex }
            };


        }

        /// <summary>
        ///  设置名字
        /// 【无权限】
        /// </summary>
        /// <param name="nickname">名字</param>
        /// <returns>设置名字</returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> SetUserName(string nickname)
        {
            int tmpcode = 0;
            bool tmpsuccess = true;
            if (_user.ID.ObjToInt() == 0)
            {
                tmpcode = 1003;
                tmpsuccess = false;
            }

            if (tmpsuccess)
            {
                var isfig = await _sysUserInfoServices.SetUserInfoName(_user.ID, nickname);
            }
            else
            {
                tmpcode = 32001;
                tmpsuccess = false;
            }

            return new MessageModel<dynamic>()
            {
                success = tmpsuccess,
                msg = "",
                code = tmpcode,
                response = new { nickname = nickname }
            };


        }

        /// <summary>
        /// 设置金币地址 设置钱包地址
        /// 【无权限】
        /// </summary>
        /// <param name="uid">uid</param>
        /// <param name="location">金币地址</param>
        /// <returns>设置金币地址</returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> SetUserlocation(string location, string addr)
        {

            int tmpcode = 0;
            bool tmpsuccess = true;
            if (_user.ID.ObjToInt() == 0)
            {
                tmpcode = 1003;
                tmpsuccess = false;
            }

            if (tmpsuccess)
            {
                var isfig = await _sysUserInfoServices.SetUserInfolocation(_user.ID, location, addr);
            }
            else if (tmpcode != 1003)
            {
                tmpcode = 33001;
                tmpsuccess = false;
            }

            return new MessageModel<dynamic>()
            {
                success = tmpsuccess,
                msg = "",
                code = tmpcode,
                response = new { location = location }
            };

        }

        [HttpPost]
        public async Task<MessageModel<dynamic>> SetUserlocationweb(string location, string addr, string tpwd, string idnum, string google)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                //判断交易密码
                var tranpwd = await _sysUserInfoServices.checkTrad(_user.ID, tpwd);
                if (tranpwd == null)
                {
                    //交易密码错误
                    result.code = 61004;
                    result.success = false;
                    result.msg = "交易密码错误";
                    return result;
                }

                var idcarduser = await _sysUserInfoServices.Query(x => x.uID == _user.ID && x.IDNumber == MD5Helper.MD5Encrypt32(idnum));
                if (idcarduser.Count == 0)
                {
                    result.code = 10001;
                    result.msg = "身份证号码校验失败！";
                    result.success = false;
                    return result;
                }


                // 判断谷歌验证
                var trangode = await _sysUserInfoServices.checkGoogleKey(_user.ID, google);
                if (trangode == null)
                {
                    //谷歌验证错误
                    result.code = 61005;
                    result.success = false;
                    result.msg = "谷歌验证码校验失败";
                    return result;
                }

                int tmpcode = 0;
                bool tmpsuccess = true;
                if (_user.ID.ObjToInt() == 0)
                {
                    tmpcode = 1003;
                    result.msg = "当前用户已经失效，请重新登陆！";
                    tmpsuccess = false;
                    return result;
                }

                if (tmpsuccess)
                {
                    var isfig = await _sysUserInfoServices.SetUserInfolocation(_user.ID, location, addr);
                }
                else if (tmpcode != 1003)
                {
                    tmpcode = 33001;
                    result.msg = "修改失败，请重试！";
                    tmpsuccess = false;
                    return result;
                }

                return result;
            }
            catch
            {
                result.msg = "修改失败，请重试！";
                result.success = false;
                return result;
            }
        }

        /// <summary>
        /// 设置农场主
        /// 【无权限】
        /// </summary>
        /// <returns>设置农场主</returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> SetUseruStatus()
        {
            int tmpcode = 0;
            bool tmpsuccess = true;
            if (_user.ID.ObjToInt() == 0)
            {
                tmpcode = 1003;
                tmpsuccess = false;
            }

            if (tmpsuccess)
            {
                var user = await _userInfoServices.GetUserInfo(_user.ID);
                var isfig = await _sysUserInfoServices.SetUserInfouStatus(user.uID, 1000);
            }
            else if (tmpcode != 1003)
            {
                tmpcode = 30001;
                tmpsuccess = false;
            }

            return new MessageModel<dynamic>()
            {
                success = tmpsuccess,
                msg = "",
                code = tmpcode
            };
        }


        /// <summary>
        /// 修改密码
        /// 【无权限】
        /// </summary>
        /// <param name="password">原密码</param>
        /// <param name="new_password">新密码</param>
        /// <returns>修改密码</returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> Resetpwd(string password, string new_password)
        {

            if (password.Equals(new_password))
            {

            }
            var user = await _sysUserInfoServices.Query(d => d.uLoginName == _user.Name && d.uLoginPWD == MD5Helper.MD5Encrypt32(password) && d.isDelete == false);
            if (user.Count > 0)
            {

                var resultuser = user.First();
                resultuser.uLoginPWD = MD5Helper.MD5Encrypt32(new_password);
                await _sysUserInfoServices.Update(resultuser);

            }
            else
            {
                return new MessageModel<dynamic>()
                {
                    success = false,
                    msg = "",
                    code = 1005,
                    response = new
                    {
                        new_password = new_password
                    }
                };
            }
            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = new
                {
                    new_password = new_password
                }
            };
        }



        /// <summary>
        /// 实名认证
        /// 【无权限】
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="ide">身份证号</param>
        /// <returns>实名认证</returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> SetUseridcard(string name, string ide)
        {

            MessageModel<dynamic> resuldata = new MessageModel<dynamic>();
            var x = await _sysUserInfoServices.Query(a => a.IDNumber == MD5Helper.MD5Encrypt32(ide));
            if (x.Count > 0)
            {
                resuldata.code = 1007;
                resuldata.success = false;
                return resuldata;
            }

            if (CheckVeridenNo.ckidcard(ide, name))
            {
                var user = await _sysUserInfoServices.QueryById(_user.ID);
                user.IDNumber = MD5Helper.MD5Encrypt32(ide);
                user.uRealName = name;
                user.isSetIDNumber = 1;
                await _sysUserInfoServices.Update(user);
            }
            else
            {
                resuldata.code = 1006;
                resuldata.success = false;
                return resuldata;
            }
            return resuldata;

        }



        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="new_password"></param>
        /// <param name="qid"></param>
        /// <param name="qanswer"></param>
        /// <param name="idcard"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> UpdatePassword(string password, int type, string new_password, int qid, string qanswer, string idcard)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            if (_user.ID == 0)
            {
                result.code = 10001;
                result.msg = "用户身份已过期，请重新登陆";
                result.success = false;
                return result;
            }
            if (password.Equals(new_password))
            {
                result.code = 10001;
                result.msg = "新密码和原密码不能相等";
                result.success = false;
                return result;
            }

            var idcarduser = await _sysUserInfoServices.Query(x => x.uID == _user.ID && x.IDNumber == MD5Helper.MD5Encrypt32(idcard));
            if (idcarduser.Count == 0)
            {
                result.code = 10001;
                result.msg = "身份证或者护照输入不正确请确认！";
                result.success = false;
                return result;
            }

            var answer = await _ianswerservices.Query(x => x.uID == _user.ID && x.answer.Equals(MD5Helper.MD5Encrypt32(qanswer)) && x.qID == qid);
            if (answer.Count == 0)
            {
                result.code = 10001;
                result.msg = "问题答案校验失败，请重新输入！";
                result.success = false;
                return result;
            }
            if (type == 0)
            {
                var user = await _sysUserInfoServices.Query(d => d.uID == _user.ID && d.uLoginPWD == MD5Helper.MD5Encrypt32(password) && d.isDelete == false);
                if (user.Count > 0)
                {

                    var resultuser = user.First();


                    resultuser.uLoginPWD = MD5Helper.MD5Encrypt32(new_password);

                    await _sysUserInfoServices.Update(resultuser);

                }
                else
                {
                    result.code = 10001;
                    result.msg = "原密码校验失败,请重新输入";
                    result.success = false;
                    return result;
                }
            }
            else
            {
                var user = await _sysUserInfoServices.Query(d => d.uID == _user.ID && d.uTradPWD == MD5Helper.MD5Encrypt32(password) && d.isDelete == false);
                if (user.Count > 0)
                {

                    var resultuser = user.First();


                    resultuser.uTradPWD = MD5Helper.MD5Encrypt32(new_password);

                    await _sysUserInfoServices.Update(resultuser);

                }
                else
                {
                    result.code = 10001;
                    result.msg = "原密码校验失败,请重新输入";
                    result.success = false;
                    return result;
                }
            }
            return result;

        }

        /// <summary>
        /// 获取所有密保问题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<QuestionViewModelList>> GetQuestion()
        {
            MessageModel<QuestionViewModelList> result = new MessageModel<QuestionViewModelList>();

            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }

                var questionData = await _iquestionservices.GetQuestions();
                if (questionData == null || questionData.Count <= 0)
                {
                    result.code = 50001;
                    result.success = false;
                    return result;
                }

                result.code = 0;
                result.success = true;
                result.response = new QuestionViewModelList();
                result.response.list = questionData;
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
        /// 删除密保问题
        /// </summary>
        /// <param name="id">问题id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> DeleteQuestion(int id)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                code = 0,
                response = null
            };
            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }

                var answerData = await _ianswerservices.GetAnswerByQid(id);
                if (answerData != null)
                {
                    result.success = false;
                    result.code = 51002;
                    return result;
                }

                var delQuestion = await _iquestionservices.DeleteQuestion(id);
                if (!delQuestion)
                {
                    result.success = false;
                    result.code = 51001;
                    return result;
                }

                result.success = true;
                result.code = 0;
                result.response = new
                {
                    msg = "删除成功"
                };
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
        /// 新增一条密保问题
        /// </summary>
        /// <param name="Question_CN">问题的中文描述</param>
        /// <param name="Question_EN">问题的英文描述</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> AddQuestion(string Question_CN, string Question_EN)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                code = 0,
                response = null
            };
            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }
                Question question = new Question()
                {
                    Question_CN = Question_CN,
                    Question_EN = Question_EN
                };

                var addQuestion = await _iquestionservices.AddQuestion(question);
                if (addQuestion <= 0)
                {
                    result.success = false;
                    result.code = 52001;
                    return result;
                }

                result.success = true;
                result.code = 0;
                result.response = new
                {
                    msg = "新增成功"
                };
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
        /// 新增或修改一条用户问题及答案
        /// </summary>
        /// <param name="qID">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> AddOrUpdateAnswer(int qID, string answer)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                code = 0,
                response = null
            };
            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }
                Answer answerObj = await _ianswerservices.GetAnswerByQid(uid, qID);

                if (answerObj != null)
                {
                    answerObj.answer = answer;
                    var updateResult = await _ianswerservices.UpdateAnswer(answerObj);
                    if (!updateResult)
                    {
                        result.success = false;
                        result.code = 53001;
                        return result;
                    }
                }
                else
                {
                    var addAnswer = await _ianswerservices.AddAnswer(new Answer()
                    {
                        uID = uid,
                        answer = answer,
                        createTime = DateTime.Now,
                        qID = qID
                    });
                    if (addAnswer <= 0)
                    {
                        result.success = false;
                        result.code = 53001;
                        return result;
                    }
                }

                result.success = true;
                result.code = 0;
                result.response = new
                {
                    msg = "操作成功"
                };
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
        /// 删除一条用户问题
        /// </summary>
        /// <param name="qID">问题id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> DeleteAnswer(int qID)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                code = 0,
                response = null
            };
            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }
                Answer answerObj = await _ianswerservices.GetAnswerByQid(uid, qID);
                if (answerObj == null)
                {
                    result.success = false;
                    result.code = 54001;
                    return result;
                }

                var delAnswer = await _ianswerservices.DeleteAnswer(answerObj);
                if (!delAnswer)
                {
                    result.success = false;
                    result.code = 54002;
                    return result;
                }

                result.success = true;
                result.code = 0;
                result.response = new
                {
                    msg = "删除成功"
                };
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
        /// 获取一条用户问题
        /// </summary>
        /// <param name="qID">问题id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<Answer>> GetAnswer(int qID)
        {
            MessageModel<Answer> result = new MessageModel<Answer>();
            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }
                Answer answerObj = await _ianswerservices.GetAnswerByQid(uid, qID);
                if (answerObj == null)
                {
                    result.success = false;
                    result.code = 54001;
                    return result;
                }

                result.success = true;
                result.code = 0;
                result.response = answerObj;
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
        /// 检查密保问题答案是否正确
        /// </summary>
        /// <param name="qID">问题id</param>
        /// <param name="answer">用户所填用户问题答案</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> CheckAnswer(int qID, string answer)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                code = 0,
                response = null
            };
            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }
                if (string.IsNullOrEmpty(answer))
                {
                    result.success = false;
                    result.code = 55001;
                    return result;
                }
                Answer answerObj = await _ianswerservices.GetAnswerByQid(uid, qID);
                if (answerObj != null)
                {
                    result.success = false;
                    result.code = 54001;
                    return result;
                }

                if (answerObj.answer != answer)
                {
                    result.success = false;
                    result.code = 55002;
                    return result;
                }

                result.success = true;
                result.code = 0;
                result.response = new
                {
                    msg = "答案正确"
                };
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
        /// 新增一条用户反馈
        /// </summary>
        /// <param name="content">反馈内容</param>
        /// <param name="imgUrl">图片地址(如果有)</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> AddUserFeedBack(string content, string imgUrl = "")
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                code = 0,
                response = null
            };
            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }
                if (string.IsNullOrEmpty(content))
                {
                    result.success = false;
                    result.code = 57001;
                    return result;
                }

                UserFeedback ufb = new UserFeedback();
                ufb.Message = content;
                ufb.MessageImgUrl = imgUrl;
                ufb.uId = uid;
                ufb.CreateTime = DateTime.Now;
                ufb.IsReply = 0;


                var addUFB = await _iuserfeedbackservices.InsertUserFeedBack(ufb);
                if (addUFB <= 0)
                {
                    result.success = false;
                    result.code = 57002;
                    return result;
                }

                result.success = true;
                result.code = 0;
                result.response = new
                {
                    msg = "新增成功"
                };
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

        //获取是否有未读消息
        [HttpPost]
        public async Task<MessageModel<dynamic>> GetReadUserFeedBack()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var data =await _idisposefeedbackservices.Query(x => x.MessageUid == _user.ID && x.IsRead==false);
                result.code = 0;
                result.msg = "";
                result.response = data.Count();
                return result;
            }
            catch
            {
                result.code = 4001;
                result.success = true;
                return result;
            }
        }


        [HttpPost]
        public async Task<MessageModel<dynamic>> SetReadUserFeedBack()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                var data = await _idisposefeedbackservices.Query(x => x.MessageUid == _user.ID && x.IsRead == false);
                foreach (DisposeFeedback model in data) 
                {
                    model.IsRead = true;
                    await _idisposefeedbackservices.Update(model);
                }
                result.code = 0;
                result.msg = "";
                result.response = data.Count();
                return result;
            }
            catch
            {
                result.code = 4001;
                result.success = true;
                return result;
            }
        }

        /// <summary>
        /// 修改用户反馈
        /// </summary>
        /// <param name="ufbId">反馈问题ID</param>
        /// <param name="content">反馈内容</param>
        /// <param name="imgUrl">图片地址(如果有)</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> UpdateUserFeedBack(int ufbId, string content, string imgUrl = "")
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                code = 0,
                response = null
            };
            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }
                if (string.IsNullOrEmpty(content))
                {
                    result.success = false;
                    result.code = 57001;
                    return result;
                }

                UserFeedback ufb = await _iuserfeedbackservices.GetUserFeedback(ufbId);
                if (ufb == null)
                {
                    result.success = false;
                    result.code = 58001;
                    return result;
                }
                if (ufb.uId != uid)
                {
                    result.success = false;
                    result.code = 58002;
                    return result;
                }

                ufb.Message = content;
                ufb.MessageImgUrl = imgUrl;


                var addUFB = await _iuserfeedbackservices.UpdateUserFeedBack(ufb);
                if (!addUFB)
                {
                    result.success = false;
                    result.code = 58003;
                    return result;
                }

                result.success = true;
                result.code = 0;
                result.response = new
                {
                    msg = "修改成功"
                };
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
        /// 删除用户反馈
        /// </summary>
        /// <param name="ufbId">反馈问题ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> DeleteUserFeedBack(int ufbId)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                code = 0,
                response = null
            };
            int errorCount = 0;
            _unitOfWork.BeginTran();
            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }

                UserFeedback ufb = await _iuserfeedbackservices.GetUserFeedback(ufbId);
                if (ufb == null)
                {
                    result.success = false;
                    result.code = 58001;
                    return result;
                }
                if (ufb.uId != uid)
                {
                    result.success = false;
                    result.code = 58002;
                    return result;
                }


                var addUFB = await _iuserfeedbackservices.DelUserFeedBack(ufbId);
                if (!addUFB) errorCount++;

                //
                var adminReply = await _iadminreplyservices.GetAdminReplies(ufbId);
                if (adminReply != null && adminReply.Count > 0)
                {
                    foreach (var item in adminReply)
                    {
                        var del = await _iadminreplyservices.DeleteAdminReply(item);
                        if (!del) errorCount++;
                    }
                }
                if (errorCount == 0)
                {
                    result.success = true;
                    result.code = 0;
                    result.response = new
                    {
                        msg = "删除成功"
                    };
                    _unitOfWork.CommitTran();
                }
                else
                {
                    result.success = false;
                    result.code = 58003;
                    _unitOfWork.RollbackTran();
                }


                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.code = 1007;
                result.msg = ex.Message;
                _unitOfWork.RollbackTran();
                return result;
            }

        }

        /// <summary>
        /// 获取用户的反馈问题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> GetUserFeedBack()
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
                var userfeedback = await _iuserfeedbackservices.Query(x => x.uId == _user.ID);
                var mangerfeedback = await _idisposefeedbackservices.Query(x => x.MessageUid == _user.ID);

                List<dynamic> list = new List<dynamic>();
                foreach (UserFeedback model in userfeedback)
                {
                    list.Add(new { id = model.Id, direction = "right", date = model.CreateTime, image = "/static/img/nick-1.f315d0d.png", msg = model.Message });
                }

                foreach (DisposeFeedback model in mangerfeedback)
                {
                    list.Add(new { id = model.id, direction = "left", date = model.CreateTime, image = "/static/img/nick-1.f315d0d.png", msg = model.Message });
                }

                result.response = list.OrderBy(t => t.date);

                return result;
            }
            catch
            {
                result.code = 70001;
                result.msg = "查询异常请稍后查询";
                result.success = false;
                return result;
            }
        }

        /// <summary>
        /// 新增一条管理员回复
        /// </summary>
        /// <param name="content">反馈内容</param>
        /// <param name="ufbId">反馈问题id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> AddAdminReply(int ufbId, string content)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                code = 0,
                response = null
            };
            int errorCount = 0;
            _unitOfWork.BeginTran();
            try
            {
                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }
                if (string.IsNullOrEmpty(content))
                {
                    result.success = false;
                    result.code = 57001;
                    return result;
                }


                UserFeedback ufb = await _iuserfeedbackservices.GetUserFeedback(ufbId);
                if (ufb == null)
                {
                    result.success = false;
                    result.code = 58001;
                    return result;
                }

                AdminReply ar = new AdminReply();
                ar.UserFeedbackId = ufbId;
                ar.ReplyMessage = content;
                ar.ReplyTime = DateTime.Now;


                var addAR = await _iadminreplyservices.AddAdminReply(ar);
                if (addAR <= 0) errorCount++;

                ufb.IsReply = 1;
                var updateUFB = await _iuserfeedbackservices.UpdateUserFeedBack(ufb);
                if (!updateUFB) errorCount++;

                if (errorCount == 0)
                {
                    result.success = true;
                    result.code = 0;
                    result.response = new
                    {
                        msg = "新增成功"
                    };
                    _unitOfWork.CommitTran();
                }
                else
                {
                    result.success = false;
                    result.code = 60001;
                    _unitOfWork.RollbackTran();
                }


                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.code = 1007;
                result.msg = ex.Message;
                _unitOfWork.RollbackTran();
                return result;
            }

        }

        /// <summary>
        /// 获取管理员的回复
        /// </summary>
        /// <param name="ufdId">用户反馈问题的id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<AdminReplyViewModelList>> GetAdminReply(int ufdId)
        {
            MessageModel<AdminReplyViewModelList> result = new MessageModel<AdminReplyViewModelList>();

            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }

                var arData = await _iadminreplyservices.GetAdminReplies(ufdId);
                if (arData == null || arData.Count <= 0)
                {
                    result.code = 61001;
                    result.success = false;
                    return result;
                }

                result.code = 0;
                result.success = true;
                result.response = new AdminReplyViewModelList();
                result.response.list = arData;

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


        //邀请码验证
        [HttpPost]
        public async Task<MessageModel<dynamic>> SetAuthenticator(string gcode, string tpwd)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();

            var tranuser = await _sysUserInfoServices.Query(x => x.uID == _user.ID && x.uTradPWD == MD5Helper.MD5Encrypt32(tpwd));
            try
            {


                if (tranuser == null)
                {
                    //交易密码错误
                    result.code = 61004;
                    result.success = false;
                    result.msg = "交易密码错误";
                    return result;
                }
                if (tranuser.Count == 0)
                {
                    //交易密码错误
                    result.code = 61004;
                    result.success = false;
                    result.msg = "交易密码错误";
                    return result;
                }

                var fuser = await _sysUserInfoServices.QueryById(tranuser.FirstOrDefault().tid);
                if (fuser == null)
                {
                    //当前账号不存在推荐人
                    result.code = 61005;
                    result.success = false;
                    result.msg = "当前账号不存在推荐人";
                    return result;
                }
                //var fgooglekey = MD5Helper.GenerateMD5(fuser.googleKey);
                //GoogleAuthenticator authenticator = new GoogleAuthenticator(30, fgooglekey);
                //string code = authenticator.GenerateCode();
                //string codebae32 = MD5Helper.ToString(Encoding.UTF8.GetBytes(tranuser.FirstOrDefault().googleKey)).Replace("=","");
                if (gcode.Equals(fuser.googleKey))
                {
                    //   result.response = new { code = codebae32 };
                    return result;
                }
                else
                {
                    //邀请码错误
                    result.code = 61005;
                    result.success = false;
                    result.msg = "邀请码不正确";
                    return result;
                }
            }
            catch (Exception)
            {

                result.code = -1;
                result.success = false;
                result.msg = "交易密码校验异常";
                return result; ;
            }
        }

        //生成Googlekey的64位
        [HttpPost]
        public async Task<MessageModel<dynamic>> SetAuthenticatorGooglekey()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();

            if (_user.ID == 0)
            {
                //身份验证过期请重新登陆
                result.code = 61005;
                result.success = false;
                result.msg = "身份验证过期请重新登陆";
                return result;
            }

            var user = await _sysUserInfoServices.QueryById(_user.ID);

            var fgooglekey = MD5Helper.GenerateMD5(user.googleKey);
            string codebae32 = MD5Helper.ToString(Encoding.UTF8.GetBytes(fgooglekey)).Replace("=", "");
            result.response = new { code = codebae32 };
            return result;
        }


        //生成新账号
        [HttpPost]
        public async Task<MessageModel<dynamic>> CreateNewAccount(string jsondata)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                //AddNewUserModel addmodel
                if (_user.ID == 0)
                {
                    result.code = 61005;
                    result.success = false;
                    result.msg = "身份验证过期请重新登陆";
                    return result;
                }
                var jsonInfo = JsonConvert.DeserializeObject<AddNewUserModel>(jsondata);
                jsonInfo.parentID = _user.ID;
                jsonInfo.loginPass = MD5Helper.MD5Encrypt32(jsonInfo.loginPass);
                jsonInfo.idNumber = MD5Helper.MD5Encrypt32(jsonInfo.idNumber);
                jsonInfo.googleCode = MD5Helper.GenerateUniqueText();
                jsonInfo.TradePass = MD5Helper.MD5Encrypt32("123456");

                var inforesult = await _sysUserInfoServices.AddSpCreatePayUser(jsonInfo);
                if (inforesult == null || inforesult.Rows.Count <= 0)
                {
                    result.code = 63001;
                    result.success = false;
                    result.msg = "添加失败，请稍后再试";
                    return result;
                }

                if (string.IsNullOrEmpty(inforesult.Rows[0][0].ToString()))
                {
                    result.code = 63002;
                    result.success = false;
                    result.msg = "添加失败，请稍后再试";
                    return result;
                }

                result.code = 0;
                result.success = true;
                result.response = inforesult;
                result.msg = inforesult.Rows[0][0].ToString();
                return result;
            }
            catch (Exception ex)
            {
                result.code = 63003;
                result.success = false;
                result.msg = ex.Message + "添加失败，请稍后再试";
                return result;
            }

        }


        //升级
        [HttpPost]
        public async Task<MessageModel<dynamic>> UpdateLevelWeb(int level)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                //AddNewUserModel addmodel
                if (_user.ID == 0)
                {
                    result.code = 61005;
                    result.success = false;
                    result.msg = "身份验证过期请重新登陆";
                    return result;
                }

                var inforesult = await _sysUserInfoServices.UpdateLevelByWeb(_user.ID, level);
                if (inforesult == null || inforesult.Rows.Count <= 0)
                {
                    result.code = 63001;
                    result.success = false;
                    result.msg = "操作失败，请稍后再试";
                    return result;
                }

                if (string.IsNullOrEmpty(inforesult.Rows[0][0].ToString()))
                {
                    result.code = 63002;
                    result.success = false;
                    result.msg = "操作失败，请稍后再试";
                    return result;
                }


                result.code = 0;
                result.success = true;
                result.response = inforesult;
                result.msg = inforesult.Rows[0][0].ToString();
                return result;
            }
            catch (Exception ex)
            {
                result.code = 63003;
                result.success = false;
                result.msg = ex.Message + "添加失败，请稍后再试";
                return result;
            }

        }

        //生成子账号
        [HttpPost]
        public async Task<MessageModel<dynamic>> CreateSubAccount(long uid, int amount, int area, string tpwd)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                //AddNewUserModel addmodel
                if (_user.ID == 0)
                {
                    result.code = 61005;
                    result.success = false;
                    result.msg = "身份验证过期请重新登陆";
                    return result;
                }

                //判断交易密码
                var tranpwd = await _sysUserInfoServices.checkTrad(_user.ID, tpwd);
                if (tranpwd == null)
                {
                    //交易密码错误
                    result.code = 61004;
                    result.success = false;
                    result.msg = "交易密码错误!";
                    return result;
                }

                var inforesult = await _sysUserInfoServices.AddSpCreateSubUser(_user.ID, uid, amount, area);
                if (inforesult == null || inforesult.Rows.Count <= 0)
                {
                    result.code = 63001;
                    result.success = false;
                    result.msg = "添加失败，请稍后再试";
                    return result;
                }


                if (string.IsNullOrEmpty(inforesult.Rows[0][0].ToString()))
                {
                    result.code = 63002;
                    result.success = false;
                    result.msg = "添加失败，请稍后再试";
                    return result;
                }

                result.code = 0;
                result.success = true;
                result.msg = inforesult.Rows[0][0].ToString();
                return result;
            }
            catch
            {
                result.code = 63006;
                result.success = false;
                result.msg = "添加失败，请稍后再试";
                return result;
            }

        }


        //一键归集
        [HttpPost]
        public async Task<MessageModel<dynamic>> OneKeyReturn()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                //AddNewUserModel addmodel
                if (_user.ID == 0)
                {
                    result.code = 61005;
                    result.success = false;
                    result.msg = "身份验证过期请重新登陆";
                    return result;
                }


                var inforesult = await _sysUserInfoServices.UpdateOneKeyReturn(_user.ID);
                //if (inforesult == null || inforesult.Rows.Count <= 0)
                //{
                //    result.code = 63001;
                //    result.success = false;
                //    result.msg = "添加失败，请稍后再试";
                //    return result;
                //}
                //if (string.IsNullOrEmpty(inforesult.Rows[0][0].ToString()))
                //{
                //    result.code = 63002;
                //    result.success = false;
                //    result.msg = "添加失败，请稍后再试";
                //    return result;
                //}

                result.code = 0;
                result.success = true;
                result.msg = inforesult.Rows[0][0].ToString();
                return result;
            }
            catch
            {
                result.code = 63006;
                result.success = false;
                result.msg = "添加失败，请稍后再试";
                return result;
            }

        }


        //Google验证码校验
        [HttpPost]
        public async Task<MessageModel<dynamic>> CheckGoogleKey(string gcode)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();

            if (_user.ID == 0)
            {
                //身份验证过期请重新登陆
                result.code = 61005;
                result.success = false;
                result.msg = "身份验证过期请重新登陆";
                return result;
            }

            if (gcode == null)
            {
                //请输入谷歌验证码
                result.code = 61005;
                result.success = false;
                result.msg = "请输入谷歌验证码";
                return result;
            }


            var fuser = await _sysUserInfoServices.QueryById(_user.ID);
            var fgooglekey = MD5Helper.GenerateMD5(fuser.googleKey);
            GoogleAuthenticator authenticator = new GoogleAuthenticator(30, fgooglekey);
            string code = authenticator.GenerateCode();

            if (gcode.Equals(code))
            {
                fuser.isBindGoogle = true;
                result.response = fuser.googleKey;
                await _sysUserInfoServices.Update(fuser);
                return result;
            }
            else
            {
                result.code = 61005;
                result.success = false;
                result.msg = "验证失败";
                return result;
            }
        }

        //交易密码校验
        [HttpPost]
        public async Task<MessageModel<dynamic>> CheckUpwd(string pwd, string idcard, string idname, string phone, string addr)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();

            try
            {

                if (_user.ID == 0)
                {
                    //身份验证过期请重新登陆
                    result.code = 61005;
                    result.success = false;
                    result.msg = "身份验证过期请重新登陆!";
                    return result;
                }

                if (pwd == null)
                {
                    //请输入谷歌验证码
                    result.code = 61005;
                    result.success = false;
                    result.msg = "请输入交易密码！";
                    return result;
                }


                if (!CheckVeridenNo.ckidcard(idcard, idname))
                {
                    //请输入谷歌验证码
                    result.code = 61005;
                    result.success = false;
                    result.msg = "身份证验证不通过！";
                    return result;
                }


                var idresult = await _sysUserInfoServices.Query(x => x.IDNumber == MD5Helper.MD5Encrypt32(idcard));
                if (idresult.Count > 0)
                {
                    //请输入谷歌验证码
                    result.code = 61006;
                    result.success = false;
                    result.msg = "身份证验证已用于其他账号！";
                    return result;
                }

                var fuser = await _sysUserInfoServices.Query(x => x.uID == _user.ID && x.uTradPWD == MD5Helper.MD5Encrypt32(pwd));

                if (fuser.Count > 0)
                {

                    var model = fuser[0];
                    model.uRealName = idname;
                    model.IDNumber = MD5Helper.MD5Encrypt32(idcard);
                    model.useraddr = addr;
                    model.userphone = phone;
                    model.isSetIDNumber = 1;
                    await _sysUserInfoServices.Update(model);

                    return result;
                }
                else
                {
                    result.code = 61005;
                    result.success = false;
                    result.msg = "请输入正确交易密码";
                    return result;
                }
            }
            catch { result.code = 6001; result.msg = "请稍后尝试!"; result.success = false; return result; }

        }


        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="uHeadImgUrl">头像url</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> UpdateHeadImage(string uHeadImgUrl)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                code = 0,
                response = null
            };
            try
            {

                long uid = _user.ID;
                if (uid == 0)
                {
                    result.success = false;
                    result.code = 1003;
                    return result;
                }
                sysUserInfo userinfo = await _sysUserInfoServices.GetUserInfo(uid);
                if (userinfo == null)
                {
                    result.success = false;
                    result.code = 56001;
                    return result;
                }
                userinfo.uHeadImgUrl = uHeadImgUrl;

                var update = await _sysUserInfoServices.UpdateUserInfo(userinfo);
                if (!update)
                {
                    result.success = false;
                    result.code = 62001;
                    return result;
                }

                result.success = true;
                result.code = 0;
                result.response = new
                {
                    msg = "修改成功"
                };
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
        /// 获取聊天token及客服ID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<dynamic>> getMessgeToken()
        {
            sysUserInfo userinfo = await _sysUserInfoServices.GetUserInfo(_user.ID);
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                code = 0,
                response = null
            };
            if (string.IsNullOrEmpty(userinfo.messageToken))
            {
                string url = Appsettings.app(new string[] { "AppSettings", "MessageServer", "Url" });//获取server
                string urlBack = Appsettings.app(new string[] { "AppSettings", "MessageServer", "UrlBack" });//获取server
                string key = Appsettings.app(new string[] { "AppSettings", "MessageServer", "AppKey" });//获取AppKey
                string secret = Appsettings.app(new string[] { "AppSettings", "MessageServer", "AppSecret" });//获取AppSecret
                string api = "user/getToken.json";


                var nonce = Math.Round(100000M).ToString();
                var timestamp = DateHelper.GetCreatetime(DateTime.Now).ToString();
                IDictionary<string, string> header = new Dictionary<string, string>();
                header.Add("App-Key", key);
                header.Add("Nonce", nonce);
                header.Add("Timestamp", timestamp);
                header.Add("Signature", MD5Helper.SHA1Encrypt(secret + nonce + timestamp));

                IDictionary<string, string> param = new Dictionary<string, string>();
                param.Add("userId", userinfo.uID.ToString());
                param.Add("name", userinfo.uNickName);
                param.Add("portraitUri", userinfo.uHeadImgUrl);
                dynamic d = HttpHelper.PostApi<dynamic>(url + api, param, header);

                if (d.code == 200)
                {
                    userinfo.messageToken = d.token;
                    await _sysUserInfoServices.Update(userinfo);
                    result.response = new
                    {
                        token = d.token,
                        customer = "service"
                    };
                }
                else
                {
                    d = HttpHelper.PostApi<dynamic>(urlBack + api, header);
                    if (d.code == 200)
                    {
                        result.response = new
                        {
                            token = d.token,
                            customer = "service"
                        };
                    }
                    else
                    {
                        result.success = false;
                        result.code = -1;
                    }
                }
            }
            else
            {
                result.response = new
                {
                    token = userinfo.messageToken,
                    customer = "service"
                };
            }
            return result;


        }




        //修改头像和昵称
        [HttpPost]
        public async Task<MessageModel<dynamic>> UpdateHeadImageAndNickName(string nickname, string uHeadImgUrl)
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try
            {
                if (_user.ID == 0)
                {
                    result.success = false;
                    result.code = 1007;
                    result.msg = "登录身份已过期请重新登录！";
                    return result;
                }

                var userdata = await _sysUserInfoServices.QueryById(_user.ID);

                userdata.uNickName = string.IsNullOrEmpty(nickname) ? userdata.uNickName : nickname;
                userdata.uHeadImgUrl = string.IsNullOrEmpty(uHeadImgUrl) ? userdata.uHeadImgUrl : uHeadImgUrl;
                await _sysUserInfoServices.Update(userdata);

                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.code = 1007;
                result.msg = "请稍后再试！";
                return result;
            }

        }


        //Google验证码校验
        [HttpPost]
        public async Task<MessageModel<dynamic>> UnBindGoogleKey()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();

            if (_user.ID == 0)
            {
                //身份验证过期请重新登陆
                result.code = 61005;
                result.success = false;
                result.msg = "身份验证过期请重新登陆";
                return result;
            }


            var fuser = await _sysUserInfoServices.QueryById(_user.ID);
            var fgooglekey = MD5Helper.GenerateMD5(fuser.googleKey);
            GoogleAuthenticator authenticator = new GoogleAuthenticator(30, fgooglekey);
            string code = MD5Helper.GenerateUniqueText();


            fuser.isBindGoogle = false;
            fuser.googleKey = code;
            await _sysUserInfoServices.Update(fuser);
            return result;

        }


        //新增业绩
        //[HttpGet]
        //public  async Task <MessageModel<RequestApiWeekView>> getRequestApiinfoByInManageWork(string date1,string date2)
        //{

        //    StringBuilder buider = new StringBuilder();
        //    buider.AppendLine(" select CAST(uCreateTime as date) uRemark, b.count + c.count tid from sysUserInfo a ");
        //    buider.AppendLine(" left join(select CAST(uCreateTime as date) date,COUNT(uStatus) * 500 count from sysUserInfo where uStatus = 1 group by CAST(uCreateTime as date) ) b on b.date = CAST(a.uCreateTime as date)");
        //    buider.AppendLine(" left join(select CAST(uCreateTime as date) date,COUNT(uStatus) * 1000 count from sysUserInfo where uStatus = 2 group by CAST(uCreateTime as date) ) c on c.date = CAST(a.uCreateTime as date)");
        //    buider.AppendLine(" where a.uCreateTime between '2020-08-01' and '2020-08-15'");
        //    buider.AppendLine(" group by  CAST(uCreateTime as date),b.count,c.count");
        //    buider.AppendLine(" order by CAST(uCreateTime as date)");

        //    var result =  await _sysUserInfoServices.QueryTable(buider.ToString());
        //    var tmpresult = new RequestApiWeekView()
        //    {
        //        columns = new List<string> { "date", "count" }
        //    };

        //    return null;


        //}

        [HttpGet]
        public async Task<MessageModel<dynamic>> getRequestApiinfoByInManageWork(string date1, string date2, string options = "")
        {

            if (string.IsNullOrEmpty(date1))
            {
                date1 = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                date2 = DateTime.Now.ToString("yyyy-MM-dd");
            }
            StringBuilder buider = new StringBuilder();

            buider.AppendLine(" select CAST(CAST(uCreateTime as date) AS varchar(20)) date, b.count + c.count count,d.count epcount  from sysUserInfo a ");
            buider.AppendLine(" left join(select CAST(uCreateTime as date) date,COUNT(uStatus) * 500 count from sysUserInfo where uStatus = 1 group by CAST(uCreateTime as date) ) b on b.date = CAST(a.uCreateTime as date)");
            buider.AppendLine(" left join(select CAST(uCreateTime as date) date,COUNT(uStatus) * 1000 count from sysUserInfo where uStatus = 2 group by CAST(uCreateTime as date) ) c on c.date = CAST(a.uCreateTime as date)");
            buider.AppendLine("   left join( select CAST(createTime as date) date,SUM(amount) count from EPexchange where stype in (1,2,3)   group by  CAST(createTime as date) ) d on d.date = CAST(a.uCreateTime as date)");
            buider.AppendLine(string.Format(" where a.uCreateTime >='{0}' AND a.uCreateTime <='{1}'", date1, Convert.ToDateTime(date2).AddDays(1).ToString("yyyy-MM-dd")));
            buider.AppendLine(" group by  CAST(uCreateTime as date),b.count,c.count,d.count ");
            buider.AppendLine(" order by CAST(uCreateTime as date)");

            switch (options)
            {
                case "day":
                    date1 = DateTime.Now.ToString("yyyy-MM-dd");
                    date2 = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    break;
                case "month":
                    buider = new StringBuilder();
                    buider.AppendLine(" select cast(MONTH(uCreateTime) as varchar(20))  date, isnull(b.count+c.count,0) count,isnull(d.count,0) epcount from sysUserInfo a ");
                    buider.AppendLine(" left join (select  MONTH(uCreateTime) date,COUNT(uStatus)*500 count from sysUserInfo where uStatus=1 group by  MONTH(uCreateTime) ) b on b.date=MONTH(a.uCreateTime) ");
                    buider.AppendLine(" left join (select  MONTH(uCreateTime) date,COUNT(uStatus)*1000 count from sysUserInfo where uStatus=2 group by MONTH(uCreateTime) ) c on c.date=MONTH(a.uCreateTime) ");
                    buider.AppendLine(" left join( select  MONTH(createTime) date,SUM(amount) count from EPexchange where stype in (1,2,3)   group by  MONTH(createTime) ) d on d.date = MONTH(a.uCreateTime)");
                    //  buider.AppendLine(string.Format(" where a.uCreateTime >='{0}' AND a.uCreateTime <='{1}'", date1, Convert.ToDateTime(date2).AddDays(1).ToString("yyyy-MM-dd")));
                    buider.AppendLine(" group by  MONTH(uCreateTime ),b.count,c.count,d.count ");
                    buider.AppendLine(" order by MONTH(uCreateTime)");
                    break;
                case "year":
                    buider = new StringBuilder();
                    buider.AppendLine(" select Year(uCreateTime) date, isnull(b.count+c.count,0) count,isnull(d.count,0) epcount from sysUserInfo a ");
                    buider.AppendLine(" left join (select  Year(uCreateTime) date,COUNT(uStatus)*500 count from sysUserInfo where uStatus=1 group by  Year(uCreateTime) ) b on b.date=Year(a.uCreateTime) ");
                    buider.AppendLine(" left join (select  Year(uCreateTime) date,COUNT(uStatus)*1000 count from sysUserInfo where uStatus=2 group by Year(uCreateTime) ) c on c.date=Year(a.uCreateTime) ");
                    buider.AppendLine(" left join( select  Year(createTime) date,SUM(amount) count from EPexchange where stype in (1,2,3)   group by  Year(createTime) ) d on d.date = Year(a.uCreateTime)");
                    //  buider.AppendLine(string.Format(" where a.uCreateTime >='{0}' AND a.uCreateTime <='{1}'", date1, Convert.ToDateTime(date2).AddDays(1).ToString("yyyy-MM-dd")));
                    buider.AppendLine(" group by  Year(uCreateTime ),b.count,c.count,d.count ");
                    buider.AppendLine(" order by Year(uCreateTime)");
                    break;
                default:

                    break;

            }

            var result = await _sysUserInfoServices.QueryTable(buider.ToString());
            List<dynamic> list = new List<dynamic>();
            foreach (DataRow data in result.Rows)
            {
                list.Add(new { date = data["date"], count = data["count"], epcount = data["epcount"] }); //Convert.ToDateTime(data["date"]).ToString("yyyy-MM-dd")
            }

            return new MessageModel<dynamic>()
            {
                success = true,
                msg = "",
                response = new
                {
                    list = list
                }
            };

        }



        //锁定
        [HttpPost]
        public async Task<MessageModel<dynamic>> adminResetlock()
        {
            long uid = Convert.ToInt64(HttpContext.Request.Form["uid"]);
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                msg = "修改成功",
                code = 200
            };
            try
            {
                var model = await _sysUserInfoServices.QueryById(uid);

                if (model != null)
                {

                    model.isDelete = model.isDelete ? false : true;
                    await _sysUserInfoServices.Update(model);
                }
                else
                {
                    result.success = false;
                    result.code = 500;
                    result.msg = "修改失败";
                }
                return result;
            }
            catch
            {
                result.success = false;
                result.code = 500;
                result.msg = "修改失败";
                return result;
            }

        }


        //修改密码
        [HttpPost]
        public async Task<MessageModel<dynamic>> adminResetpwd()
        {

            long uid = Convert.ToInt64(HttpContext.Request.Form["uid"]);
            string pwd = HttpContext.Request.Form["pwd"];
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                msg = "修改成功",
                code = 200
            };

            try
            {
                var model = await _sysUserInfoServices.QueryById(uid);

                if (model != null)
                {
                    model.uLoginPWD = MD5Helper.MD5Encrypt32(pwd);
                    await _sysUserInfoServices.Update(model);
                }
                else
                {
                    result.success = false;
                    result.code = 500;
                    result.msg = "修改失败";
                }
                return result;
            }
            catch
            {
                result.success = false;
                result.code = 500;
                result.msg = "修改失败";
                return result;
            }

        }


        //修改荣誉
        [HttpPost]
        public async Task<MessageModel<dynamic>> adminResetlevel()
        {
            long uid = Convert.ToInt64(HttpContext.Request.Form["uid"]);
            int level = Convert.ToInt32(HttpContext.Request.Form["level"]);
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                msg = "修改成功",
                code = 200
            };

            try
            {
                var model = await _sysUserInfoServices.QueryById(uid);

                if (model != null)
                {
                    model.honorLevel = level;
                    await _sysUserInfoServices.Update(model);
                }
                else
                {
                    result.success = false;
                    result.code = 500;
                    result.msg = "修改失败";
                }
                return result;
            }
            catch
            {
                result.success = false;
                result.code = 500;
                result.msg = "修改失败";
                return result;
            }
        }

        //修改推荐人
        [HttpPost]
        public async Task<MessageModel<dynamic>> adminResettid()
        {

            long uid = Convert.ToInt64(HttpContext.Request.Form["uid"]);
            long tid = Convert.ToInt64(HttpContext.Request.Form["tid"]);
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                msg = "修改成功",
                code = 200
            };

            try
            {
                var model = await _sysUserInfoServices.QueryById(uid);

                if (model != null)
                {
                    model.tid = tid;
                    await _sysUserInfoServices.Update(model);
                }
                else
                {
                    result.success = false;
                    result.code = 500;
                    result.msg = "修改失败";
                }
                return result;
            }
            catch
            {
                result.success = false;
                result.code = 500;
                result.msg = "修改失败";
                return result;
            }
        }

        //修改密保
        [HttpPost]
        public async Task<MessageModel<dynamic>> adminResetanswer()
        {
            long uid = Convert.ToInt64(HttpContext.Request.Form["uid"]);
            string answer = HttpContext.Request.Form["answer"];
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                msg = "重置成功",
                code = 200
            };

            try
            {
                var usermodel = await _sysUserInfoServices.QueryById(uid);

                if (usermodel != null)
                {
                    var an = await _answerServices.Query(x => x.uID == uid);
                    foreach (Answer model in an)
                    {
                        model.answer = MD5Helper.MD5Encrypt32(answer);
                        await _answerServices.Update(model);
                    }
                }
                else
                {
                    result.success = false;
                    result.code = 500;
                    result.msg = "重置失败";
                }
                return result;
            }
            catch
            {
                result.success = false;
                result.code = 500;
                result.msg = "重置失败";
                return result;
            }
        }

        //修改身份证
        [HttpPost]
        public async Task<MessageModel<dynamic>> adminResetidcard()
        {
            long uid = Convert.ToInt64(HttpContext.Request.Form["uid"]);
            string realname = HttpContext.Request.Form["realname"];
            string idcard = HttpContext.Request.Form["idcard"];
            MessageModel<dynamic> result = new MessageModel<dynamic>()
            {
                success = true,
                msg = "修改成功",
                code = 200
            };

            if (!CheckVeridenNo.ckidcard(idcard, realname))
            {
                result.success = false;
                result.code = 500;
                result.msg = "实名认证失败";
                return result;
            }
            try
            {
                var model = await _sysUserInfoServices.QueryById(uid);

                if (model != null)
                {
                    model.uRealName = realname;
                    model.IDNumber = MD5Helper.MD5Encrypt32(idcard);
                    model.isSetIDNumber = 1;
                    await _sysUserInfoServices.Update(model);
                }
                else
                {
                    result.success = false;
                    result.code = 500;
                    result.msg = "修改失败";
                }
                return result;
            }
            catch
            {
                result.success = false;
                result.code = 500;
                result.msg = "修改失败";
                return result;
            }
        }

        //获取feedback 
        [HttpPost]
        public async Task<MessageModel<dynamic>> GetAdminUserFeedBack()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            int pageindex = Convert.ToInt32(HttpContext.Request.Form["pageindex"]);
            int pagesize = Convert.ToInt32(HttpContext.Request.Form["pagesize"]);
            string key = HttpContext.Request.Form["key"];

            try
            {
                if (_user.ID == 0)
                {
                    result.code = 10001;
                    result.msg = "非法请求!";
                    result.success = false;
                    return result;
                }

                if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                {
                    key = "";
                }

                var data = await _iuserfeedbackservices.QueryPage(a => ((a.uId.ToString().Contains(key)) || a.Message.Contains(key)), pageindex, pagesize, " CreateTime desc ");
                result.response = data;
                result.code = 200;
                result.success = true;
                return result;
            }
            catch
            {
                result.code = 70001;
                result.msg = "查询异常请稍后查询";
                result.success = false;
                return result;
            }
        }


        //管理员回复feedback 
        [HttpPost]
        public async Task<MessageModel<dynamic>> AdminDisposeFeedBack()
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            int Id = Convert.ToInt32(HttpContext.Request.Form["Id"]);
            string msg = HttpContext.Request.Form["msg"];

            try
            {
                if (_user.ID == 0)
                {
                    result.code = 10001;
                    result.msg = "非法请求!";
                    result.success = false;
                    return result;
                }

                var userfeedback = await _iuserfeedbackservices.QueryById(Id);
                var data = await _idisposefeedbackservices.Add(new DisposeFeedback { IReply = true, CreateTime = DateTime.Now, IsDelete = false, Message = msg, MessageImgUrl = "", MessageUid= userfeedback.uId, Messageid = Id });

                if (data > 0)
                {
                    var userfeed = await _iuserfeedbackservices.QueryById(Id);
                    userfeed.IsReply = 1;
                    await _iuserfeedbackservices.Update(userfeed);
                }
                result.code = 200;
                result.success = true;
                return result;
            }
            catch
            {
                result.code = 70001;
                result.msg = "回复失败请稍后再试";
                result.success = false;
                return result;
            }
        }

        //双击获取内容
        [HttpPost]
        public async Task<MessageModel<dynamic>> GetAdminAllUserFeedBack()
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

                int UId = Convert.ToInt32(HttpContext.Request.Form["uid"]);
                var userfeedback = await _iuserfeedbackservices.Query(x => x.uId == UId);

                string uidDetails = string.Join(",", userfeedback.Select(t => t.Id).Distinct().ToArray());
                var mangerfeedback = await _idisposefeedbackservices.Query(x => (uidDetails.Contains(x.MessageUid.ToString())));
                List<dynamic> list = new List<dynamic>();
                foreach (UserFeedback model in userfeedback)
                {
                    list.Add(new { id = model.Id, isadmin = false, date = model.CreateTime, msg = model.Message });
                }

                foreach (DisposeFeedback model in mangerfeedback)
                {
                    list.Add(new { id = model.id, isadmin = true, date = model.CreateTime, msg = model.Message });
                }

                result.response = list.OrderBy(t => t.date);
                result.code = 200;
                result.success = true;
                return result;
            }
            catch
            {
                result.code = 70001;
                result.msg = "查询异常请稍后查询";
                result.success = false;
                return result;
            }
        }


        //获取SP_SearchServic
        [HttpPost]
        public async Task<MessageModel<dynamic>> GetSPSearchServicInfo()
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
                string beginTime = HttpContext.Request.Form["beginTime"];
                string endTime = HttpContext.Request.Form["endTime"];

                beginTime = string.IsNullOrEmpty(beginTime) ? "" : beginTime;
                endTime = string.IsNullOrEmpty(endTime) ? "" : endTime;

                var data = await _userInfoServices.GetSPSearchServic(beginTime, endTime);

                result.response = data;
                result.code = 200;
                result.success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.code = 70001;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }
        }

        //GetStockInfoDay
        [HttpPost]
        public async Task<MessageModel<dynamic>> GetStockInfoDay()
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

                var data = await _userInfoServices.QueryTable("SELECT SUM(amount) dpe,(SELECT Total FROM  transTotal) transtotal,(SELECT MAX(uID) FROM  dbo.sysUserInfo) newuid FROM dbo.DPE");

                result.response = data;
                result.code = 200;
                result.success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.code = 70001;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }
        }


        //更新银行卡信息
        [HttpPost]
        public async Task<MessageModel<dynamic>> updatebankinfo(int type,string alipayname="",string alipayaccount="",string bankidcard="",string bankaddr="",string bankname="",string addr="")
        {
            MessageModel<dynamic> result = new MessageModel<dynamic>();
            try 
            {
                var user = await _sysUserInfoServices.QueryById(_user.ID);
                switch (type) 
                {
                    case 1:
                        user.alipayname = alipayname;
                        user.alipayaccount = alipayaccount;
                        break;
                    case 2:
                        user.bankidcard = bankidcard;
                        user.bankaddr = bankaddr;
                        user.bankname = bankname;
                        break;
                    case 3:
                        user.addr = addr;
                        break;
                }
                if (await _sysUserInfoServices.Update(user)) 
                {
                    result.code = 0;
                    result.msg = "更新成功";
                    result.success = true;
                }
                else 
                {
                    result.code = 10002;
                    result.msg = "更新失败";
                    result.success = false;
                }
                return result;
            }
            catch 
            {
                result.code = 5000;
                result.msg = "更新失败请稍后再试";
                return result;
            }
           
        }


    }
}


