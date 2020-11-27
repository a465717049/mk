using DPE.Core.Common.Helper;
using DPE.Core.IServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DPE.Core.Tasks
{
    public class Job1TimedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ISysUserInfoServices _sysUserInfoServices;

        // 这里可以注入
        public Job1TimedService(ISysUserInfoServices sysUserInfoServices)
        {
            _sysUserInfoServices = sysUserInfoServices;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Job 1 is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(60 * 60));//一个小时

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            try
            {
                var users = await _sysUserInfoServices.Query();
                Console.WriteLine($"Job 1 启动成功，获取用户数量为:{users.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
            }

            ConsoleHelper.WriteSuccessLine($"Job 1： {DateTime.Now}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Job 1 is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
