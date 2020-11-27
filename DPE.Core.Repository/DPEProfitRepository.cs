using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// DPEProfitRepository
	/// </summary>
    public class DPEProfitRepository : BaseRepository<DPEProfit>, IDPEProfitRepository
    {
        public DPEProfitRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}