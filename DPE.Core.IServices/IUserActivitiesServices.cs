using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Threading.Tasks;
namespace DPE.Core.IServices
{	
	/// <summary>
	/// IUserActivitiesServices
	/// </summary>	
    public interface IUserActivitiesServices :IBaseServices<UserActivities>
	{
		Task<int> CkUserActivities(long uid, int  actId);
	}
}