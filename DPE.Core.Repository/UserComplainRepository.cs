using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
    public class UserComplainRepository : BaseRepository<UserComplaint>, IUserComplaintRepository
    {
        public UserComplainRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
