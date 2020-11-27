using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// UserRoleServices
	/// </summary>	
    public interface IUserRoleServices :IBaseServices<UserRole>
	{

        Task<UserRole> SaveUserRole(long uid, int rid);
        Task<int> GetRoleIdByUid(long uid);
    }
}

