using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// ADexchangeRepository
	/// </summary>
    public class ADexchangeRepository : BaseRepository<ADexchange>, IADexchangeRepository
    {
        public ADexchangeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}