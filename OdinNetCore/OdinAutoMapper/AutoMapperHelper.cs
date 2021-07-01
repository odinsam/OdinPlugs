using System;
using System.Collections.Generic;
using System.Dynamic;
using AutoMapper;
using Newtonsoft.Json.Linq;
using OdinPlugs.OdinMvcCore.OdinInject;

namespace OdinPlugs.OdinNetCore.OdinAutoMapper
{
    public class OdinAutoMapper
    {
        public OdinAutoMapper()
        {

        }

        public static TDestination DynamicMapper<TDestination, TSource>(TSource source)
                where TDestination : class
                where TSource : class
        {
            var mapper = OdinInjectHelper.GetService<IMapper>();
            dynamic dobj = new ExpandoObject();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var prop in source.GetType().GetProperties())
            {
                dic.Add(prop.Name, prop.GetValue(source));
            }
            return mapper.Map<TDestination>(dic);
        }

        public static TDestination DynamicMapper<TDestination>(Object source)
                where TDestination : class
        {
            var mapper = OdinInjectHelper.GetService<IMapper>();
            dynamic dobj = new ExpandoObject();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var prop in source.GetType().GetProperties())
            {
                dic.Add(prop.Name, prop.GetValue(source));
            }
            return mapper.Map<TDestination>(dic);
        }
    }
}