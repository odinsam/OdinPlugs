using System;
using OdinPlugs.OdinCore.ConfigModel.RabbitMQConfigModel;
using OdinPlugs.OdinCore.Models.RabbitMQModel;
using OdinPlugs.OdinInject;
using OdinPlugs.OdinMvcCore.OdinInject.InjectInterface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQReceive
{
    public interface IRabbitMQReceiveServer : IAutoInject
    {
        void ReceiveJsonMessage(RabbitMQOptions rabbitMQ, RabbitMQReceivedModel[] receivedModel);
        void ReceiveJsonMessageByEvent(RabbitMQOptions rabbitMQ, RabbitMQReceivedModel[] receivedModel);
    }
}