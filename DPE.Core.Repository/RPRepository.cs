using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// RPRepository
	/// </summary>
    public class RPRepository : BaseRepository<RP>, DPE.Core.IRepository.IRPRepository
    {
        public RPRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}