    

using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// sysUserInfoServices
	/// </summary>	
    public interface ISysUserInfoServices :IBaseServices<sysUserInfo>
	{
        Task<sysUserInfo> Register(string loginName, string loginPwd,long yid);
        Task<sysUserInfo> SaveUserInfo(string loginName, string loginPwd);
        Task<string> GetUserRoleNameStr(string loginName, string loginPwd);

        Task<bool> SetUserInfoSex(long uid, int sex);

        Task<bool> SetUserInfoName(long uid, string name);

        Task<bool> SetUserInfolocation(long uid, string location,string addr);

        Task<bool> SetUserInfoidcard(long uid, string name, string idcard);

        Task<bool> SetUserInfouStatus(long uid, decimal amount);

        Task<bool> UpdateUserInfo(sysUserInfo user);

        Task<sysUserInfo> GetUserInfo(long uid);

        Task<DataTable> GetRelationList(long parentid, long uid);
        Task<DataTable> GetRelationListbyid(long parentid);
        Task<DataTable> GetFriendsList(long parentid,long uid);
        Task<DataTable> GetFimalyList(long parentid, long uid);
        Task<sysUserInfo> checkTrad(long uid, string password);
        Task<sysUserInfo> checkGoogleKey(long uid, string googleKey);

        Task<DataTable> AddSpCreatePayUser(AddNewUserModel model);

        //更新等级
        Task<DataTable> UpdateLevelByWeb(long uid ,int level);

        Task<DataTable> AddSpCreateSubUser(long parentID, long uid, int amount, int area);

        Task<DataTable> UpdateOneKeyReturn(long uid);

        

    }
}
