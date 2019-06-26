using Entities;
using Entities.Model;
using IDal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public ZyfTestDbEntities db = DbContextFactory.GetCurrentContext();

        public virtual async Task<bool> Add(T entity)
        {
            db.Entry<T>(entity).State = EntityState.Added;
            return await db.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> Update(T entity)
        {
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Modified;
            return await db.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> Delete(T entity)
        {
            PropertyInfo proInfo = entity.GetType().GetProperty("IsDelete");
            if (proInfo == null)
            {
                return false;
            }
            else
            {
                proInfo.SetValue(entity, true, null);
            }
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Modified;
            return await db.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> DeletePhysicalData(T entity)
        {
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Deleted;
            return await db.SaveChangesAsync() > 0;
        }

        public virtual async Task<int> Count(Expression<Func<T, bool>> whereLambda)
        {
            return await db.Set<T>().AsNoTracking().CountAsync(whereLambda);
        }

        public virtual async Task<bool> Exist(Expression<Func<T, bool>> whereLambda)
        {
            return await db.Set<T>().AsNoTracking().AnyAsync(whereLambda);
        }

        public virtual async Task<T> Find(Expression<Func<T, bool>> whereLambda)
        {
            return await db.Set<T>().AsNoTracking().FirstOrDefaultAsync(whereLambda);
        }

        public virtual async Task<IQueryable<T>> FindList(Expression<Func<T, bool>> whereLambda)
        {
            return await Task.Run(() =>
                {
                    return db.Set<T>().AsNoTracking().Where(whereLambda);
                });
        }

        public virtual async Task<IQueryable<T>> FindList(Expression<Func<T, bool>> whereLambda, OrderField[] orders)
        {
            return await Task.Run(() =>
            {
                var query = db.Set<T>().AsNoTracking().Where(whereLambda);

                query = GetResultByOrderFields(orders, query);

                return query;
            });
        }

        public virtual async Task<PagedResult<T>> FindPageList(PageModel pageModel, Expression<Func<T, bool>> whereLambda)
        {
            return await Task.Run(() =>
            {
                var query = db.Set<T>().AsNoTracking().Where(whereLambda);

                query = GetResultByOrderFields(pageModel.OrderFileds, query);

                var result=query.Skip((pageModel.PageIndex - 1) * pageModel.PageSize).Take(pageModel.PageSize).ToList();

                PagedResult<T> pageResult = new PagedResult<T>()
                {
                    TotalCounts = query.Count(),
                    TotalPages = query.Count() / pageModel.PageSize,
                    PageSize = pageModel.PageSize,
                    PageIndex = pageModel.PageIndex,
                    Data = result
                };

                return pageResult;
            });
        }

        private IQueryable<T> GetResultByOrderFields(OrderField[] orders,IQueryable<T> query)
        {
            //创建表达式变量参数
            var parameter = Expression.Parameter(typeof(T), "o");

            if (orders != null && orders.Length > 0)
            {
                for (int i = 0; i < orders.Length; i++)
                {
                    //获取属性
                    var property = typeof(T).GetProperty(orders[i].PropertyName);
                    //创建一个访问属性的表达式
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);

                    var orderByExp = Expression.Lambda(propertyAccess, parameter);

                    var OrderName = string.Empty;
                    if (i > 0)
                    {
                        OrderName = orders[i].IsDesc ? "ThenByDescending" : "ThenBy";
                    }
                    else
                    {
                        OrderName = orders[i].IsDesc ? "OrderByDescending" : "OrderBy";
                    }

                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), OrderName,
                        new Type[] { typeof(T), property.PropertyType },
                        query.Expression, Expression.Quote(orderByExp)
                        );

                    query = query.Provider.CreateQuery<T>(resultExp);
                }
            }

            return query;
        }
    }
}
