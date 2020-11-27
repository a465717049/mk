using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class QuestionServices : BaseServices<Question>, IQuestionServices
    {
        private readonly IQuestionRepository _dal;
        public QuestionServices(IQuestionRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<int> AddQuestion(Question question)
        {
            return await base.Add(question);
        }

        public async Task<bool> DeleteQuestion(int id)
        {
            return await base.DeleteById(id);
        }

        public async Task<Question> GetQuestionById(int id)
        {
            return await base.QueryById(id);
        }

        public async Task<List<Question>> GetQuestions()
        {
            return await base.Query();
        }
    }
}