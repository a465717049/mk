using DPE.Core.IServices.BASE;
using DPE.Core.Model;
using DPE.Core.Model.Models;
using DPE.Core.Model.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// IUserInfoServices
	/// </summary>	
    public interface IUserInfoServices :IBaseServices<UserInfo>
	{
		Task<UserInfo> GetUserInfo(long uid);

		Task<PageModel<UserInfo>> GetAllUserInfo(int index, int pagesize,string key, string orderby);

		Task<DataSet> GetSPSearchServic(string beginTime,string endTime);
	}
}