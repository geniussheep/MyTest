using System;
using System.Collections.Generic;
using System.Data;
using AutoMapper;
using AutoMapper.Configuration;

namespace AutoMapperTest
{
    /// <summary>
    /// AutoMapper扩展帮助类
    /// </summary>
    public static class AutoMapperHelper
    {
        private static readonly Dictionary<Type, List<Type>> MapperCache = new Dictionary<Type, List<Type>>();

        private static readonly object LockObj = new object();

        private static readonly MapperConfigurationExpression MapperCfg = new MapperConfigurationExpression();

        private static void InitMapper(Type sourceType, Type destinationType)
        {
            lock (LockObj)
            {
                if (!MapperCache.ContainsKey(sourceType))
                {
                    MapperCache.Add(sourceType, new List<Type> { destinationType});
                    MapperCfg.CreateMap(sourceType, destinationType);
                    Mapper.Initialize(MapperCfg);
                    return;
                }
                if (!MapperCache[sourceType].Contains(destinationType))
                {
                    MapperCache[sourceType].Add(destinationType);
                    MapperCfg.CreateMap(sourceType, destinationType);
                    Mapper.Initialize(MapperCfg);
                }
            }
        }

        /// <summary>
        ///  类型映射
        /// </summary>
        public static TD AutoMapTo<TS,TD>(this TS obj)
        {
            if (obj == null) return default(TD);
            InitMapper(typeof(TS),typeof(TD));
            return Mapper.Map<TD>(obj);
        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TD> MapToList<TS, TD>(this IEnumerable<TS> source)
        {
            InitMapper(typeof(TS),typeof(TD));
            return Mapper.Map<List<TD>>(source);
        }

        /// <summary>
        /// 类型映射
        /// </summary>
        public static TD AutoMapTo<TS, TD>(this TS source, TD dto)
            where TS : class
            where TD : class
        {
            if (source == null) return dto;
            InitMapper(typeof(TS),typeof(TD));
            return Mapper.Map(source, dto);
        }

        /// <summary>
        /// DataReader映射
        /// </summary>
        public static IEnumerable<TD> DataReaderMapTo<TD>(this IDataReader reader)
        {
            InitMapper(typeof(IDataReader),typeof(IEnumerable<TD>));
            return Mapper.Map<IDataReader, IEnumerable<TD>>(reader);
        }
    }
}
