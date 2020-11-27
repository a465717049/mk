using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// UserFeedbackRepository
	/// </summary>
    public class UserFeedbackRepository : BaseRepository<UserFeedback>, IUserFeedbackRepository
    {
        public UserFeedbackRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}