using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using DPE.Core.Services.BASE;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class UserAppleConsumeServices : BaseServices<UserAppleConsume>, IUserAppleConsumeServices
    {
        private readonly IUserAppleConsumeRepository _dal;
        public UserAppleConsumeServices(IUserAppleConsumeRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<UserAppleConsume> GetUserAppleConsumeByType(long uid, int type)
        {
            UserAppleConsume model = new UserAppleConsume();

            var list = await base.Query(c => c.uID == uid && c.sType == type);

            if (list != null) 
            {
                model = list.FirstOrDefault();
            }

            return model;
        }
    }
}