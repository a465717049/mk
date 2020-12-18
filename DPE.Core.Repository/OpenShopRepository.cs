using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
    /// <summary>
    /// EPexchangeRepository
    /// </summary>
    public class OpenShopRepository : BaseRepository<OpenShop>, IOpenShopRepository
    {
        public OpenShopRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}