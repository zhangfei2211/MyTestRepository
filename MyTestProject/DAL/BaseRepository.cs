﻿using Entities;
using Entities.Enum;
using Entities.Model.Common;
using IDal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utlis.Extension;
using Utlis;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace Dal
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class,new()
    {
        public ZyfTestDbEntities db = DbContextFactory.GetCurrentContext();

        public virtual bool Add(T entity, bool isSaveChange = true)
        {
            try
            {
                SetCreate(entity);
                db.Entry(entity).State = EntityState.Added;

                return SaveChanges(isSaveChange);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> AddAsync(T entity, bool isSaveChange = true)
        {
            try
            {
                SetCreate(entity);

                db.Entry<T>(entity).State = EntityState.Added;

                return await SaveChangesAsync(isSaveChange);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool Adds(List<T> list, bool isSaveChange = true)
        {
            for (int i = 0; i < list.Count; i++)
            {
                SetCreate(list[i]);
                db.Entry<T>(list[i]).State = EntityState.Added;
            }
            return SaveChanges(isSaveChange);
        }

        public virtual async Task<bool> AddsAsync(List<T> list, bool isSaveChange = true)
        {
            for (int i = 0; i < list.Count; i++)
            {
                SetCreate(list[i]);
                db.Entry<T>(list[i]).State = EntityState.Added;
            }
            return await SaveChangesAsync(isSaveChange);
        }

        public virtual bool Update(T entity, bool isSaveChange = true)
        {
            SetUpdate(entity);
            db.DetachOther(entity);
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Modified;
            return SaveChanges(isSaveChange);
        }

        public virtual async Task<bool> UpdateAsync(T entity, bool isSaveChange = true)
        {
            SetUpdate(entity);
            db.DetachOther(entity);
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Modified;
            return await SaveChangesAsync(isSaveChange);
        }

        public virtual bool Updates(List<T> list, bool isSaveChange = true)
        {
            for (int i = 0; i < list.Count; i++)
            {
                SetUpdate(list[i]);
                db.Set<T>().Attach(list[i]);
                db.Entry<T>(list[i]).State = EntityState.Modified;
            }
            return SaveChanges(isSaveChange);
        }

        public virtual async Task<bool> UpdatesAsync(List<T> list, bool isSaveChange = true)
        {
            for (int i = 0; i < list.Count; i++)
            {
                SetUpdate(list[i]);
                db.Set<T>().Attach(list[i]);
                db.Entry<T>(list[i]).State = EntityState.Modified;
            }
            return await SaveChangesAsync(isSaveChange);
        }

        public virtual bool AddOrUpdate(T entity, bool isSaveChange = true)
        {
            return true;
        }

        public virtual async Task<bool> AddOrUpdateAsync(T entity, bool isSaveChange = true)
        {
            return true;
        }

        public virtual bool Delete(T entity, bool isSaveChange = true)
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
            SetUpdate(entity);
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Modified;
            return SaveChanges(isSaveChange);
        }

        public virtual async Task<bool> DeleteAsync(T entity, bool isSaveChange = true)
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
            SetUpdate(entity);
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Modified;
            return await SaveChangesAsync(isSaveChange);
        }

        public virtual bool DeleteById(Guid id, bool isSaveChange = true)
        {
            T t = new T();
            PropertyInfo pId = t.GetType().GetProperty("Id");
            if (pId == null)
            {
                return false;
            }
            else
            {
                pId.SetValue(t, id, null);
            }

            PropertyInfo pIsDelete = t.GetType().GetProperty("IsDelete");
            if (pIsDelete == null)
            {
                return false;
            }
            else
            {
                pIsDelete.SetValue(t, true, null);
            }

            db.Entry<T>(t).State = EntityState.Modified;
            return SaveChanges(isSaveChange);
        }

        public virtual async Task<bool> DeleteByIdAsync(Guid id, bool isSaveChange = true)
        {
            T t = new T();
            PropertyInfo pId = t.GetType().GetProperty("Id");
            if (pId == null)
            {
                return false;
            }
            else
            {
                pId.SetValue(t, id, null);
            }

            PropertyInfo pIsDelete = t.GetType().GetProperty("IsDelete");
            if (pIsDelete == null)
            {
                return false;
            }
            else
            {
                pIsDelete.SetValue(t, true, null);
            }

            db.Entry<T>(t).State = EntityState.Modified;
            return await SaveChangesAsync(isSaveChange);
        }

        public virtual bool Deletes(List<T> list, bool isSaveChange = true)
        {
            for (int i = 0; i < list.Count; i++)
            {
                PropertyInfo proInfo = list[i].GetType().GetProperty("IsDelete");
                if (proInfo == null)
                {
                    return false;
                }
                else
                {
                    proInfo.SetValue(list[i], true, null);
                }
                db.Set<T>().Attach(list[i]);
                db.Entry<T>(list[i]).State = EntityState.Modified;
            }
            return SaveChanges(isSaveChange);
        }

        public virtual async Task<bool> DeletesAsync(List<T> list, bool isSaveChange = true)
        {
            for (int i = 0; i < list.Count; i++)
            {
                PropertyInfo proInfo = list[i].GetType().GetProperty("IsDelete");
                if (proInfo == null)
                {
                    return false;
                }
                else
                {
                    proInfo.SetValue(list[i], true, null);
                }
                db.Set<T>().Attach(list[i]);
                db.Entry<T>(list[i]).State = EntityState.Modified;
            }
            return await SaveChangesAsync(isSaveChange);
        }

        public virtual bool DeletePhysicalData(T entity, bool isSaveChange = true)
        {
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Deleted;
            return SaveChanges(isSaveChange);
        }

        public virtual async Task<bool> DeletePhysicalDataAsync(T entity, bool isSaveChange = true)
        {
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Deleted;
            return await SaveChangesAsync(isSaveChange);
        }

        public virtual bool DeletePhysicalDataById(Guid id, bool isSaveChange = true)
        {
            T t = new T();
            PropertyInfo proInfo = t.GetType().GetProperty("Id");
            if (proInfo == null)
            {
                return false;
            }
            else
            {
                proInfo.SetValue(t, id, null);
            }

            db.Entry<T>(t).State = EntityState.Deleted;
            return SaveChanges(isSaveChange);
        }

        public virtual async Task<bool> DeletePhysicalDataByIdAsync(Guid id, bool isSaveChange = true)
        {
            T t = new T();
            PropertyInfo proInfo = t.GetType().GetProperty("Id");
            if (proInfo == null)
            {
                return false;
            }
            else
            {
                proInfo.SetValue(t, id, null);
            }

            db.Entry<T>(t).State = EntityState.Deleted;
            return await SaveChangesAsync(isSaveChange);
        }

        public virtual bool DeletePhysicalDatas(List<T> list, bool isSaveChange = true)
        {
            foreach (T ele in list)
            {
                db.Set<T>().Attach(ele);
                db.Entry<T>(ele).State = EntityState.Deleted;
            }
            return SaveChanges(isSaveChange);
        }

        public virtual async Task<bool> DeletePhysicalDatasAsync(List<T> list, bool isSaveChange = true)
        {
            foreach (T ele in list)
            {
                db.Set<T>().Attach(ele);
                db.Entry<T>(ele).State = EntityState.Deleted;
            }
            return await SaveChangesAsync(isSaveChange);
        }

        public virtual int Count(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().AsNoTracking().Count(whereLambda);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await db.Set<T>().AsNoTracking().CountAsync(whereLambda);
        }

        public virtual bool Exist(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().AsNoTracking().Any(whereLambda);
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await db.Set<T>().AsNoTracking().AnyAsync(whereLambda);
        }

        public virtual  T Find(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().AsNoTracking().FirstOrDefault(whereLambda);
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await db.Set<T>().AsNoTracking().FirstOrDefaultAsync(whereLambda);
        }

        public virtual IQueryable<T> FindAll()
        {
            return db.Set<T>().AsNoTracking();
        }

        public virtual async Task<IQueryable<T>> FindAllAsync()
        {
            return await Task.Run(() =>
            {
                return db.Set<T>().AsNoTracking();
            }); 
        }

        public virtual IQueryable<T> FindList(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().AsNoTracking().Where(whereLambda);
        }

        public virtual async Task<IQueryable<T>> FindListAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await Task.Run(() =>
                {
                    return db.Set<T>().AsNoTracking().Where(whereLambda);
                });
        }

        public virtual IQueryable<T> FindList(Expression<Func<T, bool>> whereLambda, List<OrderCondition> orders)
        {
            var query = db.Set<T>().AsNoTracking().Where(whereLambda);

            query = GetResultByOrderConditions(orders, query);

            return query;
        }

        public virtual async Task<IQueryable<T>> FindListAsync(Expression<Func<T, bool>> whereLambda, List<OrderCondition> orders)
        {
            return await Task.Run(() =>
            {
                var query = db.Set<T>().AsNoTracking().Where(whereLambda);

                query = GetResultByOrderConditions(orders, query);

                return query;
            });
        }

        public virtual PageResult<T> FindPageList(PageSearchModel pageModel, Expression<Func<T, bool>> whereLambda)
        {
            var query = db.Set<T>().AsNoTracking().Where(whereLambda);
            query = GetResultByOrderConditions(pageModel.OrderConditions, query);

            var result = query.Skip((pageModel.PageIndex - 1) * pageModel.PageSize).Take(pageModel.PageSize).ToList();

            PageResult<T> pageResult = new PageResult<T>()
            {
                TotalCounts = query.Count(),
                TotalPages = query.Count() / pageModel.PageSize,
                PageSize = pageModel.PageSize,
                PageIndex = pageModel.PageIndex,
                Data = result
            };

            return pageResult;
        }

        public virtual async Task<PageResult<T>> FindPageListAsync(PageSearchModel pageModel, Expression<Func<T, bool>> whereLambda)
        {
            return await Task.Run(() =>
            {
                var query = db.Set<T>().AsNoTracking().Where(whereLambda);

                query = GetResultByOrderConditions(pageModel.OrderConditions, query);

                var result=query.Skip((pageModel.PageIndex - 1) * pageModel.PageSize).Take(pageModel.PageSize).ToList();

                PageResult<T> pageResult = new PageResult<T>()
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

        public bool SaveChanges()
        {
            return db.SaveChanges() > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync()>0;
        }

        private bool SaveChanges(bool isSaveChange)
        {
            if (isSaveChange)
            {
                return SaveChanges();
            }
            else
            {
                return false;
            }
        }

        private async Task<bool> SaveChangesAsync(bool isSaveChange)
        {
            if (isSaveChange)
            {
                return await SaveChangesAsync();
            }
            else
            {
                return false;
            }
        }

        private IQueryable<T> GetResultByOrderConditions(List<OrderCondition> orders, IQueryable<T> query)
        {
            //创建表达式变量参数
            var parameter = Expression.Parameter(typeof(T), "o");
            if (orders != null && orders.Count > 0)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    //获取属性
                    var property = typeof(T).GetProperty(orders[i].OrderbyField);
                    //创建一个访问属性的表达式
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);

                    var orderByExp = Expression.Lambda(propertyAccess, parameter);

                    var OrderName = string.Empty;
                    if (i > 0)
                    {
                        OrderName = orders[i].IsAsc ? "ThenBy" : "ThenByDescending";
                    }
                    else
                    {
                        OrderName = orders[i].IsAsc ? "OrderBy" : "OrderByDescending";
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

        private void SetCreate(T entity)
        {
            if (!CurrentUser.User.IsNull())
            {
                Type type = entity.GetType();

                var createByPs = type.GetProperty("CreateBy");
                var createDatePs = type.GetProperty("CreateDate");
                if (!createByPs.IsNull() && createByPs.GetValue(entity).IsNull())
                {
                    createByPs.SetValue(entity, CurrentUser.User.Id);
                }
                if (!createDatePs.IsNull() && createDatePs.GetValue(entity).IsNull())
                {
                    createDatePs.SetValue(entity, DateTime.Now);
                }
            }
        }

        private void SetUpdate(T entity)
        {
            if (!CurrentUser.User.IsNull())
            {
                Type type = entity.GetType();

                var updateByPs = type.GetProperty("UpdateBy");
                var updateDatePs = type.GetProperty("UpdateDate");
                if (!updateByPs.IsNull() && updateByPs.GetValue(entity).IsNull())
                {
                    updateByPs.SetValue(entity, CurrentUser.User.Id);
                }
                if (!updateDatePs.IsNull() && updateDatePs.GetValue(entity).IsNull())
                {
                    updateDatePs.SetValue(entity, DateTime.Now);
                }
            }
        }
    }
}
