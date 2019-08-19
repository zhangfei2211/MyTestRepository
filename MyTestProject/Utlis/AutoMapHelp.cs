using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utlis.Extension;

namespace Utlis
{
    /// <summary>
    /// 只用于未配置映射的情况，有损效率。
    /// </summary>
    public static class AutoMapHelp
    {
        private static IMapper CreateMap(Type sourceType, Type destinationType)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(sourceType, destinationType);
            });

            config.AssertConfigurationIsValid();//验证结构映射是否正确，发布时注释掉

            return config.CreateMapper();
        }

        private static IMapper CreateMap<TSource, TDestination>()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });

            config.AssertConfigurationIsValid();//验证结构映射是否正确，发布时注释掉

            return config.CreateMapper();
        }


        public static T MapTo<T>(this object source)
        {
            if (source.IsNull())
            {
                return default(T);
            }

            return CreateMap(source.GetType(), typeof(T)).Map<T>(source);
        }

        public static IList<TDest> MapTo<TSource, TDest>(this IEnumerable<TSource> source)
        {
            return CreateMap<TSource, TDest>().Map<IList<TDest>>(source);
        }

        public static TDest MapTo<TSource, TDest>(this TSource source, TDest dest)
        where TSource : class
        where TDest : class
        {
            if (source.IsNull())
            {
                return dest;
            }

            return CreateMap<TSource, TDest>().Map<TDest>(source);
        }

        /// <summary>
        /// DataReader映射
        /// </summary>
        public static IEnumerable<T> DataReaderMapTo<T>(this IDataReader reader)
        {
            return CreateMap<IDataReader, T>().Map<IDataReader, IEnumerable<T>>(reader);
        }
    }
}
