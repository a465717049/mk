using DPE.Core.IServices.BASE;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DPE.Core.IServices
{
	/// <summary>
	/// IEPServices
	/// </summary>	
	public interface IEPServices : IBaseServices<EP>
	{
		Task<MessageModel<string>> EPSell(EPSellParamsModel ePSellParams);
		Task<MessageModel<string>> EPTransOtherType(string oType, string dType, string password, string googlekey, decimal amount, long uID);
		Task<MessageModel<string>> TransToUser(string oType, long userID, string password, string googlekey, decimal amount, long uID);
		Task<string> checkEPUser(long uid, long parentId, string type);
    }
}