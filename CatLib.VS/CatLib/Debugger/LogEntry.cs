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

using System.Diagnostics;

namespace CatLib.Debugger
{
    /// <summary>
    /// 日志条目记录
    /// </summary>
    internal sealed class LogEntry
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        private Logger logger;

        /// <summary>
        /// 日志内容
        /// </summary>
        private string message;

        /// <summary>
        /// 调用堆栈
        /// </summary>
        private StackTrace stackTrace;

        /// <summary>
        /// 当前日志条目的分组
        /// </summary>
        private string categroy;

        /// <summary>
        /// 日志条目记录
        /// </summary>
        /// <param name="logger">日志系统</param>
        /// <param name="message">消息内容</param>
        private LogEntry(Logger logger , string message)
        {
            this.logger = logger;
            this.message = message;
            stackTrace = new StackTrace();
            categroy = logger.FindCategroy(stackTrace.GetFrame(0).GetMethod().Name);
        }
    }
}
