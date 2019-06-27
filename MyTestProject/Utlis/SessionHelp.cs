using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utlis
{
    public class SessionHelp
    {
        /// <summary>
        /// 添加Session,有效期为默认
        /// </summary>
        /// <param name="sessionName">session名称</param>
        /// <param name="objValue">session值</param>
        public static void Add(string sessionName, object objValue)
        {
            HttpContext.Current.Session[sessionName] = objValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionName"></param>
        /// <param name="objValue"></param>
        /// <param name="timeOut"></param>
        public static void Add(string sessionName, object objValue, int timeOut)
        {
            HttpContext.Current.Session[sessionName] = objValue;
            HttpContext.Current.Session.Timeout = timeOut;
        }

        /// <summary>
        /// 获取某个session的值
        /// </summary>
        /// <param name="sessionName"></param>
        /// <returns></returns>
        public static object Get(string sessionName)
        {
            return HttpContext.Current.Session[sessionName];
        }

        public static void Remove(string sessionName)
        {
            HttpContext.Current.Session.Remove(sessionName);
        }
    }
}
