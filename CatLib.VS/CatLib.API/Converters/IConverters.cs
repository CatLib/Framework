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

namespace CatLib.API.Converters
{
    /// <summary>
    /// 转换器
    /// </summary>
    public interface IConverters
    {
        /// <summary>
        /// 增加一个转换器
        /// </summary>
        /// <param name="converter">转换器</param>
        void AddConverter(ITypeConverter converter);

        /// <summary>
        /// 从源类型转为目标类型
        /// </summary>
        /// <param name="from">源类型</param>
        /// <param name="to">目标类型</param>
        /// <param name="source">源数据</param>
        /// <returns>目标数据</returns>
        object Convert(Type from, Type to, object source);

        /// <summary>
        /// 从源类型转为目标类型
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源数据</param>
        /// <returns>目标数据</returns>
        TTarget Convert<TSource, TTarget>(TSource source);

        /// <summary>
        /// 从源类型转为目标类型
        /// </summary>
        /// <param name="from">源类型</param>
        /// <param name="to">目标类型</param>
        /// <param name="source">源数据</param>
        /// <param name="target">目标数据</param>
        /// <returns>是否成功转换</returns>
        bool TryConvert(Type from, Type to, object source, out object target);

        /// <summary>
        /// 从源类型转为目标类型
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源数据</param>
        /// <param name="target">目标数据</param>
        /// <returns>是否成功转换</returns>
        bool TryConvert<TSource, TTarget>(TSource source, out TTarget target);
    }
}
