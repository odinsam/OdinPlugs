using System.ComponentModel;

namespace Odin.Plugs.OdinBasicDataType.OdinEnum
{
    public enum EnumMethod
    {
        [Description("HttpGet")]
        Get,

        [Description("HttpPost")]
        Post,

        [Description("HttpPut")]
        Put,

        [Description("HttpDelete")]
        Delete
    }
}
