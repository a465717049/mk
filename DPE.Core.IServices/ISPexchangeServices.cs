using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DPE.Core.IServices
{	
	/// <summary>
	/// ISPexchangeServices
	/// </summary>	
    public interface ISPexchangeServices :IBaseServices<SPexchange>
	{
		Task<List<SPexchange>> GetSPExchange(long uid);
	}
}