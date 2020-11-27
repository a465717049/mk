using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class DPEexchangeServices : BaseServices<DPEexchange>, IDPEexchangeServices
    {
        private readonly IDPEexchangeRepository _dal;
        public DPEexchangeServices(IDPEexchangeRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}