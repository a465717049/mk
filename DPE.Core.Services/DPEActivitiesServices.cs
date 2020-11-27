using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class DPEActivitiesServices : BaseServices<DPEActivities>, IDPEActivitiesServices
    {
        private readonly IDPEActivitiesRepository _dal;
        public DPEActivitiesServices(IDPEActivitiesRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}
