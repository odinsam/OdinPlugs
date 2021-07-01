using OdinPlugs.OdinCore.Models.Exception;

namespace OdinPlugs.OdinCore.Models.Exception.ExceptionExtends
{
    public class AllowIpException : OdinException
    {
        public AllowIpException(string errorCode) : base(errorCode) { }
    }
}