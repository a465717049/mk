

using System;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using DPE.Core.Common;
using DPE.Core.Common.DB;
using System.Collections.Generic;
using System.Linq;
namespace DPE.Core.Extensions
{
    public class RabbitMessageLister : RabbitListener
    {

        private static SqlSugarClient db;

        // 因为Process函数是委托回调,直接将其他Service注入的话两者不在一个scope,
        // 这里要调用其他的Service实例只能用IServiceProvider CreateScope后获取实例对象
        private readonly IServiceProvider _services;

        public RabbitMessageLister(IServiceProvider services,
         ILogger<RabbitListener> logger) : base()
        {
            base.RouteKey = "ApplySellApple";
            base.QueueName = "ApplySellApple";
            List<MutiDBOperate> listdatabase = Appsettings.app<MutiDBOperate>("DBS")
               .Where(i => i.Enabled).ToList();
            var dbfirst = listdatabase.FirstOrDefault(d => d.ConnId == Appsettings.app(new string[] { "MainDB" })).Connection;
            if (dbfirst == null)
            {
                dbfirst = listdatabase.FirstOrDefault().Connection;
            }
            db = new SqlSugarClient(
                      new ConnectionConfig()
                      {// "server=rm-wz9emci41ce4mx644oo.sqlserver.rds.aliyuncs.com,5355;uid=dpe_pie_manager;pwd=!QAZXSW@#EDCVFR$aazz;database=dpepie"
                          ConnectionString = dbfirst,
                          DbType = DbType.SqlServer,//设置数据库类型
                      IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                      InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
                  });
            _services = services;

        }

        public override bool Process(string message)
        {
            try
            {
                var msg = message.Split(",");
                if (msg.Length < 2)
                {
                    return false;
                }
                long userid = 0;
                decimal dpe = 0;
                userid = Convert.ToInt32(msg[0]);
                dpe = Convert.ToDecimal(msg[1]);
                db.Ado.UseStoredProcedure().ExecuteCommand("TransDPE", new { userid, dpe });
            }
            catch
            {
                return false;
            }
            return true;

        }

    }
}
