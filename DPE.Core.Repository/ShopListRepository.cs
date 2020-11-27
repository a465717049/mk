using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// ShopListRepository
	/// </summary>
    public class ShopListRepository : BaseRepository<ShopList>, IShopListRepository
    {
        public ShopListRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}