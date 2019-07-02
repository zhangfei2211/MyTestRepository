using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utlis.Extension;

namespace Utlis
{
    public static class AutoMapHelp
    {
        private static MapperConfiguration config = new MapperConfiguration(d=>{});

        private static bool ConfigExist(Type srcType, Type destType)
        {
            return config.FindMapper(new TypePair(srcType, destType)).IsNull();
        }

        private static bool ConfigExist<TSrc, TDest>()
        {
            return config.FindMapper(new TypePair(typeof(TSrc), typeof(TDest))).IsNull();
        }

        public static T MapTo<T>(this object source)
        {
            if (source.IsNull())
            {
                return default(T);
            }
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(source.GetType(),typeof(T));
            });
            config.AssertConfigurationIsValid();//验证结构映射是否正确，发布时注释掉
            config.R
            var mapper = config.CreateMapper();

            return mapper.Map<T>(source);
        }

        public static IList<TDest> MapTo<TSource, TDest>(this IEnumerable<TSource> source)
        {
            //if (!ConfigExist<TSource, TDest>())
            //{
                Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDest>());
            //}

            return Mapper.Map<IList<TDest>>(source);
        }

        public static TDest MapTo<TSource, TDest>(this TSource source, TDest dest)
        where TSource : class
        where TDest : class
        {
            if (source.IsNull())
            {
                return dest;
            }

            //if (!ConfigExist<TSource, TDest>())
            //{
                Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDest>());
            //}

            return Mapper.Map<TDest>(source);
        }

        /// <summary>
        /// DataReader映射
        /// </summary>
        public static IEnumerable<T> DataReaderMapTo<T>(this IDataReader reader)
        {
            Mapper.Reset();
            //if (!ConfigExist<IDataReader, T>())
            //{
                Mapper.Initialize(cfg => cfg.CreateMap<IDataReader, T>());
            //
            return Mapper.Map<IDataReader, IEnumerable<T>>(reader);
        }
    }
}
