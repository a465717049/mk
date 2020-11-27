using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// FAQRepository
	/// </summary>
    public class FAQRepository : BaseRepository<FAQ>, IFAQRepository
    {
        public FAQRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}