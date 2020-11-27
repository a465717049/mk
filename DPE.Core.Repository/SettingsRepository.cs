using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// SettingsRepository
	/// </summary>
    public class SettingsRepository : BaseRepository<Settings>, ISettingsRepository
    {
        public SettingsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}