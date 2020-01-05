using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Office.Work.Platform.Api.DataService
{
    public class GHDbOperate
    {
        private readonly GHDbContext _ghDbContext;

        public GHDbOperate(GHDbContext ghDbContext)
        {
            _ghDbContext = ghDbContext;

        }
        #region 数据查询操作
        /// <summary>
        /// 返回指定类的DbSet；
        /// </summary>
        /// <returns></returns>
        public DbSet<T> GetDbSet<T>() where T : class
        {
            return _ghDbContext.Set<T>();
        }
        /// <summary>
        /// 返回指定类的DbSet AsNoTracking；
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetIQueryable<T>() where T : class
        {
            try
            {
                return _ghDbContext.Set<T>().AsNoTracking();
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }
        }
        /// <summary>
        /// 根据Key值查找P_Entity
        /// </summary>
        /// <param name="dbKey">数据库key</param>
        /// <returns></returns>
        public T GetEntity<T, TKey>(TKey dbKey) where T : class
        {
            try
            {
                return _ghDbContext.Find<T>(dbKey);
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }

        }

        /// <summary>
        /// 根据条件查询单个P_Entity
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T GetEntity<T>(Expression<Func<T, bool>> whereLambda) where T : class
        {
            try
            {
                return _ghDbContext.Set<T>().Where(whereLambda).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }

        }
        /// <summary>
        /// 根据条件查询单个P_Entity（AsNoTracking）
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T GetEntityNoTracking<T>(Expression<Func<T, bool>> whereLambda) where T : class
        {
            try
            {
                return _ghDbContext.Set<T>().Where(whereLambda).AsNoTracking().FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }

        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<List<T>> GetList<T>() where T : class
        {
            try
            {
                return await _ghDbContext.Set<T>().AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }

        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListBy<T>(Expression<Func<T, bool>> whereLambda) where T : class
        {
            try
            {
                return await _ghDbContext.Set<T>().Where(whereLambda).AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }

        }
        /// <summary>
        /// 根据条件查询，并排序
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListBy<T, TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true) where T : class
        {
            try
            {
                if (isAsc)
                {
                    return await _ghDbContext.Set<T>().Where(whereLambda).OrderBy(orderLambda).AsNoTracking().ToListAsync();
                }
                else
                {
                    return await _ghDbContext.Set<T>().Where(whereLambda).OrderByDescending(orderLambda).AsNoTracking().ToListAsync();
                }
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }
        }

        /// <summary>
        /// 根据条件查询Top多少个，并排序
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="top"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListBy<T, TKey>(int top, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true) where T : class
        {
            try
            {
                if (isAsc)
                {
                    return await _ghDbContext.Set<T>().Where(whereLambda).OrderBy(orderLambda).Take(top).AsNoTracking().ToListAsync();
                }
                else
                {
                    return await _ghDbContext.Set<T>().Where(whereLambda).OrderByDescending(orderLambda).Take(top).AsNoTracking().ToListAsync();
                }
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }
        }
        /// <summary>
        /// 根据条件排序查询  双排序
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda1"></param>
        /// <param name="orderLambda2"></param>
        /// <param name="isAsc1"></param>
        /// <param name="isAsc2"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListBy<T, TKey1, TKey2>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey1>> orderLambda1, Expression<Func<T, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true) where T : class
        {
            try
            {
                if (isAsc1)
                {
                    if (isAsc2)
                    {
                        return await _ghDbContext.Set<T>().Where(whereLambda).OrderBy(orderLambda1).ThenBy(orderLambda2).AsNoTracking().ToListAsync();
                    }
                    else
                    {
                        return await _ghDbContext.Set<T>().Where(whereLambda).OrderBy(orderLambda1).ThenByDescending(orderLambda2).AsNoTracking().ToListAsync();
                    }
                }
                else
                {
                    if (isAsc2)
                    {
                        return await _ghDbContext.Set<T>().Where(whereLambda).OrderByDescending(orderLambda1).ThenBy(orderLambda2).AsNoTracking().ToListAsync();
                    }
                    else
                    {
                        return await _ghDbContext.Set<T>().Where(whereLambda).OrderByDescending(orderLambda1).ThenByDescending(orderLambda2).AsNoTracking().ToListAsync();
                    }
                }
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderByLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPagedList<T, TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true) where T : class
        {
            try
            {

                // 分页 一定注意： Skip 之前一定要 OrderBy
                if (isAsc)
                {
                    return await _ghDbContext.Set<T>().Where(whereLambda).OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
                }
                else
                {
                    return await _ghDbContext.Set<T>().Where(whereLambda).OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
                }
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }
        }
        /// <summary>
        /// 分页查询 支持双字段排序
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderByLambda1"></param>
        /// <param name="orderByLambda2"></param>
        /// <param name="isAsc1"></param>
        /// <param name="isAsc2"></param>
        /// <returns></returns>
        public async Task<List<T>> GetPagedList<T, TKey1, TKey2>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey1>> orderByLambda1, Expression<Func<T, TKey2>> orderByLambda2, bool isAsc1 = true, bool isAsc2 = true) where T : class
        {
            try
            {
                if (isAsc1)
                {
                    if (isAsc2)
                    {
                        return await _ghDbContext.Set<T>().OrderBy(orderByLambda1).ThenBy(orderByLambda2).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
                    }
                    else
                    {
                        return await _ghDbContext.Set<T>().OrderBy(orderByLambda1).ThenByDescending(orderByLambda2).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
                    }
                }
                else
                {
                    if (isAsc2)
                    {
                        return await _ghDbContext.Set<T>().OrderByDescending(orderByLambda1).ThenBy(orderByLambda2).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
                    }
                    else
                    {
                        return await _ghDbContext.Set<T>().OrderByDescending(orderByLambda1).ThenByDescending(orderByLambda2).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
                    }
                }
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }
        }
        /// <summary>
        /// 分页查询 带输出
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderByLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public List<T> GetPagedList<T, TKey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true) where T : class
        {
            try
            {
                rowCount = _ghDbContext.Set<T>().Where(whereLambda).Count();
                if (isAsc)
                {
                    return _ghDbContext.Set<T>().OrderBy(orderByLambda).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
                }
                else
                {
                    return _ghDbContext.Set<T>().OrderByDescending(orderByLambda).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
                }
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }
        }
        /// <summary>
        /// 分页查询 带输出 并支持双字段排序
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderByLambda1"></param>
        /// <param name="orderByLambda2"></param>
        /// <param name="isAsc1"></param>
        /// <param name="isAsc2"></param>
        /// <returns></returns>
        public List<T> GetPagedList<T, TKey1, TKey2>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey1>> orderByLambda1, Expression<Func<T, TKey2>> orderByLambda2, bool isAsc1 = true, bool isAsc2 = true) where T : class
        {
            try
            {
                rowCount = _ghDbContext.Set<T>().Where(whereLambda).Count();
                if (isAsc1)
                {
                    if (isAsc2)
                    {
                        return _ghDbContext.Set<T>().OrderBy(orderByLambda1).ThenBy(orderByLambda2).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
                    }
                    else
                    {
                        return _ghDbContext.Set<T>().OrderBy(orderByLambda1).ThenByDescending(orderByLambda2).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
                    }
                }
                else
                {
                    if (isAsc2)
                    {
                        return _ghDbContext.Set<T>().OrderByDescending(orderByLambda1).ThenBy(orderByLambda2).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
                    }
                    else
                    {
                        return _ghDbContext.Set<T>().OrderByDescending(orderByLambda1).ThenByDescending(orderByLambda2).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
                    }
                }
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }
        }
        #endregion


        #region 数据添加操作
        /// <summary>
        /// 新增实体，返回受影响的行数
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns>返回受影响的行数</returns>
        public int Add<T>(T P_Entity) where T : class
        {
            try
            {
                _ghDbContext.Set<T>().Add(P_Entity).State = EntityState.Added;
                //保存成功后，会将自增的id设置给P_Entity的主键属性，并返回受影响的行数。
                return _ghDbContext.SaveChanges();
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }
        public int AddRange<T>(List<T> P_EntityList) where T : class
        {
            try
            {
                _ghDbContext.Set<T>().AddRange(P_EntityList);
                return _ghDbContext.SaveChanges();
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }
        /// <summary>
        /// 新增实体，返回对应的实体对象
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public T AddReturnEntity<T>(T P_Entity) where T : class
        {
            try
            {
                _ghDbContext.Set<T>().Add(P_Entity);
                if (_ghDbContext.SaveChanges() > 0)
                {
                    //保存成功返回保存后的实体，以便用于获取自增的ID值。
                    return P_Entity;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
                //throw e;
            }
        }

        #endregion



        #region 数据修改（更新）操作
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <returns></returns>
        public int Modify<T>(T P_Entity) where T : class
        {
            try
            {
                EntityEntry entry = _ghDbContext.Entry<T>(P_Entity);
                if (entry.State != EntityState.Modified)
                {
                    entry.State = EntityState.Modified;
                }
                return _ghDbContext.SaveChanges();
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }
        /// <summary>
        /// 修改实体，可修改指定属性
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <param name="propertyNames">要修改的字段名称</param>
        /// <returns></returns>
        public int Modify<T>(T P_Entity, params string[] propertyNames) where T : class
        {
            try
            {
                //将对象添加到EF中
                EntityEntry entry = _ghDbContext.Entry<T>(P_Entity);
                //先设置对象的包装状态为 Unchanged
                entry.State = EntityState.Unchanged;
                //循环被修改的属性名数组
                foreach (string propertyName in propertyNames)
                {
                    //将每个被修改的属性的状态设置为已修改状态；这样在后面生成的修改语句时，就只为标识为已修改的属性更新
                    entry.Property(propertyName).IsModified = true;
                }
                return _ghDbContext.SaveChanges();
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="P_Entity"></param>
        /// <param name="whereLambda"></param>
        /// <param name="modifiedPropertyNames"></param>
        /// <returns></returns>
        public int ModifyBy<T>(T P_Entity, Expression<Func<T, bool>> whereLambda, params string[] modifiedPropertyNames) where T : class
        {
            try
            {
                //查询要修改的数据
                List<T> listModifing = _ghDbContext.Set<T>().Where(whereLambda).ToList();
                //获取实体类类型对象
                Type t = typeof(T);
                //获取实体类所有的公共属性
                List<PropertyInfo> propertyInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
                //创建实体属性字典集合
                Dictionary<string, PropertyInfo> dicPropertys = new Dictionary<string, PropertyInfo>();
                //将实体属性中要修改的属性名 添加到字典集合中  键：属性名  值：属性对象
                propertyInfos.ForEach(p =>
                {
                    if (modifiedPropertyNames.Contains(p.Name))
                    {
                        dicPropertys.Add(p.Name, p);
                    }
                });
                //循环要修改的属性名
                foreach (string propertyName in modifiedPropertyNames)
                {
                    //判断要修改的属性名是否在实体类的属性集合中存在
                    if (dicPropertys.ContainsKey(propertyName))
                    {
                        //如果存在，则取出要修改的属性对象
                        PropertyInfo proInfo = dicPropertys[propertyName];
                        //取出要修改的值
                        object newValue = proInfo.GetValue(P_Entity, null);
                        //批量设置要修改对象的属性
                        foreach (T item in listModifing)
                        {
                            //为要修改的对象的要修改的属性设置新的值
                            proInfo.SetValue(item, newValue, null);
                        }
                    }
                }
                //一次性生成sql语句 到数据库执行
                return _ghDbContext.SaveChanges();
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }
        #endregion



        #region 数据删除操作
        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="P_Entity">必须包含要删除id的对象</param>
        /// <returns></returns>
        public int Del<T>(T P_Entity) where T : class
        {
            try
            {
                _ghDbContext.Set<T>().Attach(P_Entity);
                _ghDbContext.Set<T>().Remove(P_Entity);
                return _ghDbContext.SaveChanges();
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="delWhere"></param>
        /// <returns>返回受影响的行数</returns>
        public int DelBy<T>(Expression<Func<T, bool>> delWhere) where T : class
        {
            try
            {
                //查询要删除的数据
                List<T> listDeleting = _ghDbContext.Set<T>().Where(delWhere).ToList();
                //将要删除的数据 用删除方法添加到 EF 容器中
                listDeleting.ForEach(u =>
                {
                    _ghDbContext.Set<T>().Attach(u);  //先附加到EF 容器
                    _ghDbContext.Set<T>().Remove(u); //标识为删除状态
                });
                //一次性生成sql语句 到数据库执行删除
                return _ghDbContext.SaveChanges();
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }
        public int Del<T>(List<T> listDeleting) where T : class
        {
            try
            {
                //查询要删除的数据
                //将要删除的数据 用删除方法添加到 EF 容器中
                listDeleting.ForEach(u =>
                {
                    _ghDbContext.Set<T>().Attach(u);  //先附加到EF 容器
                    _ghDbContext.Set<T>().Remove(u); //标识为删除状态
                });
                //一次性生成sql语句 到数据库执行删除
                return _ghDbContext.SaveChanges();
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }
        #endregion


        #region 统计查询
        /// <summary>
        /// 统计指定类满足条件的记录条数。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public int Count<T>(Expression<Func<T, bool>> whereLambda) where T : class
        {
            try
            {
                return _ghDbContext.Set<T>().AsNoTracking().Where(whereLambda).Count();
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }
        /// <summary>
        /// 按指定的条件对指定的字段进行求和
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="whereLambda">指定的查询条件</param>
        /// <param name="sumField">指定的字段</param>
        /// <returns>Sum</returns>
        public int Sum<T>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, int>> sumField) where T : class
        {
            try
            {
                return _ghDbContext.Set<T>().AsNoTracking().Where(whereLambda).Sum(sumField);
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }
        /// <summary>
        /// 按指定的条件对指定的字段进行求和
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="whereLambda">指定的查询条件</param>
        /// <param name="sumField">指定的字段</param>
        /// <returns>float</returns>
        public float Sum<T>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, float>> sumField) where T : class
        {
            try
            {
                return _ghDbContext.Set<T>().AsNoTracking().Where(whereLambda).Sum(sumField);
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }
        /// <summary>
        /// 按指定的条件对指定的字段进行求和
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="whereLambda">指定的查询条件</param>
        /// <param name="sumField">指定的字段</param>
        /// <returns>double</returns>
        public double Sum<T>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, double>> sumField) where T : class
        {
            try
            {
                return _ghDbContext.Set<T>().AsNoTracking().Where(whereLambda).Sum(sumField);
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }
        /// <summary>
        /// 按指定的条件对指定的字段进行求和
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="whereLambda">指定的查询条件</param>
        /// <param name="sumField">指定的字段</param>
        /// <returns>decimal</returns>
        public decimal Sum<T>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, decimal>> sumField) where T : class
        {
            try
            {
                return _ghDbContext.Set<T>().AsNoTracking().Where(whereLambda).Sum(sumField);
            }
            catch (Exception)
            {
                return -1;
                //throw e;
            }
        }
        #endregion
    }
}
