using OdinPlugs.OdinCore.Models.Exception;

namespace OdinPlugs.OdinCore.Models.Exception.ExceptionExtends
{
    public class ParamSignException : OdinException
    {
        public ParamSignException(string errorCode) : base(errorCode) { }
    }
}