using DPE.Core.Common;
using DPE.Core.Model.Models;

using Microsoft.AspNetCore.Builder;
using System;

namespace DPE.Core.Extensions
{
    /// <summary>
    /// Cors 启动服务
    /// </summary>
    public static class SeedDataMildd
    {
       
        public static void UseSeedDataMildd(this IApplicationBuilder app, MyContext myContext, string webRootPath)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                if (Appsettings.app("AppSettings", "SeedDBEnabled").ObjToBool() || Appsettings.app("AppSettings", "SeedDBDataEnabled").ObjToBool())
                {
                    DBSeed.SeedAsync(myContext, webRootPath).Wait();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
