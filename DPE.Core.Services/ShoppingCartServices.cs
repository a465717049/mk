using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class ShoppingCartServices : BaseServices<ShoppingCart>, IShoppingCartSerivces
    {
        private readonly IShoppingCartRepository _dal;
        public ShoppingCartServices(IShoppingCartRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<List<ShoppingCart>> GetShopList(long uid)
        {
            var userList = await base.Query();
            return userList;
        }
    }
}