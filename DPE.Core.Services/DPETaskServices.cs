using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class DPETaskServices : BaseServices<DPETask>, IDPETaskServices
    {
        private readonly IDPETaskRepository _dal;
        public DPETaskServices(IDPETaskRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}