using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// IUserFeedbackServices
	/// </summary>	
    public interface IUserFeedbackServices :IBaseServices<UserFeedback>
	{
		Task<List<UserFeedback>> GetUserFeedback(long uid);

		Task<UserFeedback> GetUserFeedback(int id);

		Task<bool> UpdateUserFeedBack(UserFeedback userFeedback);


		Task<int> InsertUserFeedBack(UserFeedback userFeedback);

		Task<bool> DelUserFeedBack(int id);
	}
}