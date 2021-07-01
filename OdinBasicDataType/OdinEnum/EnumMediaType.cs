using System.ComponentModel;

namespace Odin.Plugs.OdinBasicDataType.OdinEnum
{
    public enum EnumMediaType
    {
        [Description("json格式")]
        JSON,
        [Description("form-data 格式")]
        FORMDATA
    }
}
