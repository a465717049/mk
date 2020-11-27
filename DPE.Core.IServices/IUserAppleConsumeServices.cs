using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{
	/// <summary>
	/// IUserAppleConsumeServices
	/// </summary>	
	public interface IUserAppleConsumeServices : IBaseServices<UserAppleConsume>
	{
		Task<UserAppleConsume> GetUserAppleConsumeByType(long uid, int type);

	}
    
}
