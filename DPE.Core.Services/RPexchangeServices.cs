using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using SqlSugar;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class RPexchangeServices : BaseServices<RPexchange>, IRPexchangeServices
    {
        private readonly IRPexchangeRepository _dal;
        public RPexchangeServices(IRPexchangeRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
        public async Task<MessageModel<string>> ProcessTransformToUser(long uid, string type, decimal amount, long touid)
        {
            MessageModel<string> result = new MessageModel<string>();

            SugarParameter[] parameters = new SugarParameter[]
            {
                 new SugarParameter("@uid",uid),
                new SugarParameter("@type",type),
                new SugarParameter("@amount",amount),
                  new SugarParameter("@touid",touid)  
        };

            var epentity = (await _dal.QueryProc("Process_TransformToUser", parameters)).FirstOrDefault();

            if (epentity?.amount.ObjToDecimal() == 0)
            {
                result.code = 0;
                result.success = true;
            }
            else
            {
                result.code = Convert.ToInt32(epentity?.amount.ObjToDecimal());
                result.success = false;
            }

            return result;

        }

    }
}