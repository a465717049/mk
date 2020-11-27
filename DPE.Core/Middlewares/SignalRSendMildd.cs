using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DPE.Core.AuthHelper.OverWrite;
using Microsoft.AspNetCore.Builder;
using System.IO;
using DPE.Core.Common.LogHelper;
using StackExchange.Profiling;
using System.Text.RegularExpressions;
using DPE.Core.IServices;
using Newtonsoft.Json;
using DPE.Core.Hubs;
using Microsoft.AspNetCore.SignalR;
using DPE.Core.Common;

namespace DPE.Core.Middlewares
{
    /// <summary>
    /// 中间件
    /// 记录请求和响应数据
    /// </summary>
    public class SignalRSendMildd
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly IHubContext<ChatHub> _hubContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="hubContext"></param>
        public SignalRSendMildd(RequestDelegate next, IHubContext<ChatHub> hubContext)
        {
            _next = next;
            _hubContext = hubContext;
        }



        public async Task InvokeAsync(HttpContext context)
        {
            if (Appsettings.app("Middleware", "SignalR", "Enabled").ObjToBool())
            {
                await _hubContext.Clients.All.SendAsync("ReceiveUpdate", LogLock.GetLogData()); 
            }
            await _next(context);
        }

    }
}

