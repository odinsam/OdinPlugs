using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQReceive
{
    public class RabbitMQReceiveHandler
    {
        public static string ReceiveJsonMessageHandler(BasicGetResult result, IModel channel, bool autoAck = true)
        {
            if (result != null)
            {
                var msgByte = result.Body;
                var msgBody = Encoding.UTF8.GetString(msgByte.ToArray());
                if (autoAck)
                    channel.BasicAck(result.DeliveryTag, false);
                return msgBody;
            }
            else
                return null;
        }

        public static string ReceiveByEventJsonMessageHandler(Object obj, BasicDeliverEventArgs e, bool autoAck = true)
        {
            var sender = obj as IModel;
            var message = e.Body;//接收到的消息
            if (autoAck)
                sender.BasicAck(e.DeliveryTag, false);
            return Encoding.UTF8.GetString(message.ToArray());
        }
    }
}