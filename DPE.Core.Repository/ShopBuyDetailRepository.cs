using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
    /// <summary>
    /// ShopListRepository
    /// </summary>
    public class ShopBuyDetailRepository : BaseRepository<ShopBuyDetail>, IShopBuyDetailRepository
    {
        public ShopBuyDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}