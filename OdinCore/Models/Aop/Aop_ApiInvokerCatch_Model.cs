using System;

namespace OdinPlugs.OdinCore.Models.Aop
{
    public class Aop_ApiInvokerCatch_Model : Aop_Invoker_Model
    {
        /// <summary>
        /// ControllerName
        /// </summary>
        /// <returns></returns>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// ActionName
        /// </summary>
        /// <returns></returns>
        public string ShowMessage { get; set; }
        /// <summary>
        /// ActionName
        /// </summary>
        /// <returns></returns>
        public string ErrorCode { get; set; }
        public long ErrorTime { get; set; }
        public System.Exception Ex { get; set; }
    }
}