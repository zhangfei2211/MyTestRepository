using Entities;
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
