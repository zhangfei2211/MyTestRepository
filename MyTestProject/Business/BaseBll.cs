using Entities;
using Entities.Model.Common;
using IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BaseBll
    {
        protected IBaseRepository<B_User> userDal;

        protected IBaseRepository<B_UserToken> userTokenDal;

        protected IBaseRepository<B_Menu> menuDal;

        protected IBaseRepository<B_Role> roleDal;

        protected IBaseRepository<B_RoleMenu> roleMenuDal;

        protected IBaseRepository<B_UserRole> userRoleDal;

        /// <summary>
        /// 专用于执行sql
        /// </summary>
        protected IBaseRepositoryForSql sqlDal;

        /// <summary>
        /// 获取空lamda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected Expression<Func<T, bool>> GetExpression<T>()
        {
            Expression<Func<T, bool>> exp = d => 1 == 1;
            return exp;
        }
    }
}
