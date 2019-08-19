using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utlis.Extension
{
    public static class BaseDataTypeExtension
    {
        /// <summary>
        ///     字符串是否为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static bool IsNotEmpty(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 字符串转Guid
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string s)
        {
            try
            {
                Guid gv = new Guid(s);
                return gv;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static bool IsNull(this Guid guid)
        {
            if (guid == Guid.Empty)
            {
                return true;
            }
            return false;
        }
    }
}
