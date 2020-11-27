using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    
    public partial class DisposeFeedbackServices : BaseServices<DisposeFeedback>, IDisposeFeedbackServices
    {
        private readonly IDisposeFeedbackRepository _dal;
        public DisposeFeedbackServices(IDisposeFeedbackRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}