using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class SplitRecordsServices : BaseServices<SplitRecords>, ISplitRecordsServices
    {
        private readonly ISplitRecordsRepository _dal;
        public SplitRecordsServices(ISplitRecordsRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}