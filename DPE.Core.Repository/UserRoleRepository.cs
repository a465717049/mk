using DPE.Core.FrameWork.IRepository;
using DPE.Core.Repository.Base;
using DPE.Core.Model.Models;
using DPE.Core.IRepository.UnitOfWork;

namespace DPE.Core.Repository
{
    /// <summary>
    /// UserRoleRepository
    /// </summary>	
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
