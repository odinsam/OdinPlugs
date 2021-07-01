using System.Collections.Generic;

namespace OdinPlugs.OdinCore.ConfigModel.ErrorCodeModel
{
    public class ErrorModel
    {
        /// <summary>
        /// ok
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// ok
        /// </summary>
        public string ShowMessage { get; set; }
        public string Handle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    public class ErrorCodesCnfOptions
    {
        public List<ErrorModel> ErrorCodes { get; set; }
    }
}