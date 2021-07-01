using OdinPlugs.OdinCore.ConfigModel.RabbitMQConfigModel;
using OdinPlugs.OdinCore.Models.RabbitMQModel;

namespace OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQSend
{
    public class RabbitMQSendServer : AbstractRabbitMQSendServer
    {
        public override void SendToRabbitMQ(RabbitMQOptions rabbitMQ, RabbitMQSendModel sendModel)
        {
            base.SendJsonMessage(rabbitMQ, sendModel);
        }
    }
}