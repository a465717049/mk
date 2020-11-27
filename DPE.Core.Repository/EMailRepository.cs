using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// EMailRepository
	/// </summary>
    public class EMailRepository : BaseRepository<EMail>, IEMailRepository
    {
        public EMailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}