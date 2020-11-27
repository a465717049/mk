
using DPE.Core.IRepository;
using DPE.Core.IServices;
using DPE.Core.Model.Models;
using DPE.Core.Services.BASE;

namespace DPE.Core.Services
{
    public partial class TasksQzServices : BaseServices<TasksQz>, ITasksQzServices
    {
        ITasksQzRepository _dal;
        public TasksQzServices(ITasksQzRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}
                    