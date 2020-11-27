

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SqlSugar;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace DPE.Core.RabbitMQ.Subscribe
{
    class Program
    {
       private static SqlSugarClient db;
        static void Main(string[] args)
        {
            RabbitConfig rabbitConfig = new RabbitConfig() {
                HostName = "8.210.61.26",
                Port = 5672,
                UserName = "dpepie_manager",
                PassWord = "1qazxsw2AAZZ"
            };

            db = new SqlSugarClient(
                new ConnectionConfig()
                { 
                    ConnectionString = "server=rm-wz9emci41ce4mx644oo.sqlserver.rds.aliyuncs.com,5355;uid=dpe_pie_manager;pwd=!QAZXSW@#EDCVFR$aazz;database=dpepie",
                    DbType = DbType.SqlServer,//设置数据库类型
                    IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                    InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
                });
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = rabbitConfig.HostName,
                    Port = rabbitConfig.Port,
                    UserName = rabbitConfig.UserName,
                    Password = rabbitConfig.PassWord
                };
                using (IConnection conn = factory.CreateConnection())
                {
                    using (IModel channel = conn.CreateModel())
                    {
                        //RegisteredQueue(channel, "AAA");//AAA为对队列名称
                       RegisteredQueue(channel, "ApplySellApple");
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("连接失败：" + ex.Message);
            }
        }

        private static void RegisteredQueue(IModel channel,string queueName)
        {
            //声明一个队列
            channel.QueueDeclare(
                queue: queueName,//消息队列名称
                durable: false,//是否缓存
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            //创建消费者对象
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
                var result = Process(queueName,message);
                if (result)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            Console.ReadKey();
        }

        public static bool Process(string queueName, string message)
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
                db.Ado.UseStoredProcedure().ExecuteCommand("TransDPE", new { userid = userid, dpe = dpe });
            }
            catch
            {
                return false;
            }
            return true;
        }

        public class RabbitConfig
        {
            public string HostName { get; set; }
            public int Port { get; set; }
            public string UserName { get; set; }
            public string PassWord { get; set; }
        }

    }
}
