using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utlis.Cookie
{
    public class CookieHelp
    {
        private static HttpContext currentContext = HttpContext.Current;

        /// <summary>
        /// 设置单个cookie
        /// </summary>
        /// <param name="cookieName">cookie名</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="cookieTimeOut">失效时间，默认7</param>
        /// <param name="timeOutType">失效时间类型，默认天</param>
        /// <returns></returns>
        public static bool SetCookie(string cookieName, string cookieValue, int cookieTimeOut = 7, CookieTimeOutType timeOutType = CookieTimeOutType.Day)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
                switch (timeOutType)
                {
                    case CookieTimeOutType.Second: cookie.Expires = DateTime.Now.AddSeconds(cookieTimeOut); break;
                    case CookieTimeOutType.Minute: cookie.Expires = DateTime.Now.AddSeconds(cookieTimeOut); break;
                    case CookieTimeOutType.Hour: cookie.Expires = DateTime.Now.AddHours(cookieTimeOut); break;
                    case CookieTimeOutType.Day: cookie.Expires = DateTime.Now.AddDays(cookieTimeOut); break;
                    case CookieTimeOutType.Month: cookie.Expires = DateTime.Now.AddMonths(cookieTimeOut); break;
                    case CookieTimeOutType.Year: cookie.Expires = DateTime.Now.AddYears(cookieTimeOut); break;
                    default: cookie.Expires = DateTime.Now.AddHours(cookieTimeOut); break;
                }

                currentContext.Response.Cookies.Add(cookie);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 设置一组cookie
        /// </summary>
        /// <param name="cookieName">cookie名</param>
        /// <param name="cookieValues">cookie键值对</param>
        /// <param name="cookieTimeOut">失效时间，默认7</param>
        /// <param name="timeOutType">失效时间类型，默认天</param>
        /// <returns></returns>
        public static bool SetCookies(string cookieName, Dictionary<string, string> cookieValues, int cookieTimeOut = 7, CookieTimeOutType timeOutType = CookieTimeOutType.Day)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                foreach (var val in cookieValues)
                {
                    cookie.Values.Add(val.Key, val.Value);
                }

                switch (timeOutType)
                {
                    case CookieTimeOutType.Second: cookie.Expires = DateTime.Now.AddSeconds(cookieTimeOut); break;
                    case CookieTimeOutType.Minute: cookie.Expires = DateTime.Now.AddSeconds(cookieTimeOut); break;
                    case CookieTimeOutType.Hour: cookie.Expires = DateTime.Now.AddHours(cookieTimeOut); break;
                    case CookieTimeOutType.Day: cookie.Expires = DateTime.Now.AddDays(cookieTimeOut); break;
                    case CookieTimeOutType.Month: cookie.Expires = DateTime.Now.AddMonths(cookieTimeOut); break;
                    case CookieTimeOutType.Year: cookie.Expires = DateTime.Now.AddYears(cookieTimeOut); break;
                    default: cookie.Expires = DateTime.Now.AddHours(cookieTimeOut); break;
                }

                currentContext.Response.Cookies.Add(cookie);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static HttpCookie GetCookie(string cookieName)
        {
            return currentContext.Request.Cookies[cookieName];
        }

        /// <summary>
        /// 获取cookie的值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookieValue(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                return currentContext.Request.Cookies[cookieName].Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取cookie所有值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static NameValueCollection GetCookieValues(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                return currentContext.Request.Cookies[cookieName].Values;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取cookie某个值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookieValues(string cookieName, string key)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                return currentContext.Request.Cookies[cookieName].Values[key];
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 清除指定cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static bool ClearCookie(string cookieName)
        {
            try
            {
                HttpContext.Current.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public enum CookieTimeOutType
        {
            Second = 1,
            Minute = 2,
            Hour = 3,
            Day = 4,
            Month = 5,
            Year = 6
        }
    }
}
