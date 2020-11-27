using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class RPServices : BaseServices<RP>, DPE.Core.IServices.IRPServices
    {
        private readonly IRPRepository _dal;
        public RPServices(IRPRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}