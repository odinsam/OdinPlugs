using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using OdinPlugs.OdinCore.ConfigModel.RabbitMQConfigModel;
using OdinPlugs.OdinCore.Models.RabbitMQModel;
using OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQBase;

namespace OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQSend
{
    public class RabbitMQSendHelper
    {
        public static void SendJsonMessage(RabbitMQOptions rabbitMQ, RabbitMQSendModel sendModel)
        {
            try
            {
                using (IConnection connection = RabbitMQConnectionBuilder.CreateRabbitMQConnection(rabbitMQ.Account.UserName, rabbitMQ.Account.Password, rabbitMQ.HostNames))
                {

                    using (IModel channel = connection.CreateModel())
                    {
                        var exchange = rabbitMQ.Exchanges.ToList().Where(e => e.ExchangeName == sendModel.ExchangeName).FirstOrDefault();
                        if (exchange != null)
                        {
                            Dictionary<string, Object> exchangeDic = RabbitMQQueueHelper.CreateQueueArgs(exchange.Arguments);
                            channel.ExchangeDeclare(sendModel.ExchangeName, exchange.ExchangeType, exchange.Durability, exchange.AutoDelete, exchangeDic);
                            var queue = exchange.Queues.ToList().Where(q => q.QueuesName == sendModel.QueuesName).FirstOrDefault();
                            if (queue != null)
                            {
                                Dictionary<string, Object> queueDic = RabbitMQQueueHelper.CreateQueueArgs(queue.Arguments);
                                //创建并配置队列
                                channel.QueueDeclare(sendModel.QueuesName, true, false, false, queueDic);
                                /* 创建消息队列，并且发送消息 */
                                System.Console.WriteLine($"[ {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,ms")} ]\t Aop 发送信息");
                                //创建队列属性
                                IBasicProperties props = channel.CreateBasicProperties();
                                //决定发送数据类型
                                props.ContentType = sendModel.ContentType;
                                //是否持久化  1 no  2 yes
                                props.DeliveryMode = sendModel.DeliveryMode;
                                //发送数据
                                channel.BasicPublish(sendModel.ExchangeName,
                                                        sendModel.RouteKey,
                                                        sendModel.Mandatory,
                                                        props,
                                                        sendModel.Encoding.GetBytes(sendModel.SendMessage));
                            }
                            else
                            {
                                throw new Exception("queueName 找不到队列");
                            }
                        }
                        else
                        {
                            throw new Exception("exchangeName 找不到交换机");
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                Task.Delay(100).Wait();
                SendJsonMessage(rabbitMQ, sendModel);
            }
        }

    }
}