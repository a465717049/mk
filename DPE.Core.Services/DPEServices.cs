using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class DPEServices : BaseServices<DPE.Core.Model.Models.DPE>, IDPEServices
    {
        private readonly IDPERepository _dal;
        public DPEServices(IDPERepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        /// <summary>
        /// 获取DPE数据
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<Model.Models.DPE> GetDPEData(long uid)
        {
            Model.Models.DPE model = null;
            List<Model.Models.DPE> list = await base.Query(c => c.uID == uid);
            if (list != null && list.Count > 0)
            {
                model = list.FirstOrDefault();
            }
            return model;
        }
    }
}