using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class AdminReplyServices : BaseServices<AdminReply>, IAdminReplyServices
    {
        private readonly IAdminReplyRepository _dal;
        public AdminReplyServices(IAdminReplyRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<int> AddAdminReply(AdminReply adminReply)
        {
            return await base.Add(adminReply);
        }

        public async Task<bool> DeleteAdminReply(AdminReply adminReply)
        {
            return await base.Delete(adminReply);
        }

        public async Task<List<AdminReply>> GetAdminReplies(int ufbId)
        {
            return await base.Query(c=>c.UserFeedbackId == ufbId);
        }

        public async Task<bool> UpdateAdminReply(AdminReply adminReply)
        {
            return await base.Update(adminReply);
        }
    }
}