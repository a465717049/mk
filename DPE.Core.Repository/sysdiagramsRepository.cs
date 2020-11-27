using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// sysdiagramsRepository
	/// </summary>
    public class sysdiagramsRepository : BaseRepository<sysdiagrams>, IsysdiagramsRepository
    {
        public sysdiagramsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}