using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
    public class ModulePermissionRepository : BaseRepository<ModulePermission>, IModulePermissionRepository
    {
        public ModulePermissionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
