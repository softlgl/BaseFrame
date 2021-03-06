﻿using System.Collections;
using System.Collections.Generic;
using AutoMapper;

namespace BaseFrame.Common.Extension
{
    /// <summary>
    /// AutoMapper扩展帮助类
    /// </summary>
    public static class AutoMapperHelper
    {
        /// <summary>
        ///  类型映射
        /// </summary>
        public static T MapTo<T>(this object obj)
        {
            if (obj == null) return default(T);
            var type = obj.GetType();
            Mapper.Initialize(cfg=> cfg.CreateMap(type, typeof(T)));
            return Mapper.Map<T>(obj);
        }
        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TDestination>(this IEnumerable source)
        {

            if(source==null||
                !source.GetEnumerator().MoveNext())
            {
                return new List<TDestination>();
            }

            foreach (var first in source)
            {
                var type = first.GetType();
                Mapper.Initialize(cfg => cfg.CreateMap(type, typeof(TDestination)));
                break;
            }
            return Mapper.Map<List<TDestination>>(source);
        }
        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            //IEnumerable<T> 类型需要创建元素的映射
            Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDestination>());
            return Mapper.Map<List<TDestination>>(source);
        }
        /// <summary>
        /// 类型映射
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
        {
            if (source == null) return destination;
            Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDestination>());
            return Mapper.Map(source, destination);
        }

    }
}
