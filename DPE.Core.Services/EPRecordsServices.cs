using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using DPE.Core.Services.BASE;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class EPRecordsServices : BaseServices<EPRecords>, IEPRecordsServices
    {
        private readonly IEPRecordsRepository _dal;
        public EPRecordsServices(IEPRecordsRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

       
    }
}