using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using DPE.Core.Services.BASE;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class UserActivitiesServices : BaseServices<UserActivities>, IUserActivitiesServices
    {
        private readonly IUserActivitiesRepository _dal;
        public UserActivitiesServices(IUserActivitiesRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<int> CkUserActivities(long uid, int actId) 
        {
            int isfig = 0;
            var result = await _dal.Query(x => x.uid == uid && x.actID == actId);
            if (result.Count <= 0 && uid!=0 )
            {
                await _dal.Add(new UserActivities { actID = actId, uid = uid, status = 0 });
            }
            else 
            {
                isfig = result[0].status.ObjToInt();
            }

            return isfig;
        }

    }
}
