using System;
using System.Threading.Tasks;
using OdinPlugs.Wx.MessageAndEvent.AbstractMAE;

namespace OdinPlugs.Wx.MessageAndEvent.MAEHandler
{
    public interface IMAEMessageHandler
    {
        string Handler(AbMAEBase messageHandler, Action<Object> callBack);
    }
}