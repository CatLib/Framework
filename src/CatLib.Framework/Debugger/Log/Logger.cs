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
using System;
using System.Collections.Generic;

namespace CatLib.Debugger.Log
{
    /// <summary>
    /// 日志系统
    /// </summary>
    public sealed class Logger : ILogger
    {
        /// <summary>
        /// 日志处理器
        /// </summary>
        private readonly List<ILogHandler> handlers;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// 调用堆栈忽略的步数
        /// </summary>
        private int skipFrames = 4;

        /// <summary>
        /// 调用计数
        /// </summary>
        private int callCount;

        /// <summary>
        /// 构造一个日志系统
        /// </summary>
        public Logger()
        {
            handlers = new List<ILogHandler>();
        }

        /// <summary>
        /// 设定调用堆栈忽略的步数
        /// </summary>
        /// <param name="skipFrames">跳过的步数</param>
        /// <param name="area">作用区域</param>
        public void SetSkip(int skipFrames, Action area = null)
        {
            Guard.Requires<ArgumentException>(skipFrames >= 0);
            if (area != null)
            {
                lock (syncRoot)
                {
                    var old = this.skipFrames;
                    try
                    {
                        if (callCount++ == 0)
                        {
                            this.skipFrames = skipFrames;
                        }
                        area.Invoke();
                    }
                    finally
                    {
                        if (--callCount == 0)
                        {
                            this.skipFrames = old;
                        }
                    }
                }
            }
            else
            {
                this.skipFrames = skipFrames;
            }
        }

        /// <summary>
        /// 增加日志处理器
        /// </summary>
        /// <param name="handler">处理器</param>
        public void AddLogHandler(ILogHandler handler)
        {
            Guard.Requires<ArgumentNullException>(handler != null);
            handlers.Add(handler);
        }

        /// <summary>
        /// 输出一条日志，日志级别为传入的等级
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        /// <exception cref="InvalidArgumentException">当传入的日志等级无效</exception>
        public void Log(LogLevels level, object message, params object[] context)
        {
            ExecLog(level, message, context);
        }

        /// <summary>
        /// 输出日志内容
        /// </summary>
        /// <param name="entry">日志实体</param>
        public void Log(ILogEntry entry)
        {
            if (handlers.Count <= 0)
            {
                return;
            }
            lock (syncRoot)
            {
                foreach (var handler in handlers)
                {
                    if (!entry.IsIgnore(handler.GetType()))
                    {
                        handler.Handler(entry);
                    }
                }
            }
        }

        /// <summary>
        /// 将日志推入日志处理器
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        /// <exception cref="InvalidArgumentException">当传入的日志等级无效</exception>
        private void ExecLog(LogLevels level, object message, params object[] context)
        {
            string messageString;
            if (context == null || context.Length == 0)
            {
                messageString = message.ToString();
            }
            else
            {
                messageString = string.Format(message.ToString(), context);
            }
            Log(MakeLogEntry(level, messageString));
        }

        /// <summary>
        /// 输出一条调试级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Debug(object message, params object[] context)
        {
            ExecLog(LogLevels.Debug, message, context);
        }

        /// <summary>
        /// 输出一条信息级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Info(object message, params object[] context)
        {
            ExecLog(LogLevels.Info, message, context);
        }

        /// <summary>
        /// 输出一条通知级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Notice(object message, params object[] context)
        {
            ExecLog(LogLevels.Notice, message, context);
        }

        /// <summary>
        /// 输出一条警告级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Warning(object message, params object[] context)
        {
            ExecLog(LogLevels.Warning, message, context);
        }

        /// <summary>
        /// 输出一条错误级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Error(object message, params object[] context)
        {
            ExecLog(LogLevels.Error, message, context);
        }

        /// <summary>
        /// 输出一条关键级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Critical(object message, params object[] context)
        {
            ExecLog(LogLevels.Critical, message, context);
        }

        /// <summary>
        /// 输出一条警报级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Alert(object message, params object[] context)
        {
            ExecLog(LogLevels.Alert, message, context);
        }

        /// <summary>
        /// 输出一条紧急级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Emergency(object message, params object[] context)
        {
            ExecLog(LogLevels.Emergency, message, context);
        }

        /// <summary>
        /// 制作一个日志条目
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="message">日志</param>
        /// <returns>日志条目</returns>
        private LogEntry MakeLogEntry(LogLevels level, string message)
        {
            return new LogEntry(level, message, skipFrames);
        }
    }
}
