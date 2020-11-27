using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class DPEMessageServices : BaseServices<DPEMessage>, IDPEMessageServices
    {
        private readonly IDPEMessageRepository _dal;
        public DPEMessageServices(IDPEMessageRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}