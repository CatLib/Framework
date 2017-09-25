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
    /// 类型转换器
    /// </summary>
    public interface ITypeConverter
    {
        /// <summary>
        /// 来源类型
        /// </summary>
        Type From { get; }

        /// <summary>
        /// 目标类型
        /// </summary>
        Type To { get; }

        /// <summary>
        /// 源类型转换到目标类型
        /// </summary>
        /// <param name="source">源类型</param>
        /// <param name="to">目标类型</param>
        /// <returns>目标类型</returns>
        object ConvertTo(object source, Type to);
    }
}
