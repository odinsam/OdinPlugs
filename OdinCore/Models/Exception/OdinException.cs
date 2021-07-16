using OdinPlugs.OdinInject;
using OdinPlugs.OdinInject.InjectCore;
using OdinPlugs.OdinInject.InjectPlugs.OdinErrorCodeInject;

namespace OdinPlugs.OdinCore.Models.Exception
{
    public class OdinException : System.Exception
    {
        private readonly IOdinErrorCode odinErrorCodeHelper;

        public string ErrorCode { get; set; }
        public string ShowMessage { get; set; }
        public string Handle { get; set; }
        public OdinException(string errorCode, string message = "") : base(message)
        {
            if (this.odinErrorCodeHelper == null)
                this.odinErrorCodeHelper = OdinInjectCore.GetService<IOdinErrorCode>();
            var errorModel = this.odinErrorCodeHelper.GetErrorModel(errorCode);
            this.ErrorCode = errorCode;
            message = errorModel.ErrorMessage;
            this.ShowMessage = errorModel.ShowMessage;
            this.Handle = errorModel.Handle;
        }
    }
}