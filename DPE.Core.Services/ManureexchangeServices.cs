using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class ManureexchangeServices : BaseServices<Manureexchange>, IManureexchangeServices
    {
        private readonly IManureexchangeRepository _dal;
        public ManureexchangeServices(IManureexchangeRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<int> AddManureExchange(Manureexchange manureexchange)
        {
            return await base.Add(manureexchange);
        }

        public async Task<List<Manureexchange>> GetManureExchange(long uid)
        {
            var userList = await base.Query(a => a.uID == uid);

            return userList;
        }
    }
}