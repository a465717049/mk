using DPE.Core.IServices.BASE;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DPE.Core.IServices
{	
	/// <summary>
	/// IEPexchangeServices
	/// </summary>	
    public interface IEPexchangeServices :IBaseServices<EPexchange>
	{
		Task<List<EPexchange>> GetEPExchange(long uid);

		 Task<List<EPexchange>> GetEPGoldExchange(long uid);

		Task<MessageModel<dynamic>> GetEPExchangePageList(int stype, int pageSize, int pageIndex, long uID);

		Task<MessageModel<string>> ProcessTransformToUser(long uid, string type, decimal amount, long touid);

		Task<bool> RoolBackThisTran(long id,string type);
	}
}