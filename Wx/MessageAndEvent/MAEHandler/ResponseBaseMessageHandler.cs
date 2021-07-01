using System;
using OdinPlugs.Wx.MessageAndEvent.AbstractMAE;
using OdinPlugs.Wx.Models.MAEModel;
using System.Threading.Tasks;
namespace OdinPlugs.Wx.MessageAndEvent.MAEHandler
{
    public class ResponseBaseMessageHandler : IMAEMessageHandler
    {
        public ResponseBaseMessageHandler()
        {
        }

        public string Handler(AbMAEBase messageHandler, Action<Object> callBack)
        {
            throw new System.NotImplementedException();
        }
    }
}