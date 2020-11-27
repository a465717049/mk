using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// BannerRepository
	/// </summary>
    public class BannerRepository : BaseRepository<Banner>, IBannerRepository
    {
        public BannerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}