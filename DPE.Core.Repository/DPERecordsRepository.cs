using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// DPERecordsRepository
	/// </summary>
    public class DPERecordsRepository : BaseRepository<DPERecords>, IDPERecordsRepository
    {
        public DPERecordsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}