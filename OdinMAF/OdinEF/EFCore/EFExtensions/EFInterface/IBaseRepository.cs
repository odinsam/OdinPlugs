using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace OdinPlugs.OdinMAF.OdinEF.EFCore.EFExtensions.EFInterface
{
    public interface IBaseRepository<T>
    {
        /// <summary>  
        /// 新增实体，返回受影响的行数  
        /// </summary>  
        /// <param name="model"></param>  
        /// <returns>返回受影响的行数</returns>  
        int Add(T model);

        /// <summary>  
        ///新增实体，返回对应的实体对象  
        /// </summary>  
        /// <param name="model"></param>  
        /// <returns>新增的实体对象</returns>  
        T AddReturnModel(T model);

        /// <summary>  
        /// 批量新增实体  
        /// </summary>  
        /// <typeparam name="T">泛型类型参数</typeparam>  
        /// <param name="entityList">待添加的实体集合</param>  
        /// <returns></returns>  
        int AddRange(List<T> entityList);

        /// <summary>
        /// 批量的插入数据(带事务)
        /// </summary>
        /// <param name="entityList">待添加的实体集合</param>
        /// <returns>是否成功插入 true：是 false：否</returns>
        bool AddRangeTransaction(List<T> entityList);

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回受影响的行数</returns>
        int DelById(int id);

        /// <summary>  
        /// 根据模型删除数据
        /// </summary>  
        /// <param name="model">该模型对象必须包含id值</param>  
        /// <returns>返回受影响的行数</returns>  
        int Delete(T model);

        /// <summary>  
        /// 根据条件删除数据 （支持多条件查询）
        /// </summary>  
        /// <param name="delWhere"></param>  
        /// <returns>返回受影响的行数</returns>  
        int Delete(Expression<Func<T, bool>> whereLambda);

        /// <summary>  
        /// 修改实体  
        /// </summary>  
        /// <param name="model">该模型对象必须包含id值</param>  
        /// <returns>返回受影响的行数</returns>  
        int Edit(T model);

        /// <summary>  
        /// 修改实体，可修改指定属性  
        /// </summary>  
        /// <param name="model">该模型对象必须包含id值</param>  
        /// <param name="propertyName">要修改的属性名称数组</param>  
        /// <returns></returns>  
        int Edit(T model, params string[] propertyNames);

        /// <summary>  
        /// 批量修改  （支持多条件查询）
        /// </summary>  
        /// <param name="model"></param>  
        /// <param name="whereLambda">条件查询表达式</param>  
        /// <param name="modifiedPropertyNames">要修改的属性名称数组</param>  
        /// <returns></returns>  
        int EditBatch(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedPropertyNames);

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entityList">待修改的实体集合</param>
        /// <returns></returns>
        int EditBatch(List<T> entityList);

        /// <summary>
        /// 批量的进行更新数据 (带事务)
        /// </summary>
        /// <param name="EntityList">待修改的实体集合</param>
        /// <returns>是否更新成功 true:是 false:否</returns>
        bool EditBatchTransaction(List<T> EntityList);

        /// <summary>
        /// 根据Id查询单个Model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(int id);

        /// <summary>  
        /// 根据条件查询单个model （支持多条件查询）
        /// </summary>  
        /// <param name="whereLambda"></param>  
        /// <returns></returns>  
        T Get(Expression<Func<T, bool>> whereLambda);

        /// <summary>  
        /// 根据条件查询单个model (支持多条件查询，支持多列排序)  
        /// </summary>  
        /// <typeparam name="TKey"></typeparam>  
        /// <param name="whereLambda">查询条件</param>  
        /// <param name="orderLambda">排序条件</param>  
        /// <param name="isAsc"></param>  
        /// <returns></returns>  
        T Get(Expression<Func<T, bool>> whereLambda, Action<IOrderable<T>> orderBy = null);

        /// <summary>  
        /// 根据条件查询单个model (支持多条件查询，仅支持单列排序)  
        /// </summary>  
        /// <typeparam name="TKey"></typeparam>  
        /// <param name="whereLambda">查询条件</param>  
        /// <param name="orderLambda">排序条件</param>  
        /// <param name="isAsc">是否为升序排序，默认true</param>  
        /// <returns></returns>  
        T Get<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true);

        E GetSelect<E>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, E>> selectLambda);

        E GetSelect<E, TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, E>> selectLambda,
            Expression<Func<T, TKey>> orderByLambda, bool isAsc = true);

        IQueryable<E> GetSelectList<E>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, E>> selectLambda);

        IQueryable<E> GetSelectList<E, TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, E>> selectLambda,
            Expression<Func<T, TKey>> orderByLambda, bool isAsc = true);

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// 获取查询条件的数据总条数 （支持多条件查询）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        int GetCount(Expression<Func<T, bool>> whereLambda = null);

        /// <summary>
        /// 判断对象是否存在 （支持多条件查询）
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns>对象存在则返回true，不存在则返回false</returns>
        bool GetAny(Expression<Func<T, bool>> whereLambda = null);

        /// <summary>  
        /// 获取数据集合 (支持多条件查询) 
        /// </summary>  
        /// <param name="whereLambda"></param>  
        /// <returns></returns>  
        IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda);

        /// <summary>  
        ///  获取数据集合 （支持多条件查询，仅支持单条件排序）
        /// </summary>  
        /// <typeparam name="TKey"></typeparam>  
        /// <param name="whereLambda"></param>  
        /// <param name="orderLambda"></param>  
        /// <param name="isAsc">是否为升序排序，默认为true</param>  
        /// <returns></returns>  
        IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true);

        /// <summary>
        /// 获取数据集合 （支持多条件查询，支持多列排序）
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda, Action<IOrderable<T>> orderBy);

        /// <summary>
        /// 获取数据集合 （支持多条件查询，支持多列排序）
        /// </summary>
        /// <typeparam name="E">目标类型：说明：将查询出来的数据转换成目标类型实体</typeparam>
        /// <param name="orderBy">排序（可选）</param>
        /// <returns></returns>
        IQueryable<E> GetList<E>(string sql, Action<IOrderable<E>> orderBy = null, params DbParameter[] dbParams) where E : class,
        new();

        /// <summary>
        /// 分页查询 （支持多条件查询，仅支持单列排序）
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示数据的条数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderByLambda">排序条件</param>
        /// <param name="isAsc">是否为升序排序，默认为true</param>
        /// <returns></returns>
        IQueryable<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true);

        /// <summary>
        /// 分页查询 ,带输出数据总条数 （支持多条件查询，仅支持单列排序）
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示数据的条数</param>
        /// <param name="totalCount">数据总条数</param>
        /// <param name="orderByLambda">排序条件</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="isAsc">是否为升序排序，默认为true</param>
        /// <returns></returns>
        IQueryable<T> GetPagedList<TKey>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, TKey>> orderByLambda, Expression<Func<T, bool>> whereLambda = null, bool isAsc = true);

        /// <summary>
        /// 分页查询，带输出数据总条数  (支持多条件查询，支持多列排序)
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示数据的条数</param>
        /// <param name="totalCount">输出数据：数据总条数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        IQueryable<T> GetPagedList(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda = null, Action<IOrderable<T>> orderBy = null);

        /// <summary>
        /// 执行存储过程的SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Sql">执行的SQL语句</param>
        /// <param name="Parms">SQL 语句的参数</param>
        /// <param name="CmdType"> SQL命令（默认为Text）</param>
        /// <returns></returns>
        IQueryable<T> ProQuery(string Sql, List<DbParameter> Parms, CommandType CmdType = CommandType.Text);

        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。
        /// </summary>
        /// <typeparam name="T">查询所返回对象的类型</typeparam>
        /// <param name="sql">SQL 查询字符串</param>
        /// <param name="parameters">要应用于 SQL 查询字符串的参数</param>
        /// <returns></returns>
        IQueryable<T> SqlQuery(string sql, params DbParameter[] parameters);

        /// <summary>
        /// 创建一个原始 SQL 用户 新增，删除，编辑
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>返回受影响的行数</returns>
        int ExecuteSqlCommand(string sql, params DbParameter[] parameters);

        /// <summary>  
        /// 获取带 in 的sql参数列表  
        /// </summary>  
        /// <param name="sql">带in ( {0} )的sql</param>  
        /// <param name="ids">以逗号分隔的id字符串</param>  
        /// <returns>sql参数列表</returns>  
        DbParameter[] GetWithInSqlParameters(ref string sql, string ids);

        bool EFTransaction(Action efTrans, Action successAction = null, Action errorAction = null);

    }
}