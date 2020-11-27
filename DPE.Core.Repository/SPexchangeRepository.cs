using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// SPexchangeRepository
	/// </summary>
    public class SPexchangeRepository : BaseRepository<SPexchange>, ISPexchangeRepository
    {
        public SPexchangeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}