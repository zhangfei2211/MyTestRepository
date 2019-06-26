using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Base
{
    public class DbContextFactory
    {
        /// <summary>
        /// 保证每一个用户，只使用自己的dbcontext,且每个用户只创建一个dbcontext
        /// </summary>
        /// <returns></returns>
        public static ZyfTestDbEntities GetCurrentContext()
        {
            ZyfTestDbEntities _nContext = CallContext.GetData("ZyfTestDbEntities") as ZyfTestDbEntities;
            if (_nContext == null)
            {
                _nContext = new ZyfTestDbEntities();
                CallContext.SetData("ZyfTestDbEntities", _nContext);
            }
            return _nContext;
        }
    }
}
