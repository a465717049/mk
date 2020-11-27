using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
    public class ExchangeTotalRepository : BaseRepository<ExchangeTotal>, IExchangeTotalRepository
    {

        public ExchangeTotalRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
