using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// DPEActivitiesRepository
	/// </summary>
    public class DPEActivitiesRepository : BaseRepository<DPEActivities>, IDPEActivitiesRepository
    {
        public DPEActivitiesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}