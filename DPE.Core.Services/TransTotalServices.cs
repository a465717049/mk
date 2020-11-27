using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class TransTotalServices : BaseServices<TransTotal>, ITransTotalServices
    {
        private readonly ITransTotalRepository _dal;
        public TransTotalServices(ITransTotalRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}