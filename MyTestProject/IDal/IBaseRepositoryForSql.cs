using Entities.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utlis.Autofac;

namespace IDal
{
    public interface IBaseRepositoryForSql : IAutofac
    {
        IQueryable<T> FindListBySQL<T>(string sql, params object[] parameters);

        Task<IQueryable<T>> FindListBySQLAsync<T>(string sql, params object[] parameters);

        Task<PageResult<T>> FindPageListBySQLAsync<T>(string sql, PageSearchModel pageModel, params object[] parameters);

        int ExecuteSQL(string sql, params object[] parameters);

        Task<int> ExecuteSQLAsync(string sql, params object[] parameters);
    }
}
