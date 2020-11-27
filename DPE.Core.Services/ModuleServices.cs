using DPE.Core.Services.BASE;
using DPE.Core.Model.Models;
using DPE.Core.IRepository;
using DPE.Core.IServices;

namespace DPE.Core.Services
{	
	/// <summary>
	/// ModuleServices
	/// </summary>	
	public class ModuleServices : BaseServices<Module>, IModuleServices
    {
	
        IModuleRepository _dal;
        public ModuleServices(IModuleRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
