using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// DPEMessageRepository
	/// </summary>
    public class DPEMessageRepository : BaseRepository<DPEMessage>, IDPEMessageRepository
    {
        public DPEMessageRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}