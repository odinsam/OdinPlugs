namespace OdinPlugs.OdinCore.Models.Aop
{
    public class Aop_ApiInvokerThrow_Model : Aop_Invoker_Model
    {
        /// <summary>
        /// ControllerName
        /// </summary>
        /// <returns></returns>
        public string ErrorMessage { get; set; }
        public long ErrorTime { get; set; }
        public System.Exception Ex { get; set; }
    }
}