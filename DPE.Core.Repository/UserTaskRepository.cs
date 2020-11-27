using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// UserTaskRepository
	/// </summary>
    public class UserTaskRepository : BaseRepository<UserTask>, IUserTaskRepository
    {
        public UserTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}