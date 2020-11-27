using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// IUserAppleStatusServices
	/// </summary>	
    public interface IUserAppleStatusServices :IBaseServices<UserAppleStatus>
	{
		Task<int> AddUserAppleStatus(UserAppleStatus userAppleStatus);

		Task<UserAppleStatus> GetUserAppleStatus(long uid);

		Task<bool> UpdateUserAppleStatus(UserAppleStatus userAppleStatus);
	}
}