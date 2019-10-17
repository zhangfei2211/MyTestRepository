using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public static class EFExtension
    {
        public static void Detach<T>(this DbContext db, T obj) where T : class
        {
            ObjectContext oc = ((IObjectContextAdapter)db).ObjectContext;
            oc.Detach(obj);
        }

        public static IEnumerable<string> GetEntityKeys<T>(this DbContext db) where T : class
        {
            ObjectContext oc = ((IObjectContextAdapter)db).ObjectContext;
            var keys = oc.CreateObjectSet<T>().EntitySet.ElementType.KeyProperties.Select(x => x.Name);
            return keys;
        }

        private static Expression<Func<T, bool>> GetFindExp<T>(T obj, IEnumerable<string> keys) where T : class
        {
            var p = Expression.Parameter(typeof(T), "x");

            var keyexps = keys.Select(x =>
            {
                var member = Expression.PropertyOrField(p, x);
                var objV = typeof(T).GetProperty(x).GetValue(obj);
                var eq = Expression.Equal(member, Expression.Constant(objV));
                return eq;
            }).ToList();

            if (keys.Count() == 1)
            {
                return Expression.Lambda<Func<T, bool>>(keyexps[0], new[] { p });
            }

            var and = Expression.AndAlso(keyexps[0], keyexps[1]);
            for (var i = 2; i < keyexps.Count; i++)
            {
                and = Expression.AndAlso(and, keyexps[i]);
            }
            return Expression.Lambda<Func<T, bool>>(and, new[] { p });
        }

        public static T FindLocal<T>(this DbContext db, T obj) where T : class
        {
            var keys = db.GetEntityKeys<T>();
            var func = GetFindExp<T>(obj, keys).Compile();
            return db.Set<T>().Local.FirstOrDefault(func);
        }

        /// <summary>
        /// 若对象在DbContext已存在，分离他，避免Attach出错
        /// </summary>
        public static void DetachOther<T>(this DbContext db, T obj) where T : class
        {
            var local = db.FindLocal(obj);
            if (local != null)
            {
                db.Detach(local);
            }
        }
    }
}
