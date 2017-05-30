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
using CatLib.API;

namespace CatLib.Config
{
    /// <summary>
    /// 转换异常
    /// </summary>
    public sealed class ConverterException : RuntimeException
    {
        /// <summary>
        /// 转换异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public ConverterException(string message) : base(message)
        {
        }

        /// <summary>
        /// 转换异常
        /// </summary>
        /// <param name="field">异常字段</param>
        /// <param name="to">转换的类型</param>
        public ConverterException(string field, Type to)
            : base("[" + field + "] Can not to change to type [" + to + "]")
        {
        }
    }
}