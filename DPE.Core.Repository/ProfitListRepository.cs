using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// ProfitListRepository
	/// </summary>
    public class ProfitListRepository : BaseRepository<ProfitList>, IProfitListRepository
    {
        public ProfitListRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}