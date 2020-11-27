using DPE.Core.IRepository;
using DPE.Core.IRepository.UnitOfWork;
using DPE.Core.Model.Models;
using DPE.Core.Repository.Base;

namespace DPE.Core.Repository
{
	/// <summary>
	/// DPEexchangeRepository
	/// </summary>
    public class DPEexchangeRepository : BaseRepository<DPEexchange>, IDPEexchangeRepository
    {
        public DPEexchangeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}