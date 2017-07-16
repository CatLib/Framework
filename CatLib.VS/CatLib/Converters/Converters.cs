/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
using System.Collections.Generic;
using CatLib.API.Config;
using CatLib.API.Converters;
using CatLib.Stl;

namespace CatLib.Converters
{
    /// <summary>
    /// 转换器管理器
    /// </summary>
    public sealed class Converters : IConverters
    {
        /// <summary>
        /// 转换器字典
        /// </summary>
        private readonly Dictionary<Type,Dictionary<Type, ITypeConverter>> coverterDictionary;

        /// <summary>
        /// 构建一个转换器
        /// </summary>
        public Converters()
        {
            coverterDictionary = new Dictionary<Type, Dictionary<Type, ITypeConverter>>();
        }

        /// <summary>
        /// 增加一个转换器
        /// </summary>
        /// <param name="converter">转换器</param>
        public void AddConverter(ITypeConverter converter)
        {
            Guard.Requires<ArgumentNullException>(converter != null);
            Guard.Requires<InvalidOperationException>(converter.From != converter.To);

            Dictionary<Type, ITypeConverter> mapping;
            if (!coverterDictionary.TryGetValue(converter.From, out mapping))
            {
                coverterDictionary[converter.From] = mapping = new Dictionary<Type, ITypeConverter>();
            }

            mapping[converter.To] = converter;
        }

        /// <summary>
        /// 从源类型转为目标类型
        /// </summary>
        /// <param name="from">源类型</param>
        /// <param name="to">目标类型</param>
        /// <param name="source">源数据</param>
        /// <returns>目标数据</returns>
        public object Convert(Type from, Type to, object source)
        {
            Guard.Requires<ArgumentNullException>(from != null);
            Guard.Requires<ArgumentNullException>(to != null);
            Guard.Requires<InvalidOperationException>(from != to);

            ITypeConverter converter;
            if (!TryGetConverter(from, to, out converter))
            {
                throw new ArgumentException("Undefined Converter [" + from + "] to [" + to +"]");
            }

            return converter.ConvertTo(source, to);
        }

        /// <summary>
        /// 从源类型转为目标类型
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源数据</param>
        /// <returns>目标数据</returns>
        public TTarget Convert<TSource, TTarget>(TSource source)
        {
            return (TTarget)Convert(typeof(TSource), typeof(TTarget), source);
        }

        /// <summary>
        /// 从源类型转为目标类型
        /// </summary>
        /// <param name="from">源类型</param>
        /// <param name="to">目标类型</param>
        /// <param name="source">源数据</param>
        /// <param name="target">目标数据</param>
        /// <returns>是否成功转换</returns>
        public bool TryConvert(Type from, Type to, object source, out object target)
        {
            Guard.Requires<ArgumentNullException>(from != null);
            Guard.Requires<ArgumentNullException>(to != null);
            Guard.Requires<InvalidOperationException>(from != to);

            target = null;
            ITypeConverter converter;
            if (!TryGetConverter(from, to, out converter))
            {
                return false;
            }

            target = converter.ConvertTo(source , to);
            return true;
        }

        /// <summary>
        /// 从源类型转为目标类型
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源数据</param>
        /// <param name="target">目标数据</param>
        /// <returns>是否成功转换</returns>
        public bool TryConvert<TSource,TTarget>(TSource source, out TTarget target)
        {
            target = default(TTarget);
            object obj;
            if (!TryConvert(typeof(TSource), typeof(TTarget), source, out obj))
            {
                return false;
            }
            target = (TTarget) obj; 
            return true;
        }

        /// <summary>
        /// 获取类型所需的转换器
        /// </summary>
        /// <param name="from">来源类型</param>
        /// <param name="to">目标类型</param>
        /// <param name="converter">转换器</param>
        /// <returns>是否成功获取转换器</returns>
        private bool TryGetConverter(Type from , Type to, out ITypeConverter converter)
        {
            bool status;
            Dictionary<Type, ITypeConverter> toDictionary;
            converter = null;

            do
            {
                status = coverterDictionary.TryGetValue(from, out toDictionary);
                from = from.BaseType;
            } while (!status && from != null);

            if (!status)
            {
                return false;
            }

            do
            {
                status = toDictionary.TryGetValue(to, out converter);
                to = to.BaseType;
            } while (!status && to != null);

            return status;
        }
    }
}
