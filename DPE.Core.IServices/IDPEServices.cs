using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// IDPEServices
	/// </summary>	
    public interface IDPEServices :IBaseServices<DPE.Core.Model.Models.DPE>
	{
		Task<DPE.Core.Model.Models.DPE> GetDPEData(long uid);
	}
}