using DPE.Core.IServices.BASE;
using DPE.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.IServices
{	
	/// <summary>
	/// IQuestionServices
	/// </summary>	
    public interface IQuestionServices :IBaseServices<Question>
	{
		Task<List<Question>> GetQuestions();

		Task<Question> GetQuestionById(int id);

		Task<int> AddQuestion(Question question);

		Task<bool> DeleteQuestion(int id);
    }
}