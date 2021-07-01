using System;
using OdinPlugs.OdinCore.ConfigModel.RabbitMQConfigModel;
using OdinPlugs.OdinCore.Models.RabbitMQModel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQReceive
{
    public abstract class AbstractRabbitMQReceiveServer : IRabbitMQReceiveServer
    {

        public void ReceiveMessage(RabbitMQOptions rabbitMQ, RabbitMQReceivedModel[] receivedModel)
        {
            RabbitMQReceiveHelper.ServerReceived(rabbitMQ, receivedModel);
        }

        public void ReceiveMessageByEvent(RabbitMQOptions rabbitMQ, RabbitMQReceivedModel[] receivedModel)
        {
            RabbitMQReceiveHelper.ServerReceivedByEvent(rabbitMQ, receivedModel);
        }
        public abstract void ReceiveJsonMessage(RabbitMQOptions rabbitMQ, RabbitMQReceivedModel[] receivedModel);
        public abstract void ReceiveJsonMessageByEvent(RabbitMQOptions rabbitMQ, RabbitMQReceivedModel[] receivedModel);

        public static AbstractRabbitMQReceiveServer GetRabbitMQSmsServer(string assemblyFullName)
        {
            return Activator.CreateInstance(Type.GetType(assemblyFullName)) as AbstractRabbitMQReceiveServer;
        }
    }
}