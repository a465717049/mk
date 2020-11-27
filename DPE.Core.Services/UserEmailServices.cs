using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class UserEmailServices : BaseServices<UserEmail>, IUserEmailServices
    {
        private readonly IUserEmailRepository _dal;
        private readonly IEMailServices _email;
        public UserEmailServices(IUserEmailRepository dal, IEMailServices email)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _email = email;
        }


        public async Task<bool> DeleteEmail(int id,long uid)
        {
            var emailmodel = await _dal.Query(x => x.eid == id && x.uId==uid);

            var email = emailmodel[0];
            email.isDelete = 1;
            bool isfig = await _dal.Update(email);

            return isfig;
        }

        public async Task<bool> DeleteEmailAll(long uid)
        {
           
           var result= await base.QuerySql("update UserEmail set isDelete=1 where uId= " + uid);

            return true;
        }

        public async Task<int> ckUserEmailStatus(long uid, long eId)
        {
            var emailmodel = await _dal.Query(x => x.eid == eId && x.uId==uid);
            if (emailmodel.Count != 0)
            {
                return emailmodel[0].status.ObjToInt();
            }
            else 
            {
                return 0;
            }
            
        }

       

    }
}