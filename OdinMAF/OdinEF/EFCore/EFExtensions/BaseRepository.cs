using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using OdinPlugs.OdinMAF.OdinEF.EFCore.EFExtensions.EFInterface;
using SqlSugar;
namespace OdinPlugs.OdinMAF.OdinEF.EFCore.EFExtensions
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private DbContext db { get; set; }
        public BaseRepository(DbContext _dbContext)
        {
            db = _dbContext;
        }

        /// <summary>  
        /// 新增实体，返回受影响的行数  
        /// </summary>  
        /// <param name="model"></param>  
        /// <returns>返回受影响的行数</returns>  
        public int Add(T model)
        {
            db.Set<T>().Add(model);
            return db.SaveChanges();
        }

        /// <summary>  
        ///新增实体，返回对应的实体对象  
        /// </summary>  
        /// <param name="model"></param>  
        /// <returns>新增的实体对象</returns>  
        public T AddReturnModel(T model)
        {
            db.Set<T>().Add(model);
            db.SaveChanges();
            return model;
        }

        /// <summary>  
        /// 批量新增实体
        /// </summary>  
        /// <typeparam name="T">泛型类型参数</typeparam>  
        /// <param name="entityList">待添加的实体集合</param>  
        /// <returns></returns>  
        public int AddRange(List<T> entityList)
        {
            db.Set<T>().AddRange(entityList);
            return db.SaveChanges();
        }

        /// <summary>
        /// 批量的新增实体(带事务)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool AddRangeTransaction(List<T> entityList)
        {
            using (TransactionScope Ts = new TransactionScope(TransactionScopeOption.Required))
            {
                db.Set<T>().AddRange(entityList);
                int Count = db.SaveChanges();
                Ts.Complete();
                return Count > 0;
            }
        }

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回受影响的行数</returns>
        public int DelById(int id)
        {
            var model = GetById(id);
            if (model == null || Convert.ToInt32(model.GetType().GetProperty("id").GetValue(model)) <= 0) return -1;
            db.Set<T>().Remove(model);
            return db.SaveChanges();
        }

        /// <summary>  
        /// 根据模型删除数据
        /// </summary>  
        /// <param name="model">该模型对象必须包含id值</param>  
        /// <returns>返回受影响的行数</returns>  
        public int Delete(T model)
        {
            db.Set<T>().Attach(model);
            db.Set<T>().Remove(model);
            return db.SaveChanges();
        }

        /// <summary>  
        /// 根据条件删除数据 （支持多条件查询）
        /// </summary>  
        /// <param name="delWhere"></param>  
        /// <returns>返回受影响的行数</returns>  
        public int Delete(Expression<Func<T, bool>> whereLambda)
        {
            //查询要删除的数据  
            List<T> listDeleting = db.Set<T>().Where(whereLambda).ToList();
            //将要删除的数据 用删除方法添加到 EF 容器中  
            listDeleting.ForEach(u =>
            {
                db.Set<T>().Attach(u); //先附加到EF 容器  
                db.Set<T>().Remove(u); //标识为删除状态  
            });
            return db.SaveChanges();
        }

        /// <summary>  
        /// 修改实体  
        /// </summary>  
        /// <param name="model">该模型对象必须包含id值</param>  
        /// <returns>返回受影响的行数</returns>  
        public int Edit(T model)
        {
            var entry = db.Entry<T>(model);
            entry.State = EntityState.Modified;
            return db.SaveChanges();
        }

        /// <summary>  
        /// 修改实体，可修改指定属性  
        /// </summary>  
        /// <param name="model">该模型对象必须包含id值</param>  
        /// <param name="propertyName">要修改的属性名称数组</param>  
        /// <returns></returns>  
        public int Edit(T model, params string[] propertyNames)
        {
            var entry = db.Entry<T>(model); //将对象添加到EF中             
            entry.State = EntityState.Unchanged; //先设置对象的包装状态为 Unchanged            
            foreach (string propertyName in propertyNames) //循环被修改的属性名数组 
            {
                //将每个被修改的属性的状态设置为已修改状态；这样在后面生成的修改语句时，就只为标识为已修改的属性更新  
                entry.Property(propertyName).IsModified = true;
            }
            return db.SaveChanges();
        }

        /// <summary>  
        /// 批量修改  （支持多条件查询）
        /// </summary>  
        /// <param name="model"></param>  
        /// <param name="whereLambda"></param>  
        /// <param name="modifiedPropertyNames"></param>  
        /// <returns></returns>  
        public int EditBatch(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedPropertyNames)
        {
            List<T> listModifing = db.Set<T>().Where(whereLambda).ToList(); //查询要修改的数据              
            Type t = typeof(T); //获取实体类类型对象             
            List<PropertyInfo> propertyInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList(); //获取实体类所有的公共属性 
            Dictionary<string, PropertyInfo> dicPropertys = new Dictionary<string, PropertyInfo>(); //创建实体属性字典集合  
                                                                                                    //将实体属性中要修改的属性名 添加到字典集合中  键：属性名  值：属性对象  
            propertyInfos.ForEach(p =>
            {
                if (modifiedPropertyNames.Contains(p.Name))
                {
                    dicPropertys.Add(p.Name, p);
                }
            });

            foreach (string propertyName in modifiedPropertyNames) //循环要修改的属性名  
            {
                if (dicPropertys.ContainsKey(propertyName)) //判断要修改的属性名是否在实体类的属性集合中存在  
                {
                    PropertyInfo proInfo = dicPropertys[propertyName]; //如果存在，则取出要修改的属性对象                      
                    object newValue = proInfo.GetValue(model, null); //取出要修改的值                    
                    foreach (T item in listModifing) //批量设置要修改对象的属性  
                    {
                        proInfo.SetValue(item, newValue, null); // 为要修改的对象的要修改的属性设置新的值
                    }
                }
            }
            return db.SaveChanges();
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public int EditBatch(List<T> entityList)
        {
            foreach (var entity in entityList)
            {
                var entry = db.Entry(entity);
                entry.State = EntityState.Modified;
            }
            return db.SaveChanges();

        }

        /// <summary>
        /// 批量的进行更新数据 (带事务)
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool EditBatchTransaction(List<T> EntityList)
        {
            int Count = 0;
            using (TransactionScope Ts = new TransactionScope(TransactionScopeOption.Required))
            {
                if (EntityList != null)
                {
                    foreach (var items in EntityList)
                    {
                        var EntityModel = db.Entry(EntityList);
                        db.Set<T>().Attach(items);
                        EntityModel.State = EntityState.Modified;
                    }
                }
                Count = db.SaveChanges();
                Ts.Complete();
            }
            return Count > 0;
        }

        /// <summary>
        /// 根据Id查询单个Model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(int id)
        {
            return db.Set<T>().FirstOrDefault(r => Convert.ToInt32(r.GetType().GetProperty("id").GetValue(r)) == id);
        }

        /// <summary>  
        /// 根据条件查询单个model （支持多条件查询）
        /// </summary>  
        /// <param name="whereLambda"></param>  
        /// <returns></returns>  
        public T Get(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).FirstOrDefault();
        }

        /// <summary>  
        /// 根据条件查询单个model (支持多条件查询，支持多列排序)  
        /// </summary>  
        /// <typeparam name="TKey"></typeparam>  
        /// <param name="whereLambda">查询条件</param>  
        /// <param name="orderLambda">排序条件</param>  
        /// <param name="isAsc"></param>  
        /// <returns></returns>  
        public T Get(Expression<Func<T, bool>> whereLambda, Action<IOrderable<T>> orderBy = null)
        {
            IQueryable<T> QueryList = db.Set<T>();
            if (whereLambda != null)
            {
                QueryList = QueryList.Where(whereLambda);
            }
            if (orderBy != null)
            {
                var linq = new Orderable<T>(QueryList);
                orderBy(linq);
                return linq.Queryable.FirstOrDefault();
            }
            return QueryList.FirstOrDefault();
        }

        /// <summary>  
        /// 根据条件查询单个model (支持多条件查询，仅支持单列排序)  
        /// </summary>  
        /// <typeparam name="TKey"></typeparam>  
        /// <param name="whereLambda">查询条件</param>  
        /// <param name="orderLambda">排序条件</param>  
        /// <param name="isAsc">是否为升序排序，默认true</param>  
        /// <returns></returns>  
        public T Get<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true)
        {
            IQueryable<T> Query = isAsc == true ? db.Set<T>().OrderBy(orderByLambda) : db.Set<T>().OrderByDescending(orderByLambda);
            if (whereLambda != null)
            {
                Query = Query.Where(whereLambda);
            }
            return Query.FirstOrDefault();
        }

        public E GetSelect<E>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, E>> selectLambda)
        {

            IQueryable<T> Query = db.Set<T>();
            if (whereLambda != null)
            {
                Query = Query.Where(whereLambda);
            }
            E selectQuery = Query.Select(selectLambda).SingleOrDefault();
            return selectQuery;
        }

        public E GetSelect<E, TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, E>> selectLambda,
            Expression<Func<T, TKey>> orderByLambda, bool isAsc = true)
        {

            IQueryable<T> Query = db.Set<T>();
            Query = isAsc == true ? Query.OrderBy(orderByLambda) : Query.OrderByDescending(orderByLambda);
            if (whereLambda != null)
            {
                Query = Query.Where(whereLambda);
            }
            E selectQuery = Query.Select(selectLambda).SingleOrDefault();
            return selectQuery;
        }

        public IQueryable<E> GetSelectList<E>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, E>> selectLambda)
        {
            IQueryable<T> Query = db.Set<T>();
            if (whereLambda != null)
            {
                Query = Query.Where(whereLambda);
            }
            IQueryable<E> selectQuery = Query.Select(selectLambda);
            return selectQuery;
        }

        public IQueryable<E> GetSelectList<E, TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, E>> selectLambda,
            Expression<Func<T, TKey>> orderByLambda, bool isAsc = true)
        {
            IQueryable<T> Query = db.Set<T>();
            Query = isAsc == true ? db.Set<T>().OrderBy(orderByLambda) : db.Set<T>().OrderByDescending(orderByLambda);
            if (whereLambda != null)
            {
                Query = Query.Where(whereLambda);
            }
            IQueryable<E> selectQuery = Query.Select(selectLambda);
            return selectQuery;
        }

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return db.Set<T>();
        }

        /// <summary>
        /// 获取查询条件的数据总条数 （支持多条件查询）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public int GetCount(Expression<Func<T, bool>> whereLambda = null)
        {
            return whereLambda != null ? db.Set<T>().Where(whereLambda).Count() : db.Set<T>().Count();
        }

        /// <summary>
        /// 判断对象是否存在 （支持多条件查询）
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns>对象存在则返回true，不存在则返回false</returns>
        public bool GetAny(Expression<Func<T, bool>> whereLambda = null)
        {
            return whereLambda != null ? db.Set<T>().Where(whereLambda).Any() : db.Set<T>().Any();
        }

        /// <summary>  
        /// 获取数据集合 (支持多条件查询) 
        /// </summary>  
        /// <param name="whereLambda"></param>  
        /// <returns></returns>  
        public IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda);
        }

        /// <summary>  
        ///  获取数据集合 （支持多条件查询，仅支持单条件排序）
        /// </summary>  
        /// <typeparam name="TKey"></typeparam>  
        /// <param name="whereLambda"></param>  
        /// <param name="orderLambda"></param>  
        /// <param name="isAsc">是否为升序排序，默认为true</param>  
        /// <returns></returns>  
        public IQueryable<T> GetList<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true)
        {
            IQueryable<T> QueryList = isAsc == true ? db.Set<T>().OrderBy(orderByLambda) : db.Set<T>().OrderByDescending(orderByLambda);
            if (whereLambda != null)
            {
                QueryList = QueryList.Where(whereLambda);
            }
            return QueryList;

        }

        /// <summary>
        /// 获取数据集合 （支持多条件查询，支持多列排序）
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda, Action<IOrderable<T>> orderBy)
        {
            IQueryable<T> QueryList = db.Set<T>();
            if (whereLambda != null)
            {
                QueryList = QueryList.Where(whereLambda);
            }
            var linq = new Orderable<T>(QueryList);
            orderBy(linq);
            return linq.Queryable;
        }

        /// <summary>
        /// 获取数据集合 （支持多条件查询，支持多列排序）
        /// </summary>
        /// <typeparam name="Entity">目标类型：说明：将查询出来的数据转换成目标类型实体</typeparam>
        /// <param name="orderBy">排序（可选）</param>
        /// <returns></returns>
        public IQueryable<E> GetList<E>(string sql, Action<IOrderable<E>> orderBy = null, params DbParameter[] parameters) where E : class, new()
        {
            IQueryable<E> QueryList = db.Database.SqlQuery<E>(sql, parameters.ToArray()).AsQueryable();
            if (orderBy != null)
            {
                var linq = new Orderable<E>(QueryList);
                orderBy(linq);
                return linq.Queryable;
            }
            return QueryList;
        }

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
        public IQueryable<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true)
        {
            //分页的时候一定要注意 Order一定在Skip 之前
            IQueryable<T> QueryList = isAsc == true ? db.Set<T>().OrderBy(orderByLambda) : db.Set<T>().OrderByDescending(orderByLambda);

            if (whereLambda != null)
            {
                QueryList = QueryList.Where(whereLambda);
            }
            return QueryList.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        }

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
        public IQueryable<T> GetPagedList<TKey>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, TKey>> orderByLambda, Expression<Func<T, bool>> whereLambda = null, bool isAsc = true)
        {
            //分页的时候一定要注意 Order一定在Skip 之前
            IQueryable<T> QueryList = isAsc == true ? db.Set<T>().OrderBy(orderByLambda) : db.Set<T>().OrderByDescending(orderByLambda);
            if (whereLambda != null)
            {
                QueryList = QueryList.Where(whereLambda);
            }
            totalCount = QueryList.Count();
            return QueryList.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        }

        /// <summary>
        /// 分页查询，带输出数据总条数  (支持多条件查询，支持多列排序)
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示数据的条数</param>
        /// <param name="totalCount">输出数据：数据总条数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public IQueryable<T> GetPagedList(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda = null, Action<IOrderable<T>> orderBy = null)
        {
            IQueryable<T> QueryList = db.Set<T>();
            if (whereLambda != null)
            {
                QueryList = QueryList.Where(whereLambda);
            }
            totalCount = QueryList.Count();
            if (orderBy != null)
            {
                var linq = new Orderable<T>(QueryList);
                orderBy(linq);
                return linq.Queryable.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            }
            return QueryList.Skip(pageSize * (pageIndex - 1)).Take(pageSize); ;
        }

        /// <summary>
        /// 执行存储过程的SQL 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Sql">执行的SQL语句</param>
        /// <param name="Parms">SQL 语句的参数</param>
        /// <param name="CmdType"> SQL命令（默认为Text）</param>
        /// <returns></returns>
        public IQueryable<T> ProQuery(string Sql, List<DbParameter> Parms, CommandType CmdType = CommandType.Text)
        {
            //进行执行存储过程
            if (CmdType == CommandType.StoredProcedure)
            {
                StringBuilder paraNames = new StringBuilder();
                if (Parms != null)
                {
                    foreach (var item in Parms)
                    {
                        paraNames.Append($" @{item},");
                    }
                }
                Sql = paraNames != null && paraNames.Length > 0 ? $"exec {Sql} {paraNames.ToString().Trim(',')}" : $"exec {Sql} ";
            }
            return db.Database.SqlQuery<T>(Sql, Parms.ToArray()).AsQueryable();
        }

        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素
        /// </summary>
        /// <typeparam name="T">查询所返回对象的类型</typeparam>
        /// <param name="sql">SQL 查询字符串</param>
        /// <param name="parameters">要应用于 SQL 查询字符串的参数</param>
        /// <returns></returns>
        public IQueryable<T> SqlQuery(string sql, params DbParameter[] parameters)
        {
            return db.Database.SqlQuery<T>(sql, parameters).AsQueryable();
        }

        /// <summary>
        /// 创建一个原始 SQL 用户 新增，删除，编辑
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteSqlCommand(string sql, params DbParameter[] parameters)
        {
            return db.Database.ExecuteSqlRaw(sql, parameters);
        }

        /// <summary>  
        /// 获取带 in 的sql参数列表  
        /// </summary>  
        /// <param name="sql">带in ( {0} )的sql</param>  
        /// <param name="ids">以逗号分隔的id字符串</param>  
        /// <returns>sql参数列表</returns>  
        public DbParameter[] GetWithInSqlParameters(ref string sql, string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return null;
            }
            string[] idArr = ids.Split(',');
            //组建sql在in中的字符串  
            StringBuilder sbCondition = new StringBuilder();
            List<DbParameter> spList = new List<DbParameter>();
            for (int i = 0; i < idArr.Length; i++)
            {
                string id = idArr[i];
                sbCondition.Append("@id" + i + ",");
                spList.Add(new MySqlParameter("@id" + i.ToString(), id));
            }
            //重新构建sql  
            sql = string.Format(sql, sbCondition.ToString().TrimEnd(','));
            return spList.ToArray();
        }

        public bool EFTransaction(Action efTrans, Action successAction = null, Action errorAction = null)
        {
            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    efTrans();
                    tran.Commit();
                    if (successAction != null)
                        successAction();
                    return true;
                }
                catch (Exception)
                {
                    tran.Rollback();
                    if (errorAction != null)
                        errorAction();
                    return false;
                }
            }
        }
    }
}