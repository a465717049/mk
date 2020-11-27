using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// UserAppleConsumeRepository
	/// </summary>
    public class UserAppleConsumeRepository : BaseRepository<UserAppleConsume>, IUserAppleConsumeRepository
    {
        public UserAppleConsumeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}