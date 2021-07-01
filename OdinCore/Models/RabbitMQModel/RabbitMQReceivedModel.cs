using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OdinPlugs.OdinCore.Models.RabbitMQModel
{
    public class RabbitMQReceivedModel
    {
        public string ExchangeName { get; set; }

        public ReceiveQueueInfo[] ReceiveQueues { get; set; }

    }
    public class ReceiveQueueInfo
    {
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }
        /// <summary>
        /// true  自动ACK：消息一旦被接收，消费者自动发送ACK 
        /// false 手动ACK：消息接收后，不会发送ACK，需要手动调用
        /// </summary>
        /// <value></value>
        public bool AutoAck { get; set; } = false;

        public Action<BasicGetResult, IModel> ReceiveAction { get; set; }

        public EventHandler<BasicDeliverEventArgs> ReceiveEventAction { get; set; }
    }
}