using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using DPE.Core.Services.BASE;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class UserInfoServices : BaseServices<UserInfo>, IUserInfoServices
    {
        private readonly IUserInfoRepository _dal;
        public UserInfoServices(IUserInfoRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<UserInfo> GetUserInfo(long uid)
        {
            var user = (await _dal.Query(x=>x.uID==uid))[0];
           
            return user;
        }

        public async Task<PageModel<UserInfo>> GetAllUserInfo(int index, int pagesize, string key, string utid,
            string ulevel, string uhonur, string ustatus, string startdate, string enddate, string orderby)
        {

            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();


            if (string.IsNullOrEmpty(startdate) || string.IsNullOrWhiteSpace(startdate))
            {
                dt1 = Convert.ToDateTime("1999-01-01");
            }
            else
            {
                dt1 = Convert.ToDateTime(startdate);
            }


            if (string.IsNullOrEmpty(enddate) || string.IsNullOrWhiteSpace(enddate))
            {
                dt2 = DateTime.Now.AddDays(1);
            }
            else
            {
                dt2 = Convert.ToDateTime(enddate).AddDays(1);
            }

            Expression<Func<UserInfo, bool>> querry = PredicateExtensions.True<UserInfo>();
            if (!string.IsNullOrEmpty(utid))
            {
                querry = querry.And(x=>x.tid.ToString().Contains(utid));
            }
            if (!string.IsNullOrEmpty(ulevel))
            {
                querry = querry.And(x => x.honorLevel.ToString()==ulevel);
            }
            if (!string.IsNullOrEmpty(uhonur))
            {
                querry = querry.And(x => x.uStatus.ToString() == uhonur);
            }
            if (!string.IsNullOrEmpty(key))
            {
                querry = querry.And(x => x.uNickName.ToString().Contains(key)    || x.UrealName.ToString().Contains(key) || x.uID.ToString().Contains(key) || x.userphone.ToString().Contains(key));
            }
            if (!string.IsNullOrEmpty(ustatus))
            {
                querry = querry.And(x => x.isDelete.ToString()== ustatus);
            }

                querry = querry.And(x => x.uCreateTime >= dt1 && x.uCreateTime <= dt2);
   
            var result = await _dal.QueryPage(querry
  , index, pagesize, orderby);
    

            return result;
            //if (string.IsNullOrEmpty(key))
            //{
            //    return (await _dal.QueryPage(null, index, pagesize, orderby));
            //}
            //else
            //{
            //    return (await _dal.QueryPage(x=>x.uNickName.Contains(key) || x.uID.ToString().Contains(key), index, pagesize, orderby));
                
            //}
        }

        public async  Task<DataSet> GetSPSearchServic(string beginTime, string endTime)
        {
            SugarParameter[] parameters = new SugarParameter[2];
            parameters[0] = new SugarParameter("@beginTime", beginTime);
            parameters[1] = new SugarParameter("@endTime", endTime);
            var result = await _dal.QueryProcDataSet("SP_SearchService", parameters);

            return  result;
        }

        
    }
}