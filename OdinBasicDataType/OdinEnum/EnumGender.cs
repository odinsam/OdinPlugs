using System.ComponentModel;

namespace OdinPlugs.OdinBasicDataType.OdinEnum
{
    public enum EnumGender
    {
        [Description("未知")]
        secrecy = -1,
        [Description("女")]
        female = 0,
        [Description("男")]
        male = 1,
    }
}
