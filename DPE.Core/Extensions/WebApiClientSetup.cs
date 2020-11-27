using Microsoft.Extensions.DependencyInjection;
using System;
using WebApiClient.Extensions.DependencyInjection;

namespace DPE.Core.Extensions
{
    /// <summary>
    /// WebApiClientSetup 启动服务
    /// </summary>
    public static class WebApiClientSetup
    {
        /// <summary>
        /// 注册WebApiClient接口
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpApi(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));


            //services.AddHttpApi<IDoubanApi>().ConfigureHttpApiConfig(c =>
            //{
            //    c.HttpHost = new Uri("http://api.xiaomafeixiang.com/");
            //    c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            //});
        }
    }
}
