using System.Text;
namespace OdinPlugs.OdinCore.Models.RabbitMQModel
{
    public class RabbitMQSendModel
    {
        public string ExchangeName { get; set; }
        public string QueuesName { get; set; }
        public string RouteKey { get; set; }
        public string SendMessage { get; set; }
        public string ContentType { get; set; } = "application/json";
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        /// <summary>
        /// <summary>
        /// 发消息时将mandatory值设置为true，保证当消息路由不到队列时，将消息进行返回，以告诉客户端
        /// </summary>
        /// <value></value>
        public bool Mandatory { get; set; } = false;
        /*
        设置消息持久化必须先设置队列持久化，要不然队列不持久化，消息持久化，队列都不存在了，消息存在还有什么意义。
        消息持久化需要将交换机持久化、队列持久化、消息持久化，才能最终达到持久化的目的
        */
        public byte DeliveryMode { get; set; } = 2;
    }
}