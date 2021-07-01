using System;
using OdinPlugs.OdinUtils.OdinTime;

namespace OdinPlugs.OdinMvcCore.OdinAttr
{
    public class CreateTimeAttribute : Attribute
    {
        public long CreateTime { get; set; }
        public CreateTimeAttribute()
        {
            CreateTime = UnixTimeHelper.GetUnixDateTime();
        }

        public CreateTimeAttribute(string time)
        {
            CreateTime = UnixTimeHelper.FromDateTime(Convert.ToDateTime(time));
        }
    }
}
