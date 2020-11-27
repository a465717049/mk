using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class DPERecordsServices : BaseServices<DPERecords>, IDPERecordsServices
    {
        private readonly IDPERecordsRepository _dal;
        public DPERecordsServices(IDPERecordsRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}