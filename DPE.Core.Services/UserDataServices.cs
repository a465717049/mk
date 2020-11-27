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
    public partial class UserDataServices : BaseServices<UserData>, IUserDataServices
    {
        private readonly IUserDataRepository _dal;
        public UserDataServices(IUserDataRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
        public async Task<UserInfoViewModel> GetUserData(long uid)
        {

            var list = await base.QueryMuch<UserData, SP, EP, UserInfoViewModel>((a, b, c) => new object[] {
              JoinType.Left,a.uID==b.uID,
              JoinType.Left,a.uID==c.uID
            }, (a, b, c) => new UserInfoViewModel
            {
                nickname=a.uID.ToString()
            },(a,b,c)=>a.uID==uid);
            var model = list[0];
            return model;

        }
    }
}