using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// IStockServices
	/// </summary>	
    public interface IStockServices :IBaseServices<Stock>
	{
		Task<Stock> GetStock();
    }
}