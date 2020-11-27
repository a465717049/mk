using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class ProfitListServices : BaseServices<ProfitList>, IProfitListServices
    {
        private readonly IProfitListRepository _dal;
        public ProfitListServices(IProfitListRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}