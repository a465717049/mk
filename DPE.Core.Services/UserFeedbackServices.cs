using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class UserFeedbackServices : BaseServices<UserFeedback>, IUserFeedbackServices
    {
        private readonly IUserFeedbackRepository _dal;
        public UserFeedbackServices(IUserFeedbackRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<bool> DelUserFeedBack(int id)
        {
            return await base.DeleteById(id);
        }

        public async Task<List<UserFeedback>> GetUserFeedback(long uid)
        {
            return await base.Query(c => c.uId == uid);
        }

        public async Task<UserFeedback> GetUserFeedback(int id)
        {
            return await base.QueryById(id);
        }

        public async Task<int> InsertUserFeedBack(UserFeedback userFeedback)
        {
            return await base.Add(userFeedback);
        }

        public async Task<bool> UpdateUserFeedBack(UserFeedback userFeedback)
        {
            return await base.Update(userFeedback);
        }

    }
}