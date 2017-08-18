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

using System.Threading;

namespace CatLib.Debugger.Log
{
    /// <summary>
    /// Log通用工具
    /// </summary>
    public static class LogUtil
    {
        /// <summary>
        /// 日志LastId
        /// </summary>
        private static long lastId;

        /// <summary>
        /// 获取日志LastId
        /// </summary>
        /// <returns>日志LastId</returns>
        public static long GetLastId()
        {
            return Interlocked.Increment(ref lastId);
        }
    }
}
