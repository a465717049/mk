using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using DPE.Core.Services.BASE;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class ShopRoleServices : BaseServices<ShopRole>, IShopRoleServices
    {
        private readonly IShopRoleRepository _dal;
        public ShopRoleServices(IShopRoleRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }



    }
}