using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using DPE.Core.Services.BASE;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class EMailServices : BaseServices<EMail>, IEMailServices
    {
        private readonly IEMailRepository _dal;
        public EMailServices(IEMailRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }


        public async Task<List<EMail>> GetEmailListInfo(long fromUid)
        {
            //var list = base.QuerySql("select a.id,a.title,a.context as 'content' ,ISNULL(b.status,0) as 'status'," +
            //   "a.createTime ,'ϵͳ' as 'source' from EMail a  left join UserEmail b on a.id=b.eid  where a.fromUID="+ fromUid);

            var userList = await base.Query(a => a.fromUID == fromUid );
            return userList;
        }


        public async Task<EMail> GetEmailInfo(int id)
        {
            //var list = base.QuerySql("select a.id,a.title,a.context as 'content' ,ISNULL(b.status,0) as 'status'," +
            //   "a.createTime ,'ϵͳ' as 'source' from EMail a  left join UserEmail b on a.id=b.eid  where a.fromUID="+ fromUid);

            var userList = await base.QueryById(id);
            return userList;
        }



    }
}