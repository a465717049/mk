using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class sysdiagramsServices : BaseServices<sysdiagrams>, IsysdiagramsServices
    {
        private readonly IsysdiagramsRepository _dal;
        public sysdiagramsServices(IsysdiagramsRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}