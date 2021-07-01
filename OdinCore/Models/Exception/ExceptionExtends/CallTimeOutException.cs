using OdinPlugs.OdinCore.Models.Exception;

namespace OdinPlugs.OdinCore.Models.Exception.ExceptionExtends
{
    public class CallTimeOutException : OdinException
    {
        public CallTimeOutException(string errorCode) : base(errorCode) { }
    }
}