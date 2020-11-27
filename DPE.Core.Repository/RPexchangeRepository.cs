using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// RPexchangeRepository
	/// </summary>
    public class RPexchangeRepository : BaseRepository<RPexchange>, DPE.Core.IRepository.IRPexchangeRepository
    {
        public RPexchangeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}