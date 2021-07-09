using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using OdinPlugs.OdinInject;
using OdinPlugs.OdinMAF.OdinCapService;
using OdinPlugs.OdinMAF.OdinEF.EFCore.EFExtensions.EFInterface;
using Serilog;
using SqlSugar;
using SqlSugar.IOC;

namespace OdinPlugs.OdinMAF.OdinEF.EFCore.EFExtensions
{
    public class SqlSugarBaseRepository<TEntity> : SimpleClient<TEntity>, ISqlSugarBaseRepository<TEntity> where TEntity : class, new()
    {
        public SqlSugarBaseRepository(ISqlSugarClient context = null) : base(context)
        {
            if (context == null)
            {
                var att = typeof(TEntity).GetCustomAttribute<TenantAttribute>();
                if (att != null)
                {
                    var configId = att.configId;
                    base.Context = DbScoped.Sugar.GetConnection(configId);
                }
                else
                {
                    base.Context = DbScoped.Sugar;
                }
                base.Context.Aop.OnLogExecuting = (sql, pars) => //SQL执行中事件
                {
                    //loggers.Warn(new LogContent("OnLogExecuting", "SQL语句打印", "SQL：" + GetParas(pars) + "【SQL语句】：" + sql));
                };
                base.Context.Aop.OnError = (e) =>
                {
                    Log.Error(e.Message, $"BaseRepository,SQL异常事件");
                };
            }
        }



