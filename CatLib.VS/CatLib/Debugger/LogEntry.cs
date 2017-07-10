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

using CatLib.API.Debugger;
using System.Diagnostics;
using System.Threading;

namespace CatLib.Debugger
{
    /// <summary>
    /// 日志条目记录
    /// </summary>
    internal sealed class LogEntry : ILogEntry
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        private static long lastId;

        /// <summary>
        /// 条目id
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevels Level { get; private set; }

        /// <summary>
        /// 调用堆栈
        /// </summary>
        public StackTrace StackTrace { get; private set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// 日志条目记录
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="message">消息内容</param>
        /// <param name="skipFrams">跳过的帧数</param>
        public LogEntry(LogLevels level, string message, int skipFrams)
        {
            Level = level;
            Message = message;
            StackTrace = new StackTrace(skipFrams, true);
            var declaringType = StackTrace.GetFrame(0).GetMethod().DeclaringType;
            if (declaringType != null)
            {
                Namespace = declaringType.Namespace;
            }
            Id = Interlocked.Increment(ref lastId);
        }
    }
}
