using DPE.Core.Repository.Base;
using DPE.Core.Model.Models;
using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;

namespace DPE.Core.Repository
{
    /// <summary>
    /// ModuleRepository
    /// </summary>	
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        public ModuleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
