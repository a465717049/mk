using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class DPEProfitServices : BaseServices<DPEProfit>, IDPEProfitServices
    {
        private readonly IDPEProfitRepository _dal;
        public DPEProfitServices(IDPEProfitRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}