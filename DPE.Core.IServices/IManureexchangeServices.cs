using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// IManureexchangeServices
	/// </summary>	
    public interface IManureexchangeServices :IBaseServices<Manureexchange>
	{
		Task<List<Manureexchange>> GetManureExchange(long uid);

		Task<int> AddManureExchange(Manureexchange manureexchange);
	}
}