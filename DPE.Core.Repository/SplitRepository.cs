using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// SplitRepository
	/// </summary>
    public class SplitRepository : BaseRepository<Split>, ISplitRepository
    {
        public SplitRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}