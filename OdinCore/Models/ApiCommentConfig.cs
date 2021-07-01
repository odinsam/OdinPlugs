using System.Collections.Generic;
namespace Odin.Plugs.OdinCore.Models
{
    public class ApiCommentConfig
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string ApiController { get; set; }
        public string ApiAction { get; set; }
        public string DisplayName { get; set; }
        public string Method { get; set; }
        public string ApiPath { get; set; }
        public string RouteTemplate { get; set; }
        public string ActionId { get; set; }
        /// <summary>
        /// 是否过期 0未过期  1已过期
        /// </summary>
        /// <value>The is obsolete.</value>
        public int IsObsolete { get; set; } = 0;
        public long CreateTime { get; set; }
        public long? UpdateTime { get; set; }

        /// <summary>
        /// 访问权限 0 匿名 1 登录 2 角色
        /// </summary>
        /// <value>The allow scope.</value>
        public int AllowScope { get; set; } = 0;


        public IEnumerable<AttributeConfig> Attributes { get; set; }
        public IDictionary<string, string> RouteValues { get; set; }
        public IEnumerable<ParameterConfig> Parameters { get; set; }
    }

    public class AttributeConfig
    {
        public string TypeName { get; set; }
        public IEnumerable<ConstructorArgConfig> ConstructorArgs { get; set; }
        public IEnumerable<NamedArgumentConfig> NamedArguments { get; set; }
    }

    public class ConstructorArgConfig
    {
        public string ArgumentValue { get; set; }
    }

    public class NamedArgumentConfig
    {
        public string MemberName { get; set; }
        public string TypedValue { get; set; }
    }

    public class ParameterConfig
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
    }
}