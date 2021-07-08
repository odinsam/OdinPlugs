using System;
using System.Globalization;
using OdinPlugs.OdinUtils.Utils.OdinTime;

namespace OdinPlugs.OdinMvcCore.OdinAttr
{
    public class CreateTimeAttribute : Attribute
    {
        public long CreateTime { get; set; }
        public CreateTimeAttribute()
        {
            CreateTime = UnixTimeHelper.GetUnixDateTime();
        }
        public CreateTimeAttribute(string time, string timeFormat = "yyyy/MM/dd HH:mm:ss")
        {
            CreateTime = UnixTimeHelper.FromDateTime(DateTime.ParseExact(time, timeFormat, System.Globalization.CultureInfo.CurrentCulture));
        }
    }
}