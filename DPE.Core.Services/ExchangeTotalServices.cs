using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class ExchangeTotalServices : BaseServices<ExchangeTotal>, IExchangeTotalServices
    {
        private readonly IExchangeTotalRepository _dal;
        public ExchangeTotalServices(IExchangeTotalRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}