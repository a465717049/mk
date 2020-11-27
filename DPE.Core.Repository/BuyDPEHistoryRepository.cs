using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// BuyDPEHistoryRepository
	/// </summary>
    public class BuyDPEHistoryRepository : BaseRepository<BuyDPEHistory>, IBuyDPEHistoryRepository
    {
        public BuyDPEHistoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}