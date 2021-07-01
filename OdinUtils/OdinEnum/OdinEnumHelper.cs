using System;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace OdinPlugs.OdinUtils.OdinEnum
{
    public class OdinEnumHelper
    {

        /// <summary>
        /// 动态创建枚举
        /// </summary>
        /// <param name="enumDictionary">枚举元素列表</param>
        /// <param name="enumName">枚举名</param>
        /// <returns>Enum枚举</returns>
        public static Type CreateEnum(Dictionary<string, int> enumDictionary, string enumName = "DefalutEnum")
        {
            if (enumDictionary == null || enumDictionary.Count <= 0)
                return null;

            AppDomain currentDomain = AppDomain.CurrentDomain;
            AssemblyName aName = new AssemblyName("OdinDynamicAssembly");
            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
            ModuleBuilder mb = ab.DefineDynamicModule(aName.Name);
            EnumBuilder eb = mb.DefineEnum("Elevation", TypeAttributes.Public, typeof(int));
            foreach (var item in enumDictionary)
            {
                eb.DefineLiteral(item.Key, item.Value);
            }
            Type finished = eb.CreateTypeInfo();
            return finished;
        }

        /// <summary>
        /// 动态创建枚举
        /// <code>
        /// <para>foreach (object o in Enum.GetValues(finished))</para> 
        /// <para>{</para> 
        /// Console.WriteLine("{0}.{1} = {2}", finished, o, ((int) o));
        /// <para>}</para> 
        /// </code>
        /// </summary>
        /// <param name="enumDictionary">枚举元素列表</param>
        /// <param name="enumName">枚举名</param>
        /// <returns> Enum枚举</returns>
        public static Type CreateEnum(List<string> enumList, string enumName = "DefalutEnum")
        {
            if (enumList == null || enumList.Count <= 0)
                return null;
            AppDomain currentDomain = AppDomain.CurrentDomain;
            AssemblyName aName = new AssemblyName("OdinDynamicAssembly");
            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
            ModuleBuilder mb = ab.DefineDynamicModule(aName.Name);
            EnumBuilder eb = mb.DefineEnum("Elevation", TypeAttributes.Public, typeof(int));
            for (int i = 0; i < enumList.Count; i++)
            {
                eb.DefineLiteral(enumList[i], i);
            }
            Type finished = eb.CreateTypeInfo();
            return finished;
        }
    }
}