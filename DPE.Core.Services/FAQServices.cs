using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class FAQServices : BaseServices<FAQ>, IFAQServices
    {
        private readonly IFAQRepository _dal;
        public FAQServices(IFAQRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}