using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using SqlSugar;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class ShopListServices : BaseServices<ShopList>, IShopListServices
    {
        private readonly IShopListRepository _dal;
        public ShopListServices(IShopListRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<List<ShopList>> GetShopList(long uid)
        {
            var userList = await base.Query();
            return userList;
        }

        public async Task<DataTable> GetautoProcessamount(long userID, decimal money)
        {
            SugarParameter[] parameters = new SugarParameter[2];
            parameters[0] = new SugarParameter("@userID", userID);
            parameters[1] = new SugarParameter("@money", money);
            return await _dal.QueryProcTable("autoProcessamount", parameters);
        }

       
    }
}