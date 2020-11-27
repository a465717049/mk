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
using DPE.Core.IRepository.UnitOfWork;
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
    /// UserActivities活动报名 管理【权限】
    /// </summary>
    [Produces("application/json")]
    [Route("api/UserActivities")]
    [Authorize(Permissions.Name)]
    public class UserActivitiesController :Controller
    {

        private readonly IUserActivitiesServices _iUseractivitiesservices;
        private readonly IUserTaskServices _iusertaskservices;

        private readonly IDPETaskServices _idpetaskservices;
        private readonly IDPEActivitiesServices _iDpeactivitiesservices;
        readonly IUser _user;
        readonly IUserInfoServices _userInfoServices;

        readonly IEPServices _iepservices;
        readonly IEPexchangeServices _iepexchangeservices;

        private readonly IUnitOfWork _unitOfWork;

        public UserActivitiesController(IEPServices iepservices, IUnitOfWork unitOfWork,IUserTaskServices iusertaskservices, IEPexchangeServices iepexchangeservices, IUser user, IUserActivitiesServices iUseractivitiesservices, IUserInfoServices userInfoServices, IDPEActivitiesServices iDpeactivitiesservices)
        {
            this._user = user;
            _userInfoServices = userInfoServices;
            _iUseractivitiesservices = iUseractivitiesservices;
            _iDpeactivitiesservices = iDpeactivitiesservices;
            _iusertaskservices = iusertaskservices;
            _unitOfWork = unitOfWork;
            _iusertaskservices = iusertaskservices;
            _iepservices = iepservices;
            _iepexchangeservices = iepexchangeservices;
        }

        /// <summary>
        /// 活动报名
        /// </summary>
        /// <param name="id">活动id // 0 没有报名 1 已报名</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserActivities")]
        public async Task<MessageModel<UserActivitiesViewModels>> GetUserActivities(int id)
        {
            //_user.ID
            var remsg = new MessageModel<UserActivitiesViewModels>()
            {
                success = false,
                msg = "",
                code = 36001,
                response = new UserActivitiesViewModels()
                {
                    id = id,
                    status = 0
                }
            };

            _unitOfWork.BeginTran();
            try
            {
                var dpeact = await _iDpeactivitiesservices.QueryById(id);
                if (dpeact == null || _user.ID == 0)
                {
                    return remsg;
                }

                //判断当前是否足够EP
                var user = await _userInfoServices.GetUserInfo(_user.ID);
                if (user.EP < dpeact.amount) 
                {
                    //金币不足
                    remsg.code = 36003;
                    return remsg;
                }

                var usersstatus = await _iUseractivitiesservices.Query(x => x.uid == _user.ID && x.actID == id);
             
                if (usersstatus.Count <= 0)
                {
                    await _iUseractivitiesservices.Add(new UserActivities { uid = _user.ID, actID = id, status = 1 });
                }
                else
                {
                    if (usersstatus.First().status == 1)
                    {
                        //已参加过
                        remsg.code = 36002;
                        return remsg;
                    }
                    usersstatus.First().status = 1;
                    await _iUseractivitiesservices.Update(usersstatus);
                }
                dpeact.qty = dpeact.qty + 1;
                await _iDpeactivitiesservices.Update(dpeact);

                //执行完 扣金币
                EP modelep = await _iepservices.QueryById(_user.ID);

                //插入ep
                await _iepexchangeservices.Add(new EPexchange { amount=dpeact.amount, fromID=_user.ID, createTime=DateTime.Now, lastTotal=user.EP
                ,price=dpeact.amount, recordID=_user.ID, remark="参加活动", scount=0, uID=_user.ID, stype=98});

                modelep.amount = modelep.amount - dpeact.amount;
                await _iepservices.Update(modelep);


                _unitOfWork.CommitTran();
                return new MessageModel<UserActivitiesViewModels>()
                {
                    success = true,
                    msg = "",
                    response = new UserActivitiesViewModels()
                    {
                        id = id,
                        status = 1,
                        code = 0
                    }
                };
            }
            catch
            {
                _unitOfWork.RollbackTran();
                return remsg;
            }

        }

    }
}
