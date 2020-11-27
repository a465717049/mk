using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class UserTaskServices : BaseServices<UserTask>, IUserTaskServices
    {
        private readonly IUserTaskRepository _dal;
        public UserTaskServices(IUserTaskRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}