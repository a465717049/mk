using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class DownExcelRecordServices : BaseServices<DownExcelRecord>, IDownExcelRecordServices
    {
        private readonly IDownExcelRecordRepository _dal;
        public DownExcelRecordServices(IDownExcelRecordRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}