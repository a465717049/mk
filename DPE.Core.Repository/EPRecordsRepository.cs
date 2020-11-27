using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// EPRecordsRepository
	/// </summary>
    public class EPRecordsRepository : BaseRepository<EPRecords>, IEPRecordsRepository
    {
        public EPRecordsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}