using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// ISplitServices
	/// </summary>	
    public interface ISplitServices :IBaseServices<Split>
	{
		Task<Split> GetOneByUId(long uid);

		Task<bool> UpdateSplit(Split split);

	}
}