using System;
using OdinPlugs.OdinCore.ConfigModel.RabbitMQConfigModel;
using OdinPlugs.OdinCore.Models.RabbitMQModel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQReceive
{
    public class RabbitMQReceiveServer : AbstractRabbitMQReceiveServer
    {
        public override void ReceiveJsonMessage(RabbitMQOptions rabbitMQ, RabbitMQReceivedModel[] receivedModel)
        {
            base.ReceiveMessage(rabbitMQ, receivedModel);
        }

        public override void ReceiveJsonMessageByEvent(RabbitMQOptions rabbitMQ, RabbitMQReceivedModel[] receivedModel)
        {
            base.ReceiveMessageByEvent(rabbitMQ, receivedModel);
        }
    }
}