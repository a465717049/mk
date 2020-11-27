using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// IManureServices
	/// </summary>	
    public interface IManureServices :IBaseServices<Manure>
	{
		Task<bool> UpdateManure(Manure manure);

		Task<Manure> GetOne(long uid);
	}
}