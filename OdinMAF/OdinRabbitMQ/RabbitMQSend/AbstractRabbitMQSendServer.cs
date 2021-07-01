using System;
using OdinPlugs.OdinCore.ConfigModel.RabbitMQConfigModel;
using OdinPlugs.OdinCore.Models.RabbitMQModel;

namespace OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQSend
{
    public abstract class AbstractRabbitMQSendServer : IRabbitMQSendServer
    {
        public abstract void SendToRabbitMQ(RabbitMQOptions rabbitMQ, RabbitMQSendModel sendModel);
        public void SendJsonMessage(RabbitMQOptions rabbitMQ, RabbitMQSendModel sendModel)
        {
            RabbitMQSendHelper.SendJsonMessage(rabbitMQ, sendModel);
        }

        public static AbstractRabbitMQSendServer GetRabbitMQClient(string assemblyFullName)
        {
            return Activator.CreateInstance(Type.GetType(assemblyFullName)) as AbstractRabbitMQSendServer;
        }
    }
}