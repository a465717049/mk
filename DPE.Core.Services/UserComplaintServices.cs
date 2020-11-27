using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DPE.Core.Services
{
    public class UserComplaintServices : BaseServices<UserComplaint>, IUserComplaintServices
    {
        private readonly IUserComplaintRepository _dal;
        public UserComplaintServices(IUserComplaintRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}
