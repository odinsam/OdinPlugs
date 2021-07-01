using System.ComponentModel;

namespace OdinPlugs.OdinBasicDataType.OdinEnum
{
    public enum EnumStatus
    {
        [Description("启用")]
        Enable = 0,
        [Description("未启用")]
        Disenable = 1,
        [Description("删除")]
        Deleted = 1,
        [Description("未删除")]
        NonDeleted = 0,
    }
}
