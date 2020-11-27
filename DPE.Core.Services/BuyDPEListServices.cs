using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class BuyDPEListServices : BaseServices<BuyDPEList>, IBuyDPEListServices
    {
        private readonly IBuyDPEListRepository _dal;
        public BuyDPEListServices(IBuyDPEListRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}