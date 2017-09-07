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
using CatLib.API.Converters;

namespace CatLib.Converters.Plan
{
    /// <summary>
    /// IList string转string
    /// </summary>
    public sealed class IListStringStringConverter : ITypeConverter
    {
        /// <summary>
        /// 来源类型
        /// </summary>
        public Type From
        {
            get
            {
                return typeof(IList<string>);
            }
        }

        /// <summary>
        /// 目标类型
        /// </summary>
        public Type To
        {
            get
            {
                return typeof(string);
            }
        }

        /// <summary>
        /// 源类型转换到目标类型
        /// </summary>
        /// <param name="source">源类型</param>
        /// <param name="to">目标类型</param>
        /// <returns>目标类型</returns>
        public object ConvertTo(object source, Type to)
        {
            var list = source as IList<string>;
            if (list == null)
            {
                throw new ArgumentException("Input value's Type is not IList<string>");
            }

            return string.Join(";", new List<string>(list).ToArray());
        }
    }
}
