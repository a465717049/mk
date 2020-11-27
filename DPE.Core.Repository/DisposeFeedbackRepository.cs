using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
    /// <summary>
    /// CompanyProfitRepository
    /// </summary>
    public class DisposeFeedbackRepository : BaseRepository<DisposeFeedback>, IDisposeFeedbackRepository
    {
        public DisposeFeedbackRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}