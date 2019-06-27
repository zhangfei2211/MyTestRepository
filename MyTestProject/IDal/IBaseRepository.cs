using Entities.Model;
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
        Task<bool> Add(T entity, bool isSaveChange = true);

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="list"></param>
        /// <returns>是否成功</returns>
        Task<bool> Adds(List<T> list, bool isSaveChange = true);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> Update(T entity, bool isSaveChange = true);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> Updates(List<T> list, bool isSaveChange = true);

        /// <summary>
        /// 新增或修改，在不明确数据会是新增还是修改时使用(暂未实现)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdate(T entity, bool isSaveChange = true);

        /// <summary>
        /// 逻辑删除,需要实体有IsDelete字段
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> Delete(T entity, bool isSaveChange = true);

        /// <summary>
        /// 批量逻辑删除,需要实体有IsDelete字段
        /// </summary>list</param>
        /// <returns>是否成功</returns>
        Task<bool> Deletes(List<T> list, bool isSaveChange = true);

        /// <summary>
        /// 物理删除，删除具体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeletePhysicalData(T entity, bool isSaveChange = true);

        /// <summary>
        /// 批量物理删除，删除具体数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> DeletePhysicalDatas(List<T> list, bool isSaveChange = true);

        /// <summary>
        /// 获取数据条数
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns>数量</returns>
        Task<int> Count(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="whereLambda">条件表达式</param>
        /// <returns>是否存在</returns>
        Task<bool> Exist(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查询数据单条
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        Task<T> Find(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体列表</returns>
        Task<IQueryable<T>> FindList(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="orders">排序集合</param>
        /// <returns></returns>
        Task<IQueryable<T>> FindList(Expression<Func<T, bool>> whereLambda, OrderField[] orders);

        /// <summary>
        /// 查询分页数据列表
        /// </summary>
        /// <param name="pageModel"></param>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<PagedResult<T>> FindPageList(PageModel pageModel, Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 提交所有数据库修改，用于多个增删改事务
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangesAsync();
    }
}
