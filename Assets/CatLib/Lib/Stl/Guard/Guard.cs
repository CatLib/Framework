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
    }
}