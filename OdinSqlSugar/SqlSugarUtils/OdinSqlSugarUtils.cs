using System;
using SqlSugar;
using SqlSugar.IOC;

namespace OdinPlugs.OdinSqlSugar.SqlSugarUtils
{
    public class OdinSqlSugarUtils
    {
        static SqlSugarClient Db = DbScoped.Sugar;
        /// <summary>
        /// 检查表是否存在
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool CheckTable(Type type)
        {
            string tableName = Db.EntityMaintenance.GetTableName(type);
            return Db.DbMaintenance.IsAnyTable(tableName, false);
        }

        /// <summary>
        /// 检查表是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool CheckTable<T>()
        {
            string tableName = Db.EntityMaintenance.GetTableName(typeof(T));
            return Db.DbMaintenance.IsAnyTable(tableName, false);
        }

        /// <summary>
        /// 检查表是否存在
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static bool CheckTable(string TableName)
        {
            return Db.DbMaintenance.IsAnyTable(TableName, false);
        }
    }
}