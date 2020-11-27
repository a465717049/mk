using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class ManureServices : BaseServices<Manure>, IManureServices
    {
        private readonly IManureRepository _dal;
        public ManureServices(IManureRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<Manure> GetOne(long uid)
        {
            Manure model = null;
            List<Manure> list = await base.Query(c => c.uID == uid);
            if (list != null && list.Count > 0)
            {
                model = list.FirstOrDefault();
            }
            return model;
        }

        public async Task<bool> UpdateManure(Manure manure)
        {
            return await base.Update(manure);
        }
    }
}