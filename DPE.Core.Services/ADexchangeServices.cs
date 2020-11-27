using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class ADexchangeServices : BaseServices<ADexchange>, IADexchangeServices
    {
        private readonly IADexchangeRepository _dal;
        public ADexchangeServices(IADexchangeRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}