using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class ShopBuyDetailSerivces : BaseServices<ShopBuyDetail>, IShopBuyDetailSerivces
    {
        private readonly IShopBuyDetailRepository _dal;

        public ShopBuyDetailSerivces(IShopBuyDetailRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }


    }
}