using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DPE.Core.Services
{
    public partial class UserGoodsListServices : BaseServices<UserGoodsList>, IUserGoodsListServices
    {
        private readonly IUserGoodsListRepository _dal;
        public UserGoodsListServices(IUserGoodsListRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
        public async Task<List<UserGoodsList>> GetUserGoodsList(long uid)
        {
            var userList = await base.Query(x=>x.uID == uid);
            return userList;
        }

        public async Task<List<int>> GetUserShopOwnNum(long uid, long shopID)
        {
            List<int> list = new List<int>();
            var result =await _dal.Query(x => x.shopID == shopID && x.uID == uid);

            if (result.Count > 0) 
            {
                foreach (var s in result) 
                {
                    list.Add(s.num.ObjToInt());
                }
            }
            return list;

        }

      

    }
}