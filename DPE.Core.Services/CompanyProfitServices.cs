using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class CompanyProfitServices : BaseServices<CompanyProfit>, ICompanyProfitServices
    {
        private readonly ICompanyProfitRepository _dal;
        public CompanyProfitServices(ICompanyProfitRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}