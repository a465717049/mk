using DPE.Core.IServices.BASE;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// IRPexchangeServices
	/// </summary>	
    public interface IRPexchangeServices :IBaseServices<RPexchange>
	{
		Task<MessageModel<string>> ProcessTransformToUser(long uid, string type, decimal amount, long touid);
	}
}