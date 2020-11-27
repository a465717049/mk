using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// UserEmailRepository
	/// </summary>
    public class UserEmailRepository : BaseRepository<UserEmail>, IUserEmailRepository
    {
        public UserEmailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}