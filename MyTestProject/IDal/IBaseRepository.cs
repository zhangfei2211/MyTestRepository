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
        /// 保存
        /// </summary>
        /// <param name="entities"></param>
        /// <returns>是否成功</returns>
        Task<bool> Add(T entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> Update(T entities);

        /// <summary>
        /// 逻辑删除,需要实体有IsDelete字段
        /// </summary>
        /// <param name="entities">数据实体</param>
        /// <returns>是否成功</returns>
        Task<bool> Delete(T entities);

        /// <summary>
        /// 物理删除，删除具体数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> DeletePhysicalData(T entities);

        /// <summary>
        /// 取数据条数
        /// </summary>
        /// <param name="whereClause"></param>
        /// <returns>数量</returns>
        Task<int> Count(Expression<Func<T, bool>> whereClause);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereLambda">条件表达式</param>
        /// <returns>是否存在</returns>
        Task<bool> Exist(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查询数据
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
    }
}
