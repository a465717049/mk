using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// BuyDPEListRepository
	/// </summary>
    public class BuyDPEListRepository : BaseRepository<BuyDPEList>, IBuyDPEListRepository
    {
        public BuyDPEListRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}