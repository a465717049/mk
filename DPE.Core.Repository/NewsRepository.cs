
using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;


namespace DPE.Core.Repository
{
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        public NewsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}