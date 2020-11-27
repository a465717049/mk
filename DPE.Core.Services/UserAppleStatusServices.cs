using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class UserAppleStatusServices : BaseServices<UserAppleStatus>, IUserAppleStatusServices
    {
        private readonly IUserAppleStatusRepository _dal;
        public UserAppleStatusServices(IUserAppleStatusRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<int> AddUserAppleStatus(UserAppleStatus userAppleStatus)
        {
            return await base.Add(userAppleStatus);
        }

        public async Task<UserAppleStatus> GetUserAppleStatus(long uid)
        {
            UserAppleStatus model = null;
            List<UserAppleStatus> list = await base.Query(c => c.uID == uid);
            if (list != null && list.Count >0)
            {
                model = list.FirstOrDefault();
            }
            return model;
        }

        public async Task<bool> UpdateUserAppleStatus(UserAppleStatus userAppleStatus)
        {
            return await base.Update(userAppleStatus);
        }
    }
}