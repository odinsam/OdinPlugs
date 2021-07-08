using System;
using OdinPlugs.OdinUtils.Utils.OdinTime;

namespace OdinPlugs.OdinMvcCore.OdinAttr
{
    public class UpdateTimeAttribute : Attribute
    {
        public long UpdateTime { get; set; }
        public UpdateTimeAttribute()
        {
            UpdateTime = UnixTimeHelper.GetUnixDateTime();
        }

        public UpdateTimeAttribute(string time)
        {
            UpdateTime = UnixTimeHelper.FromDateTime(Convert.ToDateTime(time));
        }
    }
}
