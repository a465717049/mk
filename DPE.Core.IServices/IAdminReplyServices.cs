using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// IAdminReplyServices
	/// </summary>	
    public interface IAdminReplyServices :IBaseServices<AdminReply>
	{
		Task<List<AdminReply>> GetAdminReplies(int ufbId);

		Task<int> AddAdminReply(AdminReply adminReply);

		Task<bool> UpdateAdminReply(AdminReply adminReply);

		Task<bool> DeleteAdminReply(AdminReply adminReply);
	}
}