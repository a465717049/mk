using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// UserDataRepository
	/// </summary>
    public class UserDataRepository : BaseRepository<UserData>, IUserDataRepository
    {
        public UserDataRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}