using System;
using System.Collections.Generic;
using OdinPlugs.OdinCore.ConfigModel.RabbitMQConfigModel;

namespace OdinPlugs.OdinMAF.OdinRabbitMQ.RabbitMQBase
{
    public class RabbitMQQueueHelper
    {
        public static Dictionary<string, Object> CreateQueueArgs(ArgumentsModel[] Arguments)
        {
            Dictionary<string, Object> dic = null;
            foreach (var args in Arguments)
            {
                if (args.Value != "0" && args.Value.ToString() != "")
                {
                    System.Console.WriteLine(args.Value);
                    dic = new Dictionary<string, object>();
                    dic.Add(args.KeyName, args.Value.ToString());
                };
            }
            return dic;
        }
    }
}