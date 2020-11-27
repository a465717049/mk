using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// SPRepository
	/// </summary>
    public class SPRepository : BaseRepository<SP>, ISPRepository
    {
        public SPRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}