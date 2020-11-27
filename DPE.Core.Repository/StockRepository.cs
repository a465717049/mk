using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// StockRepository
	/// </summary>
    public class StockRepository : BaseRepository<Stock>, IStockRepository
    {
        public StockRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}