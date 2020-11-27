using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class IRPServices : BaseServices<IRP>, IIRPServices
    {
        private readonly IIRPRepository _dal;
        private readonly IIRPexchangeRepository _iirpexchangerepository;
        public IRPServices(IIRPRepository dal, IIRPexchangeRepository iirpexchangerepository)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _iirpexchangerepository = iirpexchangerepository;
        }


        public async Task<bool> SetAdSignIn(long uid)
        {
            var user = await _dal.QueryById(uid);
            bool isfig = false;
            if (user == null)
            {
                IRP model = new IRP();
                model.amount = 1;
                model.uID = uid;
                isfig = await _dal.Add(model) > 0 ? true : false;
                await _iirpexchangerepository.Add(new IRPexchange() { amount = model.amount, uID = model.uID, fromID = model.uID, createTime = DateTime.Now, lastTotal = 0, stype = 3, remark = "ºžµ½" });
            }
            else
            {
                var tmpisfig = await _iirpexchangerepository.Query(x => x.uID == uid && x.stype == 3 && x.createTime.Value.Date.Equals(DateTime.Now.Date));
                if (tmpisfig.Count > 0)
                {
                    return false;
                }
                await _iirpexchangerepository.Add(new IRPexchange() { amount = user.amount + 1, uID = user.uID, fromID = user.uID, createTime = DateTime.Now, lastTotal = user.amount, stype = 3, remark = "ºžµ½" });
                user.amount = user.amount + 1;
                isfig = await _dal.Update(user);
            }
            return isfig;
        }
    }
}