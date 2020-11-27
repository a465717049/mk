using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class IIRPexchangeServices : BaseServices<IRPexchange>, IServices.IIRPexchangeServices
    {
        private readonly IIRPexchangeRepository _dal;
        public IIRPexchangeServices(IIRPexchangeRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}