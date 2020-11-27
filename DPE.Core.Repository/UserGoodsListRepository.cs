using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// UserGoodsListRepository
	/// </summary>
    public class UserGoodsListRepository : BaseRepository<UserGoodsList>, IUserGoodsListRepository
    {
        public UserGoodsListRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}