        private string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}\n";
            }

            return key;
        }

        internal ISqlSugarClient Db
        {
            get { return Context; }
        }

        public void BeginTran()
        {
            Context.Ado.BeginTran();
        }

        public void Commit()
        {
            Context.Ado.CommitTran();
        }

        public void Rollback()
        {
            Context.Ado.RollbackTran();
        }

        /// <summary>
        /// 功能描述:根据ID查新数据
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns></returns>
        public async Task<TEntity> QueryByIdAsync(object objId)
        {
            return await Context.Queryable<TEntity>().In(objId).SingleAsync();
        }
        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public async Task<TEntity> QueryByIdAsync(object objId, bool blnUseCache = false)
        {
            return await Context.Queryable<TEntity>().WithCacheIF(blnUseCache).In(objId).SingleAsync();
        }
        /// <summary>
        /// 功能描述:根据ID查询数据
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public async Task<List<TEntity>> QueryByIdsAsync(object[] lstIds)
        {
            return await Context.Queryable<TEntity>().In(lstIds).ToListAsync();
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public async Task<long> AddAsync(TEntity entity)
        {
            var insert = Context.Insertable(entity);
            return await insert.ExecuteReturnBigIdentityAsync();
        }

        /// <summary>
        /// 写入实体数据(同步方法)
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public long Add(TEntity entity)
        {
            var insert = Context.Insertable(entity);
            return insert.ExecuteReturnBigIdentity();
        }


        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>返回自增量列</returns>
        public async Task<int> AddAsync(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            var insert = Context.Insertable(entity);
            if (insertColumns == null)
            {
                return await insert.ExecuteReturnIdentityAsync();
            }
            else
            {
                return await insert.InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
            }
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>返回被影响的行数</returns>
        public async Task<int> AddByRowsAsync(TEntity entity)
        {
            var insert = Context.Insertable(entity);
            return await insert.ExecuteCommandAsync();
        }
        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="listEntity">实体类集合</param>
        /// <returns>返回被影响的行数</returns>
        public int AddByRowsAsync(List<TEntity> listEntity)
        {
            return Context.Insertable(listEntity.ToArray()).ExecuteCommand();
        }

        /// <summary>
        /// 批量插入实体(速度快)
        /// </summary>
        /// <param name="listEntity">实体集合</param>
        /// <returns>影响行数</returns>
        public async Task<int> AddAsync(List<TEntity> listEntity)
        {
            return await Context.Insertable(listEntity.ToArray()).ExecuteCommandAsync();
        }

        public int Add(List<TEntity> listEntity)
        {
            return Context.Insertable(listEntity.ToArray()).ExecuteCommand();
        }


        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public new async Task<bool> UpdateAsync(TEntity entity)
        {
            //这种方式会以主键为条件
            return await Context.Updateable(entity).ExecuteCommandHasChangeAsync();
        }
        public bool IsUpdate(List<TEntity> entities)
        {
            return Context.Updateable(entities).ExecuteCommandHasChange();
        }
        /// <summary>
        /// 同步 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(TEntity entity)
        {
            //这种方式会以主键为条件
            return Context.Updateable(entity).ExecuteCommand();
        }
        /// <summary>
        /// 同步 更新批量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(List<TEntity> entities)
        {
            return Context.Updateable(entities).ExecuteCommand();
        }



        /// <summary>
        /// 异步 更新实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(List<TEntity> entities)
        {

            return await Context.Updateable(entities).ExecuteCommandAsync();

        }

        public async Task<bool> UpdateAsync(TEntity entity, string strWhere)
        {
            return await Context.Updateable(entity).Where(strWhere).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> UpdateAsync(string strSql, SugarParameter[] parameters = null)
        {
            return await Context.Ado.ExecuteCommandAsync(strSql, parameters) > 0;
        }

        public async Task<bool> UpdateAsync(object operateAnonymousObjects)
        {
            return await Context.Updateable<TEntity>(operateAnonymousObjects).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> UpdateAsync(
          TEntity entity,
          List<string> lstColumns = null,
          List<string> lstIgnoreColumns = null,
          string strWhere = ""
            )
        {
            IUpdateable<TEntity> up = Context.Updateable(entity);
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            {
                up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
            }
            if (lstColumns != null && lstColumns.Count > 0)
            {
                up = up.UpdateColumns(lstColumns.ToArray());
            }
            if (!string.IsNullOrEmpty(strWhere))
            {
                up = up.Where(strWhere);
            }
            return await up.ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public new async Task<bool> DeleteAsync(TEntity entity)
        {
            return await Context.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public new async Task<bool> DeleteByIdAsync(object id)
        {
            return await Context.Deleteable<TEntity>(id).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public new async Task<bool> DeleteByIdsAsync(object[] ids)
        {
            return await Context.Deleteable<TEntity>().In(ids).ExecuteCommandHasChangeAsync();
        }



        /// <summary>
        /// 功能描述:查询所有数据
        /// </summary>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync()
        {
            return await Context.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(string strWhere)
        {
            return await Context.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await Context.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }
        public async Task<TEntity> QueryFistAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await Context.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).FirstAsync();
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await Context.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(strOrderByFileds != null, strOrderByFileds).ToListAsync();
        }
        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await Context.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(string strWhere, string strOrderByFileds)
        {
            return await Context.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToListAsync();
        }


        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(
            Expression<Func<TEntity, bool>> whereExpression,
            int intTop,
            string strOrderByFileds)
        {
            return await Context.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(
            string strWhere,
            int intTop,
            string strOrderByFileds)
        {
            return await Context.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToListAsync();
        }



        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(
            Expression<Func<TEntity, bool>> whereExpression,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds)
        {
            return await Context.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToPageListAsync(intPageIndex, intPageSize);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(
          string strWhere,
          int intPageIndex,
          int intPageSize,

          string strOrderByFileds)
        {

            return await Context.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToPageListAsync(intPageIndex, intPageSize);
        }




        /// <summary> 
        ///查询-多表查询
        /// </summary> 
        /// <typeparam name="T">实体1</typeparam> 
        /// <typeparam name="T2">实体2</typeparam> 
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param> 
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param> 
        /// <returns>值</returns>
        public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return await Context.Queryable(joinExpression).Select(selectExpression).ToListAsync();
            }
            return await Context.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).ToListAsync();
        }

        /// <summary>
        /// 根据Sql查询集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QuerySqlAsync(string sql)
        {
            return await Context.SqlQueryable<TEntity>(sql).ToListAsync();
        }





        /// <summary>
        /// ADO 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> SqlQueryToListAsync(string sql)
        {
            return await Context.Ado.SqlQueryAsync<TEntity>(sql);
        }
        /// <summary>
        /// ADO 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<T>> SqlQueryToListAsync<T>(string sql)
        {
            return await Context.Ado.SqlQueryAsync<T>(sql);
        }


        /// <summary>
        /// ADO 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<TEntity> SqlQueryAsync(string sql)
        {
            return await Context.Ado.SqlQuerySingleAsync<TEntity>(sql);
        }
        /// <summary>
        /// ADO 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public async Task<T> SqlQueryAsync<T>(string sql)
        {
            return await Context.Ado.SqlQuerySingleAsync<T>(sql);
        }

        /// <summary>
        /// 查询返回 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns>返回List 集合</returns>
        public async Task<List<T>> SqlQuerysAsync<T>(string sql)
        {
            return await Context.Ado.SqlQueryAsync<T>(sql);
        }

        /// <summary>
        /// ADO 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pairs"></param>
        /// <returns>返回List 集合</returns>
        public async Task<List<T>> SqlQueryAsync<T>(string sql, Dictionary<string, object> pairs)
        {
            return await Context.Ado.SqlQueryAsync<T>(sql, FormationParameter(pairs).ToArray());
        }
        /// <summary>
        /// ADO 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pairs"></param>
        /// <returns>返回List 集合</returns>
        public async Task<List<TEntity>> SqlQueryAsync(string sql, Dictionary<string, object> pairs)
        {
            return await Context.Ado.SqlQueryAsync<TEntity>(sql, FormationParameter(pairs).ToArray());
        }

        /// <summary>
        /// ADO 执行 增删改方法
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> ExecuteCommandAsync(string sql)
        {
            return await Context.Ado.ExecuteCommandAsync(sql);
        }

        /// <summary>
        /// ADO 执行 增删改方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public async Task<int> ExecuteCommandAsync(string sql, Dictionary<string, object> pairs)
        {
            return await Context.Ado.ExecuteCommandAsync(sql, FormationParameter(pairs).ToArray());
        }

        /// <summary>
        /// 获取序列自增ID
        /// </summary>
        /// <param name="tname">序列名</param>
        /// <param name="sequenceName">对应表名称</param>
        /// <returns></returns>
        public long getquery_id(string sequenceName, string tableName)
        {
            long myvar = 0;
            while (true)
            {
                string sql = "select " + sequenceName + ".NEXTVAL from dual ";
                try
                {
                    myvar = Context.Ado.SqlQuerySingle<long>(sql);

                    if (myvar > 0)
                    {
                        sql = "select count(*) from " + tableName + " where ID=" + myvar.ToString();
                        long myids = Context.Ado.SqlQuerySingle<long>(sql);
                        if (myids == 0)
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"getquery_id,获取序列异常,【序列获取失败】");
                }
            }
            return myvar;
        }


        private List<SugarParameter> FormationParameter(Dictionary<string, object> pairs)
        {
            List<SugarParameter> olist = new List<SugarParameter>();
            if (pairs.Count > 0)
            {
                foreach (var item in pairs.Keys)
                {
                    olist.Add(new SugarParameter(item, pairs[item].ToString()));
                }
            }
            return olist;
        }

        /// <summary>
        ///  执行包体 方法（存储过程）
        /// </summary>
        /// <param name="packFuncName">imr_pkg_mr_data_syn.SP_SYN_SYS_USERS</param>
        /// <returns></returns>
        public System.Data.DataTable ExecStoredProcedure(string packFuncName, List<SugarParameter> olist)
        {
            try
            {
                System.Data.DataTable pack = Context.Ado.UseStoredProcedure().GetDataTable(packFuncName, olist.ToArray());
                return pack;
            }
            catch (Exception err)
            {

                throw err;
            }
        }

        public void ExecuteTranscationByCap<TEntity>(string publishName, TEntity contentObj, Action<ISqlSugarClient, TEntity> action = null, IDictionary<string, string> headers = null)
        {
            OdinInjectCore.GetService<IOdinCapEventBus>().CapTransactionPublish<TEntity>(
                publishName,
                contentObj,
                action,
                headers
            );
        }
    }
}