using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class BuyDPEHistoryServices : BaseServices<BuyDPEHistory>, IBuyDPEHistoryServices
    {
        private readonly IBuyDPEHistoryRepository _dal;
        public BuyDPEHistoryServices(IBuyDPEHistoryRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}