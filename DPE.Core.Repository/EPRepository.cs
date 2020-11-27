using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// EPRepository
	/// </summary>
    public class EPRepository : BaseRepository<EP>, IEPRepository
    {
        public EPRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}