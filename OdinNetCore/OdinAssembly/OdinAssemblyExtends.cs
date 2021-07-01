using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OdinPlugs.OdinNetCore.OdinAssembly
{
    public static class OdinAssemblyExtends
    {
        public static IEnumerable<Type> GetAllTypes<T>(this Assembly ass) where T : class
        {
            var type = typeof(T);
            return ass.GetTypes().Where(t => type.IsAssignableFrom(t));
        }
    }
}