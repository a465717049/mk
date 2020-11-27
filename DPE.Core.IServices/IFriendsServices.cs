using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DPE.Core.IServices
{
    public interface IFriendsServices : IBaseServices<Friends>
    {
        Task<List<Friends>> GetFriends(long uid);

        Task<Friends> GetOne(long uid);
    }
}
