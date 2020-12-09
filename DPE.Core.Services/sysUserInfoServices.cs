using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using DPE.Core.IServices;
using DPE.Core.IRepository;
using System.Threading.Tasks;
using System.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using DPE.Core.Common.Helper;
using System.Data;

namespace DPE.Core.FrameWork.Services
{
    /// <summary>
    /// sysUserInfoServices
    /// </summary>	
    public class SysUserInfoServices : BaseServices<sysUserInfo>, ISysUserInfoServices
    {

        IsysUserInfoRepository _dal;
        IUserRoleServices _userRoleServices;
        IRoleRepository _roleRepository;
        IBuyDPEListRepository _ibuydpelistrepository;
        
        public SysUserInfoServices(IsysUserInfoRepository dal, IUserRoleServices userRoleServices, IRoleRepository roleRepository, IBuyDPEListRepository ibuydpelistrepository)
        {
            this._dal = dal;
            this._userRoleServices = userRoleServices;
            this._roleRepository = roleRepository;
            base.BaseDal = dal;
            _ibuydpelistrepository = ibuydpelistrepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public async Task<sysUserInfo> SaveUserInfo(string loginName, string loginPwd)
        {
            sysUserInfo sysUserInfo = new sysUserInfo(loginName, loginPwd);
            sysUserInfo model = new sysUserInfo();
            var userList = await base.Query(a => a.uLoginName == sysUserInfo.uLoginName && a.uLoginPWD == sysUserInfo.uLoginPWD);
            if (userList.Count > 0)
            {
                model = userList.FirstOrDefault();
            }
            else
            {
                var id = await base.Add(sysUserInfo);
                model = await base.QueryById(id);
            }

            return model;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public async Task<string> GetUserRoleNameStr(string loginName, string loginPwd)
        {
            string roleName = "";
            var user = (await base.Query(a => a.uLoginName == loginName && a.uLoginPWD == loginPwd)).FirstOrDefault();
            var roleList = await _roleRepository.Query(a => a.IsDeleted==false);
            if (user != null)
            {
                var userRoles = await _userRoleServices.Query(ur => ur.UserId == user.uID);
                if (userRoles.Count > 0)
                {
                    var arr = userRoles.Select(ur => ur.RoleId.ObjToString()).ToList();
                    var roles = roleList.Where(d => arr.Contains(d.Id.ObjToString()));
                    roleName = string.Join(',', roles.Select(r => r.Name).ToArray());
                }
            }
            return roleName;
        }
        public async Task<sysUserInfo> Register(string loginName, string loginPwd, long yid)
        {
            SugarParameter[] par = new SugarParameter[] {
                new SugarParameter("uLoginPWD",loginPwd),
                new SugarParameter("uLoginName",loginName),
                new SugarParameter("yid",yid)
            };
            return (await _dal.QueryProc("Sp_CreateNewUser", par))[0];
        }
        public async Task<bool> SetUserInfoSex(long uid, int sex)
        {
            var user = await _dal.QueryById(uid);
            user.sex = sex;
            bool isfig = await _dal.Update(user);

            return isfig;
        }

        public async Task<bool> SetUserInfoName(long uid, string name)
        {
            var user = await _dal.QueryById(uid);
            user.uNickName = name;
            bool isfig = await _dal.Update(user);
            return isfig;
        }

        public async Task<bool> SetUserInfolocation(long uid, string location,string addr)
        {
            var user = await _dal.QueryById(uid);
            user.addr = location;
            user.Taddr = addr;
            bool isfig = await _dal.Update(user);
            return isfig;
        }


        public async Task<bool> SetUserInfoidcard(long uid, string name, string idcard)
        {
            var user = await _dal.QueryById(uid);
            user.IDNumber = idcard;
            user.uRealName = name;
            bool isfig = await _dal.Update(user);
            return isfig;
        }

        public async Task<bool> SetUserInfouStatus(long uid,decimal amount)
        {
            var user = await _dal.QueryById(uid);
            if (user.uStatus == 1) { return true; }
            user.uStatus = 1;
            bool isfig = await _dal.Update(user);

            //成为农场主之后插入EPbuyList
            BuyDPEList buy = new BuyDPEList() { uID = uid, amount = amount, createTime = DateTime.Now };
             await  _ibuydpelistrepository.Add(buy);
            return isfig; 
        }

        public async Task<bool> UpdateUserInfo(sysUserInfo user)
        {
            return await base.Update(user);
        }

        public async Task<sysUserInfo> GetUserInfo(long uid)
        {
            sysUserInfo model = null;

            var data = await base.Query(c => c.uID == uid);
            if (data != null && data.Count > 0) 
            {
                model = data.FirstOrDefault();
            }

            return model;
        }

        public async Task<DataTable> GetRelationList(long parentid,long uid)
        {
            SugarParameter[] parameters = new SugarParameter[2];
            parameters[0] = new SugarParameter("@parentId", parentid);
            parameters[1] = new SugarParameter("@maxId", uid);
            return await _dal.QueryProcTable("SP_SearchRelation", parameters);
        }
        public async Task<DataTable> GetFimalyList(long parentid,long uid)
        {
            SugarParameter[] parameters = new SugarParameter[2];
            parameters[0] = new SugarParameter("@parentId", parentid);
            parameters[1] = new SugarParameter("@uid", uid);
            return await _dal.QueryProcTable("SP_SearchFamily", parameters);
        }


        public async Task<DataTable> AddSpCreatePayUser(AddNewUserModel model)
        {
            SugarParameter[] par = new SugarParameter[] {
                new SugarParameter("parentID",model.parentID),
                new SugarParameter("idType",model.idType),
                new SugarParameter("idNumber",model.idNumber),
                new SugarParameter("uRealName",model.uRealName),
                new SugarParameter("bankCardName",model.bankCardName),
                new SugarParameter("loginPass",model.loginPass),
                new SugarParameter("investmentAmount",model.investmentAmount),
                new SugarParameter("CountryPhoneCode",model.CountryPhoneCode),
                new SugarParameter("MemberNo",model.MemberNo),
                new SugarParameter("NickName",model.NickName),
                new SugarParameter("googleCode",model.googleCode),
                new SugarParameter("TradePass",model.TradePass),
                new SugarParameter("TransUserID",model.TransUserID),
                new SugarParameter("Jid",model.Jid),
                new SugarParameter("phone",model.phone),
                new SugarParameter("addr",model.addr)
            };
            return await _dal.QueryProcTable("Sp_CreatePayUser", par);
        }

        public async Task<DataTable> UpdateOneKeyReturn(long uid)
        {
            SugarParameter[] par = new SugarParameter[] {
                new SugarParameter("userid",uid)

            };
            return await _dal.QueryProcTable("ReturnToMain", par);
        }


        public async Task<DataTable> AddSpCreateSubUser(long parentID, long uid, int amount, int area)
        {
            SugarParameter[] par = new SugarParameter[] {
                new SugarParameter("parentID",parentID),
                new SugarParameter("investmentAmount",amount),
                new SugarParameter("Jid",uid),
                new SugarParameter("L",area)
            };
            return await _dal.QueryProcTable("Sp_CreatePaySonUser", par);
        }

      


        public async Task<DataTable> GetFriendsList(long parentid, long uid)
        {

            SugarParameter[] parameters = new SugarParameter[2];
            parameters[0] = new SugarParameter("@parentId", parentid);
            parameters[1] = new SugarParameter("@uid", uid);
            return await _dal.QueryProcTable("SP_SearchFriends", parameters);
        }

        public async Task<sysUserInfo> checkTrad(long uid,string password)
        {
            var user = (await base.Query(d => d.uID == uid && d.uTradPWD == MD5Helper.MD5Encrypt32(password) && d.isDelete == false)).ToList().FirstOrDefault();
            return user;
        }
        public async Task<sysUserInfo> checkGoogleKey(long uid, string googleKey)
        {
            var user = (await base.Query(d => d.uID == uid && d.isDelete == false)).ToList().FirstOrDefault();
            var key = MD5Helper.GenerateMD5(user.googleKey);
            GoogleAuthenticator authenticator = new GoogleAuthenticator(30, key);
            string code = authenticator.GenerateCode();
            if (code != googleKey)
            {
               
                return null;
            }
            else
            {
                return user;
            }
            
        }
    }
}

//----------sysUserInfo结束----------
