using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// UserActivitiesRepository
	/// </summary>
    public class UserActivitiesRepository : BaseRepository<UserActivities>, IUserActivitiesRepository
    {
        public UserActivitiesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}