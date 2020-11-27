using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
    public class FriendsRepository : BaseRepository<Friends>, IFriendsRepository
    {
        public FriendsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
