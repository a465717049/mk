using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// IRPRepository
	/// </summary>
    public class IRPRepository : BaseRepository<IRP>, IIRPRepository
    {
        public IRPRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}