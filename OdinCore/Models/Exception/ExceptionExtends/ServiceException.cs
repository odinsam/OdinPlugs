using OdinPlugs.OdinCore.Models.Exception;

namespace OdinPlugs.OdinCore.Models.Exception.ExceptionExtends
{
    public class ServiceException : OdinException
    {
        public ServiceException(string errorCode) : base(errorCode) { }
    }
}