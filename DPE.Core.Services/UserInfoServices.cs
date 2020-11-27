using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using DPE.Core.Services.BASE;
using SqlSugar;
using System.Collections.Generic;
using System.Data;
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

        public async Task<PageModel<UserInfo>> GetAllUserInfo(int index,int pagesize,string key,string orderby)
        {
            if (string.IsNullOrEmpty(key))
            {
                return (await _dal.QueryPage(null, index, pagesize, orderby));
            }
            else
            {
                return (await _dal.QueryPage(x=>x.uNickName.Contains(key) || x.uID.ToString().Contains(key), index, pagesize, orderby));
            }
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