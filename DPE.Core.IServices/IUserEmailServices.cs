using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// IUserEmailServices
	/// </summary>	
    public interface IUserEmailServices :IBaseServices<UserEmail>
	{

		Task<bool> DeleteEmail(int id, long uid);

		Task<bool> DeleteEmailAll(long uid);

		Task<int> ckUserEmailStatus(long uid,long eId);
	}
}