using System;
using System.Collections.Generic;
using System.Linq;
using OdinPlugs.OdinCore.ConfigModel.RabbitMQConfigModel;
using OdinPlugs.OdinCore.Models.RabbitMQModel;
using OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQBase;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQReceive
{
    public class RabbitMQReceiveHelper
    {
        public static void ServerReceived(RabbitMQOptions rabbitMQ, RabbitMQReceivedModel[] receivedModel)
        {
            using (IConnection connection = RabbitMQConnectionBuilder.CreateRabbitMQConnection(rabbitMQ.Account.UserName, rabbitMQ.Account.Password, rabbitMQ.HostNames))
            {
                IModel channel = connection.CreateModel();
                foreach (var model in receivedModel)
                {
                    var exchange = rabbitMQ.Exchanges.ToList().Where(e => e.ExchangeName == model.ExchangeName).FirstOrDefault();
                    Dictionary<string, Object> exchangeDic = RabbitMQQueueHelper.CreateQueueArgs(exchange.Arguments);
                    channel.ExchangeDeclare(exchange.ExchangeName, exchange.ExchangeType, exchange.Durability, exchange.AutoDelete, exchangeDic == null ? null : exchangeDic);
                    foreach (var queueModel in model.ReceiveQueues)
                    {
                        var queue = exchange.Queues.ToList().Where(q => q.QueuesName == queueModel.QueueName).FirstOrDefault();
                        if (queue != null)
                        {
                            Dictionary<string, Object> queueDic = RabbitMQQueueHelper.CreateQueueArgs(queue.Arguments);
                            //声明一个队列，设置队列是否持久化，排他性，与自动删除
                            channel.QueueDeclare(queueModel.QueueName, queue.Durable, queue.Exclusive, queue.AutoDelete, queueDic == null ? null : queueDic);
                            //一次只接收一个
                            channel.BasicQos(0, 1, false);
                            //绑定消息队列，交换器，routingkey
                            channel.QueueBind(queueModel.QueueName, model.ExchangeName, queueModel.RoutingKey, queueDic == null ? null : queueDic);
                            BasicGetResult msgResponse = channel.BasicGet(queueModel.QueueName, false);
                            /*
                            like this
                            if (result != null)
                            {
                                var msgByte = result.Body;
                                var msgBody = Encoding.UTF8.GetString(msgByte.ToArray());
                                Console.WriteLine(string.Format("***接收时间:{0}，消息内容：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));

                                channel.BasicAck(result.DeliveryTag, false);
                            }
                            */
                            queueModel.ReceiveAction(msgResponse, channel);
                        }
                        else
                        {
                            throw new Exception("queueName 找不到队列");
                        }
                    }
                }
            }
        }


        public static void ServerReceivedByEvent(RabbitMQOptions rabbitMQ, RabbitMQReceivedModel[] receivedModel)
        {
            using (IConnection connection = RabbitMQConnectionBuilder.CreateRabbitMQConnection(rabbitMQ.Account.UserName, rabbitMQ.Account.Password, rabbitMQ.HostNames))
            {
                IModel channel = connection.CreateModel();
                foreach (var model in receivedModel)
                {
                    var exchange = rabbitMQ.Exchanges.ToList().Where(e => e.ExchangeName == model.ExchangeName).FirstOrDefault();
                    Dictionary<string, Object> exchangeDic = RabbitMQQueueHelper.CreateQueueArgs(exchange.Arguments);
                    channel.ExchangeDeclare(exchange.ExchangeName, exchange.ExchangeType, exchange.Durability, exchange.AutoDelete, exchangeDic == null ? null : exchangeDic);
                    foreach (var queueModel in model.ReceiveQueues)
                    {
                        var queue = exchange.Queues.ToList().Where(q => q.QueuesName == queueModel.QueueName).FirstOrDefault();
                        if (queue != null)
                        {
                            Dictionary<string, Object> queueDic = RabbitMQQueueHelper.CreateQueueArgs(queue.Arguments);
                            //声明一个队列，设置队列是否持久化，排他性，与自动删除
                            channel.QueueDeclare(queueModel.QueueName, queue.Durable, queue.Exclusive, queue.AutoDelete, queueDic == null ? null : queueDic);
                            //一次只接收一个
                            channel.BasicQos(0, 1, false);
                            //绑定消息队列，交换器，routingkey
                            channel.QueueBind(queueModel.QueueName, model.ExchangeName, queueModel.RoutingKey, queueDic == null ? null : queueDic);
                            var consumer = new EventingBasicConsumer(channel);
                            channel.BasicConsume(queueModel.QueueName, false, consumer);
                            consumer.Received += queueModel.ReceiveEventAction;
                        }
                        else
                        {
                            throw new Exception("queueName 找不到队列");
                        }
                    }
                }
            }
        }
    }
}