using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Linq;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class SettingsServices : BaseServices<Settings>, ISettingsServices
    {
        private readonly ISettingsRepository _dal;
        public SettingsServices(ISettingsRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<Settings> GetSettings()
        {
            return (await base.Query()).FirstOrDefault();
        }
    }
}