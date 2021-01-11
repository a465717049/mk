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
    public partial class ShopSkuDetailServices : BaseServices<ShopSkuDetail>, IShopSkuDetailServices
    {
        private readonly IShopSkuDetailRepository _dal;
        public ShopSkuDetailServices(IShopSkuDetailRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<List<ShopSkuDetail>> GetShopSkuDetail(long uid)
        {
            var userList = await base.Query();
            return userList;
        }
       
    }
}