using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// IRPexchangeRepository
	/// </summary>
    public class IRPexchangeRepository : BaseRepository<IRPexchange>, IIRPexchangeRepository
    {
        public IRPexchangeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}