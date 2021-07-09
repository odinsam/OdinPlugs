using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OdinPlugs.OdinInject.InjectInterface;
using SqlSugar;

namespace OdinPlugs.OdinMAF.OdinEF.EFCore.EFExtensions.EFInterface
{
    public interface ISqlSugarBaseRepository<TEntity> : IAutoInject
    {
        void BeginTran();

        void Commit();

        void Rollback();

        /// <summary>
        /// 功能描述:根据ID查新数据
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns></returns>
        Task<TEntity> QueryByIdAsync(object objId);

        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        Task<TEntity> QueryByIdAsync(object objId, bool blnUseCache = false);

        /// <summary>
        /// 功能描述:根据ID查询数据
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        Task<List<TEntity>> QueryByIdsAsync(object[] lstIds);


        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<long> AddAsync(TEntity entity);


        /// <summary>
        /// 写入实体数据(同步方法)
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        long Add(TEntity entity);



        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>返回自增量列</returns>
        Task<int> AddAsync(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null);


        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>返回被影响的行数</returns>
        Task<int> AddByRowsAsync(TEntity entity);

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="listEntity">实体类集合</param>
        /// <returns>返回被影响的行数</returns>
        int AddByRowsAsync(List<TEntity> listEntity);


        /// <summary>
        /// 批量插入实体(速度快)
        /// </summary>
        /// <param name="listEntity">实体集合</param>
        /// <returns>影响行数</returns>
        Task<int> AddAsync(List<TEntity> listEntity);


        int Add(List<TEntity> listEntity);


        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity);

        bool IsUpdate(List<TEntity> entities);

        /// <summary>
        /// 同步 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(TEntity entity);

        /// <summary>
        /// 同步 更新批量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(List<TEntity> entities);


        /// <summary>
        /// 异步 更新实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(List<TEntity> entities);


        Task<bool> UpdateAsync(TEntity entity, string strWhere);


        Task<bool> UpdateAsync(string strSql, SugarParameter[] parameters = null);


        Task<bool> UpdateAsync(object operateAnonymousObjects);


        Task<bool> UpdateAsync(
          TEntity entity,
          List<string> lstColumns = null,
          List<string> lstIgnoreColumns = null,
          string strWhere = ""
            );


        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TEntity entity);


        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(object id);


        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(object[] ids);




        /// <summary>
        /// 功能描述:查询所有数据
        /// </summary>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync();


        /// <summary>
        /// 功能描述:查询数据列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(string strWhere);


        /// <summary>
        /// 功能描述:查询数据列表
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression);

        Task<TEntity> QueryFistAsync(Expression<Func<TEntity, bool>> whereExpression);


        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);

        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);


        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(string strWhere, string strOrderByFileds);



        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(
            Expression<Func<TEntity, bool>> whereExpression,
            int intTop,
            string strOrderByFileds);


        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(
            string strWhere,
            int intTop,
            string strOrderByFileds);




        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(
            Expression<Func<TEntity, bool>> whereExpression,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds);


        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        Task<List<TEntity>> QueryAsync(
          string strWhere,
          int intPageIndex,
          int intPageSize,

          string strOrderByFileds);





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
        Task<List<TResult>> QueryMuchAsync<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();


        /// <summary>
        /// 根据Sql查询集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<TEntity>> QuerySqlAsync(string sql);






        /// <summary>
        /// ADO 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<TEntity>> SqlQueryToListAsync(string sql);

        /// <summary>
        /// ADO 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<TEntity>> SqlQueryToListAsync<TEntity>(string sql);



        /// <summary>
        /// ADO 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<TEntity> SqlQueryAsync(string sql);

        /// <summary>
        /// ADO 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pairs"></param>
        /// /// <returns></returns>
        Task<TEntity> SqlQueryAsync<TEntity>(string sql);


        /// <summary>
        /// 查询返回 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns>返回List 集合</returns>
        Task<List<TEntity>> SqlQuerysAsync<TEntity>(string sql);


        /// <summary>
        /// ADO 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pairs"></param>
        /// <returns>返回List 集合</returns>
        Task<List<TEntity>> SqlQueryAsync<TEntity>(string sql, Dictionary<string, object> pairs);

        /// <summary>
        /// ADO 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pairs"></param>
        /// <returns>返回List 集合</returns>
        Task<List<TEntity>> SqlQueryAsync(string sql, Dictionary<string, object> pairs);


        /// <summary>
        /// ADO 执行 增删改方法
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> ExecuteCommandAsync(string sql);


        /// <summary>
        /// ADO 执行 增删改方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        Task<int> ExecuteCommandAsync(string sql, Dictionary<string, object> pairs);

        /// <summary>
        /// 获取序列自增ID
        /// </summary>
        /// <param name="tname">序列名</param>
        /// <param name="sequenceName">对应表名称</param>
        /// <returns></returns>
        long getquery_id(string sequenceName, string tableName);

        /// <summary>
        ///  执行包体 方法（存储过程）
        /// </summary>
        /// <param name="packFuncName">imr_pkg_mr_data_syn.SP_SYN_SYS_USERS</param>
        /// <returns></returns>
        System.Data.DataTable ExecStoredProcedure(string packFuncName, List<SugarParameter> olist);


    }
}