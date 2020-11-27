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
    public partial class NewsServices : BaseServices<News>, INewsServices
    {
        private readonly INewsRepository _dal;
        public NewsServices(INewsRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }



    }
}