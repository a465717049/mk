using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DPE.Core.IServices
{	
	/// <summary>
	/// IUserGoodsListServices
	/// </summary>	
    public interface IUserGoodsListServices :IBaseServices<UserGoodsList>
	{

		Task<List<UserGoodsList>> GetUserGoodsList(long uid);

		Task<List<int>> GetUserShopOwnNum(long uid,long shopID);
	}
}