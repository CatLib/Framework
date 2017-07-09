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
using System.Threading;
using CatLib.API.Debugger;

namespace CatLib.Debugger
{
    /// <summary>
    /// 日志条目记录
    /// </summary>
    internal sealed class LogEntry
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        private static long lastId = 0;

        /// <summary>
        /// 日志等级
        /// </summary>
        private readonly LogLevels level;

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevels Level
        {
            get { return level; }
        }

        /// <summary>
        /// 日志内容
        /// </summary>
        private readonly string message;

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// 调用堆栈
        /// </summary>
        private readonly StackTrace stackTrace;

        /// <summary>
        /// 调用堆栈
        /// </summary>
        public StackTrace StackTrace
        {
            get { return stackTrace; }
        }

        /// <summary>
        /// 当前日志条目的分组
        /// </summary>
        private readonly string categroy;

        /// <summary>
        /// 当前日志条目的分组
        /// </summary>
        public string Categroy
        {
            get { return categroy; }
        }

        /// <summary>
        /// 条目id
        /// </summary>
        private readonly long id;

        /// <summary>
        /// 条目id
        /// </summary>
        public long Id
        {
            get { return id; }
        }

        /// <summary>
        /// 日志条目记录
        /// </summary>
        /// <param name="logger">日志系统</param>
        /// <param name="level">日志等级</param>
        /// <param name="message">消息内容</param>
        /// <param name="skipFrams">跳过的帧数</param>
        public LogEntry(Logger logger, LogLevels level, string message, int skipFrams)
        {
            this.level = level;
            this.message = message;
            categroy = string.Empty;
            stackTrace = new StackTrace(skipFrams, true);
            var declaringType = stackTrace.GetFrame(0).GetMethod().DeclaringType;
            if (declaringType != null && logger != null)
            {
                categroy = logger.FindCategroy(declaringType.Namespace);
            }

            id = Interlocked.Increment(ref lastId);
        }
    }
}
