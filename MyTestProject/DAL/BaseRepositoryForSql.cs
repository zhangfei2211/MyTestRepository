using Entities;
using Entities.Model.Common;
using IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class BaseRepositoryForSql: IBaseRepositoryForSql
    {
        public ZyfTestDbEntities db_sql = DbContextFactory.GetCurrentContext();

        public virtual IQueryable<T> FindListBySQL<T>(string sql, params object[] parameters)
        {
            return db_sql.Database.SqlQuery<T>(sql, parameters).AsQueryable();
        }

        public virtual async Task<IQueryable<T>> FindListBySQLAsync<T>(string sql, params object[] parameters)
        {
            return await Task.Run(() => {
                return db_sql.Database.SqlQuery<T>(sql, parameters).AsQueryable();
            });
        }

        public virtual async Task<PageResult<T>> FindPageListBySQLAsync<T>(string sql, PageSearchModel pageModel, params object[] parameters)
        {
            return await Task.Run(async () => {
                //return db_sql.Database.SqlQuery<T>(sql, parameters).AsQueryable();

                //适用于需要汇总计算后再分页的情况
                var resultSql = String.Format(@"select top {1} * from 
                        (
                            select top({0}*{1}) ROW_NUMBER() OVER(Order by {3}) as RowNum,*from
                            ({2}) a
                        ) b
                        where RowNum Between ({0}-1)*{1}+1 and {0}*{1}
                        order by RowNum", pageModel.PageIndex, pageModel.PageSize, sql, pageModel.OrderBy);

                var countSql = "select count(*) from(" + sql + ") a";

                var totalCount = (await FindListBySQLAsync<int>(countSql)).FirstOrDefault();

                var result = (await FindListBySQLAsync<T>(resultSql)).ToList();

                PageResult<T> pageResult = new PageResult<T>()
                {
                    TotalCounts = totalCount,
                    TotalPages = totalCount / pageModel.PageSize,
                    PageSize = pageModel.PageSize,
                    PageIndex = pageModel.PageIndex,
                    Data = result
                };

                return pageResult;
            });
        }

        public virtual int ExecuteSQL(string sql, params object[] parameters)
        {
            return db_sql.Database.ExecuteSqlCommand(sql, parameters);
        }

        public virtual async Task<int> ExecuteSQLAsync(string sql, params object[] parameters)
        {
            return await db_sql.Database.ExecuteSqlCommandAsync(sql, parameters);
        }
    }
}
