using OdinPlugs.OdinCore.Models.Exception;

namespace OdinPlugs.OdinCore.Models.Exception.ExceptionExtends
{
    public class TokenException : OdinException
    {
        public TokenException(string errorCode) : base(errorCode) { }
    }
}