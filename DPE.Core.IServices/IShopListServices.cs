using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
namespace DPE.Core.IServices
{	
	/// <summary>
	/// IShopListServices
	/// </summary>	
    public interface IShopListServices :IBaseServices<ShopList>
	{

		Task<List<ShopList>> GetShopList(long uid);

		//¸üÐÂ½áËã
		Task<DataTable> GetautoProcessamount(long userID,decimal money);
	}
}