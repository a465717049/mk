using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// AdminReplyRepository
	/// </summary>
    public class AdminReplyRepository : BaseRepository<AdminReply>, IAdminReplyRepository
    {
        public AdminReplyRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}