using OdinPlugs.OdinCore.Models.Exception;

namespace OdinPlugs.OdinCore.Models.Exception.ExceptionExtends
{
    public class RequestGuidException : OdinException
    {
        public RequestGuidException(string errorCode) : base(errorCode) { }
    }
}