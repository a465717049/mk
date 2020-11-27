using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;


namespace DPE.Core.Repository
{
    public class SearchRelationRepository : BaseRepository<SearchRelation>, ISearchRelationRepository
    {
        public SearchRelationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
