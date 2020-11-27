using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Linq;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class StockServices : BaseServices<Stock>, IStockServices
    {
        private readonly IStockRepository _dal;
        public StockServices(IStockRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<Stock> GetStock()
        {
            return (await base.Query(null, c => c.createTime,false)).FirstOrDefault();
        }
    }
}