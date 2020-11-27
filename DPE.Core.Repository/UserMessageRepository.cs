using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// UserMessageRepository
	/// </summary>
    public class UserMessageRepository : BaseRepository<UserMessage>, IUserMessageRepository
    {
        public UserMessageRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}