using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// DPETaskRepository
	/// </summary>
    public class DPETaskRepository : BaseRepository<DPETask>, IDPETaskRepository
    {
        public DPETaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}