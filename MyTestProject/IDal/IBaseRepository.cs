using Entities.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utlis.Autofac;

namespace IDal
{
    public interface IBaseRepository<T>:IAutofac
    {
        /// <summary>
        /// 单个保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>是否成功</returns>
        bool Add(T entity, bool isSaveChange = true);

        /// <summary>
        /// 单个保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>是否成功</returns>
        Task<bool> AddAsync(T entity, bool isSaveChange = true);

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="list"></param>
        /// <returns>是否成功</returns>
        bool Adds(List<T> list, bool isSaveChange = true);

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="list"></param>
        /// <returns>是否成功</returns>
        Task<bool> AddsAsync(List<T> list, bool isSaveChange = true);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        bool Update(T entity, bool isSaveChange = true);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> UpdateAsync(T entity, bool isSaveChange = true);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list">数据实体</param>
        /// <returns>是否成功</returns>
        bool Updates(List<T> list, bool isSaveChange = true);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> UpdatesAsync(List<T> list, bool isSaveChange = true);

        /// <summary>
        /// 新增或修改，在不明确数据会是新增还是修改时使用(暂未实现)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddOrUpdate(T entity, bool isSaveChange = true);

        /// <summary>
        /// 新增或修改，在不明确数据会是新增还是修改时使用(暂未实现)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdateAsync(T entity, bool isSaveChange = true);

        /// <summary>
        /// 逻辑删除,需要实体有IsDelete字段
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        bool Delete(T entity, bool isSaveChange = true);

        /// <summary>
        /// 逻辑删除,需要实体有IsDelete字段
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteAsync(T entity, bool isSaveChange = true);

        /// <summary>
        /// 逻辑删除，需要实体有Id,IsDelete字段
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isSaveChange"></param>
        /// <returns></returns>
        bool DeleteById(Guid id, bool isSaveChange = true);

        /// <summary>
        /// 逻辑删除，需要实体有Id,IsDelete字段
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isSaveChange"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(Guid id, bool isSaveChange = true);

        /// <summary>
        /// 批量逻辑删除,需要实体有IsDelete字段
        /// </summary>list</param>
        /// <returns>是否成功</returns>
        bool Deletes(List<T> list, bool isSaveChange = true);

        /// <summary>
        /// 批量逻辑删除,需要实体有IsDelete字段
        /// </summary>list</param>
        /// <returns>是否成功</returns>
        Task<bool> DeletesAsync(List<T> list, bool isSaveChange = true);

        /// <summary>
        /// 物理删除，删除具体数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool DeletePhysicalData(T entity, bool isSaveChange = true);

        /// <summary>
        /// 物理删除，删除具体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeletePhysicalDataAsync(T entity, bool isSaveChange = true);

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isSaveChange"></param>
        /// <returns></returns>
        bool DeletePhysicalDataById(Guid id, bool isSaveChange = true);

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isSaveChange"></param>
        /// <returns></returns>
        Task<bool> DeletePhysicalDataByIdAsync(Guid id, bool isSaveChange = true);

        /// <summary>
        /// 批量物理删除，删除具体数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool DeletePhysicalDatas(List<T> list, bool isSaveChange = true);

        /// <summary>
        /// 批量物理删除，删除具体数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> DeletePhysicalDatasAsync(List<T> list, bool isSaveChange = true);

        /// <summary>
        /// 获取数据条数
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns>数量</returns>
        int Count(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 获取数据条数
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns>数量</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="whereLambda">条件表达式</param>
        /// <returns>是否存在</returns>
        bool Exist(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="whereLambda">条件表达式</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistAsync(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查询数据单条
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        T Find(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查询数据单条
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        Task<T> FindAsync(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查找所有数据
        /// </summary>
        /// <returns></returns>
        IQueryable<T> FindAll();

        /// <summary>
        /// 查找所有数据
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<T>> FindAllAsync();

        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体列表</returns>
        IQueryable<T> FindList(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体列表</returns>
        Task<IQueryable<T>> FindListAsync(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="orders">排序集合</param>
        /// <returns></returns>
        IQueryable<T> FindList(Expression<Func<T, bool>> whereLambda, List<OrderCondition> orders);

        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="orders">排序集合</param>
        /// <returns></returns>
        Task<IQueryable<T>> FindListAsync(Expression<Func<T, bool>> whereLambda, List<OrderCondition> orders);

        /// <summary>
        /// 查询分页数据列表
        /// </summary>
        /// <param name="pageModel"></param>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        PageResult<T> FindPageList(PageSearchModel pageModel, Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查询分页数据列表
        /// </summary>
        /// <param name="pageModel"></param>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<PageResult<T>> FindPageListAsync(PageSearchModel pageModel, Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 提交所有数据库修改，用于多个增删改事务
        /// </summary>
        /// <returns></returns>
        bool SaveChanges();

        /// <summary>
        /// 提交所有数据库修改，用于多个增删改事务
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangesAsync();
    }
}
