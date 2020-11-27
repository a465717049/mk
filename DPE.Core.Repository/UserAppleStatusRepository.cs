using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// UserAppleStatusRepository
	/// </summary>
    public class UserAppleStatusRepository : BaseRepository<UserAppleStatus>, IUserAppleStatusRepository
    {
        public UserAppleStatusRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}