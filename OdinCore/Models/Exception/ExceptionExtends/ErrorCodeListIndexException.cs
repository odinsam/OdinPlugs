using OdinPlugs.OdinCore.Models.Exception;

namespace OdinPlugs.OdinCore.Models.Exception.ExceptionExtends
{
    public class ErrorCodeListIndexException : OdinException
    {
        public ErrorCodeListIndexException(string errorCode) : base(errorCode) { }
    }
}