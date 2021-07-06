using System.Diagnostics;
using System.Security.AccessControl;
using OdinPlugs.OdinBasicDataType.OdinEnum.EnumLink;

namespace OdinPlugs.OdinCore.Models.ApiLinkModels
{
    public class OdinApiLinkModel
    {
        /// <summary>
        /// 当次链路唯一标识
        /// </summary>
        /// <value></value>
        public long Id { get; set; }

        /// <summary>
        /// 链路状态 
        /// start、end、invoker
        /// </summary>
        /// <value></value>  
        public EnumLinkStatus LinkStatusEnum { get; set; }

        /// <summary>
        /// 链路状态  LinkStatusEnum.ToString()
        /// </summary>
        /// <value></value>  
        public string LinkStatusStr { get; set; }

        /// <summary>
        /// 上层链路
        /// </summary>
        /// <value></value>
        public long LinkPrevious { get; set; }

        /// <summary>
        /// 当前链路调用返回状态
        /// Success 成功 CatchReturn catch ThrowException exception
        /// </summary>
        /// /// <value></value>
        public EnumInvokerReturnStatus InvokerReturnStatusEnum { get; set; } = EnumInvokerReturnStatus.None;

        /// <summary>
        /// 当前链路调用返回状态
        /// Success 成功 CatchReturn catch ThrowException exception
        /// </summary>
        /// /// <value></value>
        public string InvokerReturnStatusStr { get; set; }

        /// <summary>
        /// 调用返回结果
        /// </summary>
        /// <value></value>
        public string InvokerResult { get; set; }

        /// <summary>
        /// 下层链路
        /// </summary>
        /// <value></value>
        public long LinkNext { get; set; }

        /// <summary>
        /// 消耗时间 ms
        /// </summary>
        public long? ElapsedTime { get; set; } = null;

        /// <summary>
        /// 当前调用类全名(命名空间.类名)
        /// </summary>
        /// <value></value>
        public string InvokerClassFullName { get; set; }
        /// <summary>
        /// 当前调用类全名(类名)
        /// </summary>
        /// <value></value>
        public string InvokerClassName { get; set; }
        /// <summary>
        /// 当前调用方法名
        /// </summary>
        /// <value></value>
        public string InvokerMethodName { get; set; }
        public string InvokerMethodParams { get; set; }

        /// <summary>
        /// 链路序列
        /// </summary>
        /// <value></value>
        public int LinkSort { get; set; }
    }
}