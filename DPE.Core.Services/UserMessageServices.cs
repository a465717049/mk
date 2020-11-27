using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class UserMessageServices : BaseServices<UserMessage>, IUserMessageServices
    {
        private readonly IUserMessageRepository _dal;
        public UserMessageServices(IUserMessageRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}