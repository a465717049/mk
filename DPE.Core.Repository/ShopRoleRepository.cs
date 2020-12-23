using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
    /// <summary>
    /// ShopRoleRepository
    /// </summary>
    public class ShopRoleRepository : BaseRepository<ShopRole>, IShopRoleRepository
    {
        public ShopRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}