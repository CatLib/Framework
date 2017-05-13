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

namespace CatLib.Stl
{
    /// <summary>
    /// 守卫
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// 验证一个条件,并在该协定的条件失败时引发异常。
        /// </summary>
        /// <typeparam name="TException">异常</typeparam>
        /// <param name="condition">条件</param>
        [System.Diagnostics.DebuggerNonUserCode]
        public static void Requires<TException>(bool condition) where TException : Exception , new()
        {
            if (condition)
            {
                return;
            }
            throw new TException();
        }

        /// <summary>
        /// 不为空或者null
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名</param>
        [System.Diagnostics.DebuggerNonUserCode]
        public static void NotEmptyOrNull(string argumentValue, string argumentName)
        {
            if (string.IsNullOrEmpty(argumentValue))
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// 内容不为空
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名</param>
        [System.Diagnostics.DebuggerNonUserCode]
        public static void NotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}