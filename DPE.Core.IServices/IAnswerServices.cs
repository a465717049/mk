using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// IAnswerServices
	/// </summary>	
    public interface IAnswerServices :IBaseServices<Answer>
	{


    
		Task<int> AddAnswer(Answer answer);

		Task<bool> UpdateAnswer(Answer answer);

		Task<bool> DeleteAnswer(Answer answer);

		Task<List<Answer>> GetAnswer(long uid);

		Task<Answer> GetAnswerByQid(long uid,int qid);

		Task<List<Answer>> GetAnswerByQid(int qid);

	}
}