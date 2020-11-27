
using DPE.Core.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPE.Core.AOP
{
    public class RabbitMQClient
    {
        private readonly IModel _channel;

        public RabbitMQClient()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Appsettings.app(new string[] { "AppSettings","RabbitMQ", "HostName" }),
                    Port= Appsettings.app(new string[] { "AppSettings", "RabbitMQ", "Port" }).ObjToInt(),
                    UserName = Appsettings.app(new string[] { "AppSettings", "RabbitMQ", "UserName" }),
                    Password = Appsettings.app(new string[] { "AppSettings", "RabbitMQ", "Password" }),
                    VirtualHost="/"
                };
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();

                //PushMessage("AAA",new {id=1 });
               // PushMessage("BBB", new { id = 12, msg = "BBB" });
            }
            catch(Exception ex)
            {
                throw new Exception("初始化队列失败："+ex.Message);
            }
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="message">消息内容,可以是字符串</param>
        public virtual void PushMessage(string queue, object message)
        {
            _channel.QueueDeclare(
                    queue: queue,//队列名称
                    durable: false,//是否持久化, 队列的声明默认是存放到内存中的，如果rabbitmq重启会丢失，如果想重启之后还存在就要使队列持久化，保存到Erlang自带的Mnesia数据库中，当rabbitmq重启之后会读取该数据库
                    exclusive: false,//是否排外的，有两个作用，一：当连接关闭时connection.close()该队列是否会自动删除；二：该队列是否是私有的private，如果不是排外的，可以使用两个消费者都访问同一个队列，没有任何问题，如果是排外的，会对当前队列加锁，其他通道channel是不能访问的
                    autoDelete: false,//是否自动删除，当最后一个消费者断开连接之后队列是否自动被删除，可以通过RabbitMQ Management，查看某个队列的消费者数量，当consumers = 0时队列就会自动删除
                    arguments: null //队列中的消息什么时候会自动被删除？
                    );
            string msgJson = string.Empty;
            if (message.GetType() == typeof(string))
            {
                msgJson = message.ToString();
            }
            else {
                msgJson = JsonConvert.SerializeObject(message);
            }
            var body = Encoding.UTF8.GetBytes(message.ToString());
            _channel.BasicPublish(exchange: "",//交换器名称
                routingKey: queue,//路由键
                basicProperties: null,
                body: body //消息体
                );
        }
    }
}
