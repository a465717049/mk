using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
   public class SearchRelationServices : BaseServices<SearchRelation>, ISearchRelationServices
    {
        private readonly ISearchRelationRepository _dal;
        public SearchRelationServices(ISearchRelationRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }


        public async Task<List<SearchRelation>> SPSearchRelation(long parentId)
        {

            List<SearchRelation> list = new List<SearchRelation>();

            SugarParameter[] parameters = new SugarParameter[]
         {
                 new SugarParameter("@parentId",parentId)
         };

            var epentity = await _dal.QueryProc("SP_SearchRelation", parameters);
            return epentity;
        }


        public async Task<List<SearchRelation>> SPSearchFamily(long parentId,long uid)
        {

            List<SearchRelation> list = new List<SearchRelation>();

            SugarParameter[] parameters = new SugarParameter[]
         {
                 new SugarParameter("@parentId",parentId),
                 new SugarParameter("@uid",uid)
         };

            var epentity = await _dal.QueryProc("SP_SearchFamily", parameters);
            return epentity;
        }

    }
}
