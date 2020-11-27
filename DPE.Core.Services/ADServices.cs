using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class ADServices : BaseServices<AD>, IADServices
    {
        private readonly IADRepository _dal;
        private readonly IADexchangeRepository _iadexchangerepository;
        public ADServices(IADRepository dal, IADexchangeRepository iadexchangerepository)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _iadexchangerepository = iadexchangerepository;
        }

    }
}