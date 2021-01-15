using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// ShopSkuRepository
	/// </summary>
    public class ShopSkuRepository : BaseRepository<ShopSku>, IShopSkuRepository
    {
        public ShopSkuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}