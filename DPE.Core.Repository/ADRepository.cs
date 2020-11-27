using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// ADRepository
	/// </summary>
    public class ADRepository : BaseRepository<AD>, IADRepository
    {
        public ADRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}