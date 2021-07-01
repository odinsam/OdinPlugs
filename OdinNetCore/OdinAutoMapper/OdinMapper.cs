using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using AutoMapper;
using AutoMapper.Configuration;

namespace OdinPlugs.OdinNetCore.OdinAutoMapper
{
    public class OdinMapper<S, D>
    {
        /// <summary>
        /// create automapper object
        /// <para></para>
        /// <example>
        /// For example:
        /// <code>
        /// var odinMapper = new OdinMapper();
        /// <para></para>
        /// var mapper = odinMapper.CreateMapper(c=> { c.AddProfile &lt; AutoMapperBootStrapper &gt; () });
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="configure">config action</param>
        /// <returns>mapper object</returns>
        public D CreateMapper(Action<IMapperConfigurationExpression> configure, S source)
        {
            var cfg = new MapperConfiguration(configure);
            var mapper = cfg.CreateMapper();
            return mapper.Map<S, D>(source);
        }
    }
}