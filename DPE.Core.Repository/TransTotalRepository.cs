using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// TransTotalRepository
	/// </summary>
    public class TransTotalRepository : BaseRepository<TransTotal>, ITransTotalRepository
    {
        public TransTotalRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}