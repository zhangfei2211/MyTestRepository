using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utlis.Extension
{
    public static class IEnumerableExtension
    {
        /// <summary>
        /// 向集合中添加元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="value"></param>
        public static void Add<T>(this IEnumerable<T> collection, T value)
        {
            (collection as List<T>).Add(value);
        }
        /// <summary>
        /// 从集合中删除元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="value"></param>
        public static void Remove<T>(this IEnumerable<T> collection, T value)
        {
            (collection as List<T>).Remove(value);
        }
        /// <summary>
        /// 检索集合中是否包含某个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Contains<T>(this IEnumerable<T> collection, T value)
        {
            return (collection as List<T>).Contains(value);
        }
    }
}