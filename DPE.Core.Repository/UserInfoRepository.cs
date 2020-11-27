using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// UserInfoRepository
	/// </summary>
    public class UserInfoRepository : BaseRepository<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}