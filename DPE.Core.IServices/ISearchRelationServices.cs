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
    public interface ISearchRelationServices : IBaseServices<SearchRelation>
    {
        Task<List<SearchRelation>> SPSearchRelation(long parentId);

        Task<List<SearchRelation>> SPSearchFamily(long parentId, long uid);
    }
}
