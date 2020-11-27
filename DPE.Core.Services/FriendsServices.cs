using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class FriendsServices : BaseServices<Friends>, IFriendsServices
    {
        private readonly IFriendsRepository _dal;
        public FriendsServices(IFriendsRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<List<Friends>> GetFriends(long uid)
        {
            SugarParameter[] par = new SugarParameter[] {
                new SugarParameter("uId",uid)
            };

            return await _dal.QueryProc("Sp_SplitUser", par);
        }

        public async Task<Friends> GetOne(long uid)
        {
            return (await GetFriends(uid)).FirstOrDefault();
        }
    }
}
