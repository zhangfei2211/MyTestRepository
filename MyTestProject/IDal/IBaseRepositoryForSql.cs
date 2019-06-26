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
        Task<IQueryable<T>> FindListBySQL<T>(string sql, params object[] parameters);

        Task<int> ExecuteSQL(string sql, params object[] parameters);
    }
}
