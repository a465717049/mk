using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class SPexchangeServices : BaseServices<SPexchange>, ISPexchangeServices
    {
        private readonly ISPexchangeRepository _dal;
        public SPexchangeServices(ISPexchangeRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
        public async Task<List<SPexchange>> GetSPExchange(long uid)
        {
            var userList = await base.Query(a => a.uID == uid);

            return userList;
        }
    }
}