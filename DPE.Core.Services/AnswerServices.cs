using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;
using StackExchange.Redis;
using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DPE.Core.Services
{
    public partial class AnswerServices : BaseServices<Answer>, IAnswerServices
    {
        private readonly IAnswerRepository _dal;
        public AnswerServices(IAnswerRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }


        public async Task<int> AddAnswer(Answer answer)
        {
            return await base.Add(answer);
        }

        public async Task<bool> DeleteAnswer(Answer answer)
        {
            return await base.Delete(answer);
        }

        public async Task<List<Answer>> GetAnswer(long uid)
        {
            return await base.Query(c => c.uID == uid);
        }

        public async Task<Answer> GetAnswerByQid(long uid, int qid)
        {
            Answer model = null;
            var data = await base.Query(c => c.uID == uid && c.qID == qid);
            if (data != null && data.Count>0)
            {
                model = data.FirstOrDefault();
            }

            return model;
        }

        public async Task<List<Answer>> GetAnswerByQid(int qid)
        {
            return await base.Query(c => c.qID == qid);
        }

        public async Task<bool> UpdateAnswer(Answer answer)
        {
            return await base.Update(answer);
        }
    }
}