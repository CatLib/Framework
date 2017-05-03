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

namespace CatLib.Thread
{
    /// <summary>
    /// 线程事件
    /// </summary>
    public sealed class ThreadEvents
    {
        /// <summary>
        /// 当线程执行遇到异常
        /// </summary>
        public static readonly string ON_THREAD_EXECURE_ERROR = "thread.execure.error";
    }
}