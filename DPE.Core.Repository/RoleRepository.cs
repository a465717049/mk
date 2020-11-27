using DPE.Core.IRepository;
using DPE.Core.Repository.Base;
using DPE.Core.Model.Models;
using DPE.Core.IRepository.UnitOfWork;

namespace DPE.Core.Repository
{
    /// <summary>
    /// RoleRepository
    /// </summary>	
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
