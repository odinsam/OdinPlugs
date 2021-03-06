using OdinPlugs.OdinCore.ConfigModel.RabbitMQConfigModel;
using OdinPlugs.OdinCore.Models.RabbitMQModel;
using OdinPlugs.OdinInject.InjectInterface;

namespace OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQSend
{
    public interface IRabbitMQSendServer : IAutoInject
    {
        void SendToRabbitMQ(RabbitMQOptions rabbitMQ, RabbitMQSendModel sendModel);
    }
}