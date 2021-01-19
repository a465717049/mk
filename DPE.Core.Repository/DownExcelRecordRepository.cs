using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
    /// <summary>
    /// DownExcelRecordRepository
    /// </summary>
    public class DownExcelRecordRepository : BaseRepository<DownExcelRecord>, IDownExcelRecordRepository
    {
        public DownExcelRecordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}