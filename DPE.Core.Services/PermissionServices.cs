using DPE.Core.Services.BASE;
using DPE.Core.Model.Models;
using DPE.Core.IRepository;
using DPE.Core.IServices;

namespace DPE.Core.Services
{	
	/// <summary>
	/// PermissionServices
	/// </summary>	
	public class PermissionServices : BaseServices<Permission>, IPermissionServices
    {
	
        IPermissionRepository _dal;
        public PermissionServices(IPermissionRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
