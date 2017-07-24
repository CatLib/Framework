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

using CatLib.API.Converters;
using System;

namespace CatLib.Converters.Plan
{
    /// <summary>
    /// string转bool转换器
    /// </summary>
    public sealed class StringBoolConverter : ITypeConverter
    {
        /// <summary>
        /// 来源
        /// </summary>
        public Type From
        {
            get
            {
                return typeof(string);
            }
        }

        /// <summary>
        /// 目标
        /// </summary>
        public Type To
        {
            get
            {
                return typeof(bool);
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
            switch (source.ToString().ToLowerInvariant())
            {
                case "false":
                case "off":
                case "no":
                case "n":
                case "0":
                    return false;
                case "true":
                case "on":
                case "yes":
                case "y":
                case "1":
                    return true;
            }

            throw new ConverterException(source.GetType(), to, source);
        }
    }
}
