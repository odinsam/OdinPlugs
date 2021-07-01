using OdinPlugs.OdinCore.Models.Exception;

namespace OdinPlugs.OdinCore.Models.Exception.ExceptionExtends
{
    public class CustomException : OdinException
    {
        public CustomException(string errorCode) : base(errorCode) { }
    }
}