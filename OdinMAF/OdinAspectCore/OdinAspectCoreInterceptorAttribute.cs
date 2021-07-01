using System;
using System.Dynamic;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Serilog;

namespace OdinPlugs.OdinMAF.OdinAspectCore
{
    public class OdinAspectCoreInterceptorAttribute : AbstractInterceptorAttribute
    {
        string _str;
        public OdinAspectCoreInterceptorAttribute(string str)
        {
            _str = str;
        }
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                Log.Information($"拦截前执行{_str}");
                await next(context);
            }
            catch
            {
                Log.Information($"执行出错");
                throw;
            }
            finally
            {
                Log.Information($"拦截后执行{_str}");
            }
        }
    }
}