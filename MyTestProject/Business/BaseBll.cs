using Entities;
using IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BaseBll
    {
        protected IBaseRepository<B_User> userDal;

        protected IBaseRepository<B_UserToken> userTokenDal;

        /// <summary>
        /// 专用于执行sql
        /// </summary>
        protected IBaseRepositoryForSql sqlDal;
    }
}
