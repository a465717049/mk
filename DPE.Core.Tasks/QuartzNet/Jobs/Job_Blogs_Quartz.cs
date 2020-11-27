using DPE.Core.IServices;
using Quartz;
using System;
using System.Threading.Tasks;

/// <summary>
/// 这里要注意下，命名空间和程序集是一样的，不然反射不到
/// </summary>
namespace DPE.Core.Tasks
{
    public class Job_Blogs_Quartz : JobBase, IJob
    {
        private readonly ISysUserInfoServices _sysUserInfoServices;
        private readonly ITasksQzServices _tasksQzServices;

        public Job_Blogs_Quartz(ISysUserInfoServices sysUserInfoServices, ITasksQzServices tasksQzServices)
        {
            _sysUserInfoServices = sysUserInfoServices;
            _tasksQzServices = tasksQzServices;
        }
        public async Task Execute(IJobExecutionContext context)
        {

            //var param = context.MergedJobDataMap;
            // 可以直接获取 JobDetail 的值
            var jobKey = context.JobDetail.Key;
            var jobId = jobKey.Name;

            var executeLog = await ExecuteJob(context, async () => await Run(context, jobId.ObjToInt()));

            // 也可以通过数据库配置，获取传递过来的参数
            JobDataMap data = context.JobDetail.JobDataMap;
            //int jobId = data.GetInt("JobParam");

            //var model = await _tasksQzServices.QueryById(jobId);
            //if (model != null)
            //{
            //    model.RunTimes += 1;
            //    model.Remark += $"{executeLog}<br />";
            //    await _tasksQzServices.Update(model);
            //}

        }
        public async Task Run(IJobExecutionContext context, int jobid)
        {
            var list = await _sysUserInfoServices.Query();

            if (jobid > 0)
            {
                var model = await _tasksQzServices.QueryById(jobid);
                if (model != null)
                {
                    model.RunTimes += 1;
                    model.Remark += $"【{DateTime.Now}】执行任务【Id：{context.JobDetail.Key.Name}，组别：{context.JobDetail.Key.Group}】【执行成功】<br>";
                    await _tasksQzServices.Update(model);
                }
            }

            await Console.Out.WriteLineAsync("博客总数量" + list.Count.ToString());
        }
    }



}
