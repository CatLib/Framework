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
using CatLib.API.Debugger;
using System.Diagnostics;
using System.Threading;

namespace CatLib.Debugger.Log
{
    /// <summary>
    /// 日志条目记录
    /// </summary>
    internal sealed class LogEntry : ILogEntry
    {
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
        /// 记录时间
        /// </summary>
        public long Time { get; private set;}

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
            Id = LogUtil.GetLastId();
            Time = DateTime.Now.Timestamp();
        }

        /// <summary>
        /// 获取调用堆栈
        /// </summary>
        /// <param name="assemblyMatch">程序集是否符合输出条件</param>
        /// <returns>调用堆栈</returns>
        public string[] GetStackTrace(Predicate<string> assemblyMatch = null)
        {
            var callStack = new List<string>(StackTrace.FrameCount);

            for (var i = 0; i < StackTrace.FrameCount; i++)
            {
                var frame = StackTrace.GetFrame(i);
                var method = frame.GetMethod();
                if (method.DeclaringType == null || !assemblyMatch(method.DeclaringType.Assembly.GetName().Name))
                {
                    callStack.Add(string.Format("{0}(at {1}:{2})", method, frame.GetFileName(),
                        frame.GetFileLineNumber()));
                }
            }
            return callStack.ToArray();
        }

        /// <summary>
        /// 是否可以被忽略
        /// </summary>
        /// <param name="type">处理器类型</param>
        /// <returns>是否可以忽略这个处理器</returns>
        public bool IsIgnore(Type type)
        {
            return false;
        }
    }
}
