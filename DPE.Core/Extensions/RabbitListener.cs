
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DPE.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace DPE.Core.Extensions
{
    public class RabbitListener : IHostedService
    {

        private readonly IConnection connection;
        private readonly IModel channel;


        public RabbitListener()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Appsettings.app(new string[] { "AppSettings", "RabbitMQ", "HostName" }),
                    Port = Appsettings.app(new string[] { "AppSettings", "RabbitMQ", "Port" }).ObjToInt(),
                    UserName = Appsettings.app(new string[] { "AppSettings", "RabbitMQ", "UserName" }),
                    Password = Appsettings.app(new string[] { "AppSettings", "RabbitMQ", "Password" }),
                    VirtualHost = "/"
                 
                };
                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
               // Console.WriteLine($"RabbitListener init error,ex:{ex.Message}");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Register();
            return Task.CompletedTask;
        }

        protected string RouteKey;
        protected string QueueName;

        // 处理消息的方法
        public virtual bool Process(string message)
        {

            throw new NotImplementedException();
        }

        // 注册消费者监听在这里
        public void Register()
        {
            channel.ExchangeDeclare(exchange: "message", type: "topic");
            channel.QueueDeclare(queue: QueueName,
                    durable: false,//是否持久化, 队列的声明默认是存放到内存中的，如果rabbitmq重启会丢失，如果想重启之后还存在就要使队列持久化，保存到Erlang自带的Mnesia数据库中，当rabbitmq重启之后会读取该数据库
                    exclusive: false,//是否排外的，有两个作用，一：当连接关闭时connection.close()该队列是否会自动删除；二：该队列是否是私有的private，如果不是排外的，可以使用两个消费者都访问同一个队列，没有任何问题，如果是排外的，会对当前队列加锁，其他通道channel是不能访问的
                    autoDelete: false,//是否自动删除，当最后一个消费者断开连接之后队列是否自动被删除，可以通过RabbitMQ Management，查看某个队列的消费者数量，当consumers = 0时队列就会自动删除
                    arguments: null //队列中的消息什么时候会自动被删除？
                    );
            channel.QueueBind(queue: QueueName,
                              exchange: "message",
                              routingKey: RouteKey);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var result = Process(message);
                if (result)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            channel.BasicConsume(queue: QueueName, consumer: consumer);
        }

        public void DeRegister()
        {
            this.connection.Close();
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.connection.Close();
            return Task.CompletedTask;
        }
    }

}


