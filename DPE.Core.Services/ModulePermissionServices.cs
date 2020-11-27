using DPE.Core.Services.BASE;
using DPE.Core.Model.Models;
using DPE.Core.IRepository;
using DPE.Core.IServices;

namespace DPE.Core.Services
{	
	/// <summary>
	/// ModulePermissionServices
	/// </summary>	
	public class ModulePermissionServices : BaseServices<ModulePermission>, IModulePermissionServices
    {
	
        IModulePermissionRepository _dal;
        public ModulePermissionServices(IModulePermissionRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
