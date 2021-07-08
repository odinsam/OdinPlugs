using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OdinPlugs.OdinCore.ConfigModel;
using OdinPlugs.OdinCore.Models;
using OdinPlugs.OdinUtils.Utils.OdinTime;

namespace OdinPlugs.OdinMvcCore.MvcCore
{
    public class OdinApiCommentCore
    {
        private readonly ConfigOptions options;
        public OdinApiCommentCore(ConfigOptions _options)
        {
            options = _options;
        }
        /// <summary>
        /// 获取当前项目所有Controller中action的信息
        /// </summary>
        /// <returns>The API comment.</returns>
        /// <param name="_actionProvider">Action provider.</param>
        public IEnumerable<ApiCommentConfig> GetApiComments(IActionDescriptorCollectionProvider _actionProvider)
        {
            string authorType = "OdinPlugs.OdinAttr.AuthorAttribute";
            string createTimeType = "OdinPlugs.OdinAttr.CreateTimeAttribute";
            string updateTimeType = "OdinPlugs.OdinAttr.UpdateTimeAttribute";
            string obsoleteTimeType = "System.ObsoleteAttribute";
            string allowType = "Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute";
            string authType = "Microsoft.AspNetCore.Authorization.AuthorizeAttribute";
            string postType = "Microsoft.AspNetCore.Mvc.HttpPostAttribute";
            string putType = "Microsoft.AspNetCore.Mvc.HttpPutAttribute";
            string deleteType = "Microsoft.AspNetCore.Mvc.HttpDeleteAttribute";
            string versionType = "Microsoft.AspNetCore.Mvc.ApiVersionAttribute";
            var actionDescs = _actionProvider.ActionDescriptors.Items.Cast<ControllerActionDescriptor>().Select(x =>
                new ApiCommentConfig
                {
                    Author = (
                                x.MethodInfo.CustomAttributes
                                        .SingleOrDefault(c => c.AttributeType.FullName == authorType) != null
                                ?
                                x.MethodInfo.CustomAttributes
                                        .SingleOrDefault(c => c.AttributeType.FullName == authorType)
                                        .ConstructorArguments[0].Value.ToString()
                                :
                                (
                                    x.ControllerTypeInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == authorType) != null
                                    ?
                                    x.ControllerTypeInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == authorType)
                                            .ConstructorArguments[0].Value.ToString()
                                    :
                                    "NoAuthor"
                                )
                            ),
                    ApiController = x.ControllerName + "Controller",
                    ApiAction = x.ActionName,
                    DisplayName = x.DisplayName,
                    RouteTemplate = x.AttributeRouteInfo.Template,
                    ApiPath = x.AttributeRouteInfo.Template.Replace("{version:apiVersion}", (
                            x.ControllerTypeInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == versionType) != null
                                            ?
                                            x.ControllerTypeInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == versionType)
                                            .ConstructorArguments[0].Value.ToString()
                                            :
                                            options.ApiVersion.MajorVersion + "." + options.ApiVersion.MinorVersion
                        )).Replace("{id}", ""),
                    CreateTime = (
                                    x.MethodInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == createTimeType) != null
                                    ?
                                    UnixTimeHelper.FromDateTime(Convert.ToDateTime(x.MethodInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == createTimeType)
                                            .ConstructorArguments[0].Value.ToString()))
                                    :
                                    (
                                    x.ControllerTypeInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == createTimeType) != null
                                    ?
                                    UnixTimeHelper.FromDateTime(Convert.ToDateTime(x.ControllerTypeInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == createTimeType)
                                            .ConstructorArguments[0].Value.ToString()))
                                    :
                                    UnixTimeHelper.GetUnixDateTime()
                                    )
                                ),
                    UpdateTime = (
                                    x.MethodInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == updateTimeType) != null
                                    ?
                                    UnixTimeHelper.FromDateTime(Convert.ToDateTime(x.MethodInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == updateTimeType)
                                            .ConstructorArguments[0].Value.ToString()))
                                    :
                                    (
                                    x.ControllerTypeInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == updateTimeType) != null
                                    ?
                                    UnixTimeHelper.FromDateTime(Convert.ToDateTime(x.ControllerTypeInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == updateTimeType)
                                            .ConstructorArguments[0].Value.ToString()))
                                    :
                                    UnixTimeHelper.GetUnixDateTime()
                                    )
                                ),
                    IsObsolete = (
                                    x.ControllerTypeInfo.CustomAttributes
                                        .SingleOrDefault(c => c.AttributeType.FullName == obsoleteTimeType) != null
                                    ?
                                    1
                                    :
                                    (x.MethodInfo.CustomAttributes
                                        .SingleOrDefault(c => c.AttributeType.FullName == obsoleteTimeType) != null
                                    ? 1 : 0)
                                ),
                    Method = (
                                    x.MethodInfo.CustomAttributes
                                        .SingleOrDefault(c => c.AttributeType.FullName == postType) != null
                                    ?
                                    "POST"
                                    :
                                        (x.MethodInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == putType) != null
                                        ?
                                        "PUT"
                                        :
                                            (
                                            x.MethodInfo.CustomAttributes
                                                .SingleOrDefault(c => c.AttributeType.FullName == deleteType) != null
                                            ?
                                            "DELETE"
                                            :
                                            "GET"
                                            )
                                        )
                                ),
                    AllowScope = (
                                    x.ControllerTypeInfo.CustomAttributes
                                            .SingleOrDefault(c => c.AttributeType.FullName == authType) == null
                                        ?
                                        (
                                            x.ControllerTypeInfo.CustomAttributes
                                                    .SingleOrDefault(c => c.AttributeType.FullName == allowType) != null
                                                ? 0 :
                                                (x.MethodInfo.CustomAttributes
                                                    .SingleOrDefault(c => c.AttributeType.FullName == authType) != null
                                                    ? 1 : 0)
                                        )
                                        :
                                        (
                                            x.MethodInfo.CustomAttributes
                                                    .SingleOrDefault(c => c.AttributeType.FullName == allowType) != null
                                                ? 0 : 1
                                        )
                                ),
                    Attributes = x.MethodInfo.CustomAttributes.Select(z =>
                        new AttributeConfig
                        {
                            TypeName = z.AttributeType.FullName,
                            ConstructorArgs = z.ConstructorArguments.Select(v =>
                                new ConstructorArgConfig { ArgumentValue = v.Value.ToString() }),
                            NamedArguments = z.NamedArguments.Select(v => new NamedArgumentConfig
                            {
                                MemberName = v.MemberName,
                                TypedValue = v.TypedValue.Value.ToString(),
                            }),
                        }),
                    ActionId = x.Id,
                    RouteValues = x.RouteValues,
                    Parameters = x.Parameters.Select(z =>
                        new ParameterConfig
                        {
                            Name = z.Name,
                            TypeName = z.ParameterType.Name,
                        })
                });
            return actionDescs;
        }


        public static ApiCommentConfig GetApiComment(IEnumerable<ApiCommentConfig> apiComments,
                                                        string controllerName, string actionName)
        {
            ApiCommentConfig api = apiComments.Where(cms => cms.ApiController == controllerName && cms.ApiAction == actionName)
                                                .SingleOrDefault();
            return api;
        }

    }
}