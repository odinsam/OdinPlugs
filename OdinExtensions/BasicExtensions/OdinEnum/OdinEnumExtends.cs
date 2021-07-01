using System;
using System.ComponentModel;
using System.Reflection;

namespace OdinPlugs.OdinExtensions.BasicExtensions.OdinEnum
{
    public static class OdinEnumExtends
    {
        /// <summary>
        /// get enum description
        /// </summary>
        /// <param name="enumValue">enum</param>
        /// <returns>enum description</returns>
        public static string GetDescription(this Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs.Length == 0)    //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }



        public static T ParseEnum<T>(this object oValue)
        {
            if (oValue == null) return default(T);
            if (oValue.Equals('\0')) return (T)Convert.ChangeType(0, typeof(T));
            if (oValue is T) return (T)oValue;
            var sValue = oValue.ToString();
            if (string.IsNullOrEmpty(sValue))
                return default(T);
            try
            {
                var o = Enum.Parse(typeof(T), sValue);
                if (o == null)
                    return default(T);
                return (T)o;
            }
            catch
            {
                return default(T);
            }
        }
    }
}