using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// ShopSkuDetailRepository
	/// </summary>
    public class ShopSkuDetailRepository : BaseRepository<ShopSkuDetail>, IShopSkuDetailRepository
    {
        public ShopSkuDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}