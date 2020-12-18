using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class OpenShopServices : BaseServices<OpenShop>, IOpenShopServices
    {
        private readonly IOpenShopRepository _dal;
        public OpenShopServices(IOpenShopRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}