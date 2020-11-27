using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
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
    }
}