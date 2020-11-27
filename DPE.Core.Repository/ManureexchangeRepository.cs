using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// ManureexchangeRepository
	/// </summary>
    public class ManureexchangeRepository : BaseRepository<Manureexchange>, IManureexchangeRepository
    {
        public ManureexchangeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}