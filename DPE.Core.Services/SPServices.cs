using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using DPE.Core.IServices;
using DPE.Core.IRepository;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace DPE.Core.Services
{
    public partial class SPServices : BaseServices<SP>, ISPServices
    {
        private readonly ISPRepository _dal;
        public SPServices(ISPRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

     
    }
}