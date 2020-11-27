using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DPE.Core.IServices
{	
	/// <summary>
	/// IEMailServices
	/// </summary>	
    public interface IEMailServices :IBaseServices<EMail>
	{
		Task<List<EMail>> GetEmailListInfo(long fromUid);

		Task<EMail> GetEmailInfo(int id);

	
		
	}
}