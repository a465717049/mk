using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// DPERepository
	/// </summary>
    public class DPERepository : BaseRepository<DPE.Core.Model.Models.DPE>, IDPERepository
    {
        public DPERepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}