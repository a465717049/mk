using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// ManureRepository
	/// </summary>
    public class ManureRepository : BaseRepository<Manure>, IManureRepository
    {
        public ManureRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}