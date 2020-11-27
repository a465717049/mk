using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// SplitRecordsRepository
	/// </summary>
    public class SplitRecordsRepository : BaseRepository<SplitRecords>, ISplitRecordsRepository
    {
        public SplitRecordsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}