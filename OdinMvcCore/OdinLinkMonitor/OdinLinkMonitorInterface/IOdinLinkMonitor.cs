using System;
using System.Collections.Generic;
using System.Diagnostics;
using AspectCore.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using OdinPlugs.OdinBasicDataType.OdinEnum.EnumLink;
using OdinPlugs.OdinCore.Models.ApiLinkModels;
using OdinPlugs.OdinInject.InjectInterface;

namespace OdinPlugs.OdinMvcCore.OdinLinkMonitor.OdinLinkMonitorInterface
{
    public interface IOdinLinkMonitor : IAutoInject
    {
        Dictionary<long, Stack<OdinApiLinkModel>> CreateOdinLinkMonitor(long linkSnowFlakeId);
        Dictionary<long, Stack<OdinApiLinkModel>> ApiInvokerLinkMonitor(AspectContext context);
        Dictionary<long, Stack<OdinApiLinkModel>> ApiInvokerToEndLinkMonitor(AspectContext context, bool isSuccess,
            Stopwatch stopwatch);
        Dictionary<long, Stack<OdinApiLinkModel>> ApiInvokerOverLinkMonitor(ActionExecutedContext context,
            long elapsedTime, bool isSuccess = true);
    }
}