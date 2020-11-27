using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// EPexchangeRepository
	/// </summary>
    public class EPexchangeRepository : BaseRepository<EPexchange>, IEPexchangeRepository
    {
        public EPexchangeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}