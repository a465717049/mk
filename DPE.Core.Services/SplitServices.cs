using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class SplitServices : BaseServices<Split>, ISplitServices
    {
        private readonly ISplitRepository _dal;
        public SplitServices(ISplitRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<Split> GetOneByUId(long uid)
        {
            Split model = null;
            var list = (await base.Query(c => c.uID == uid));
            if (list != null)
            {
                model = list.FirstOrDefault();
            }
            return model;
        }

        public async Task<bool> UpdateSplit(Split split)
        {
            return (await base.Update(split));
        }
    }
}