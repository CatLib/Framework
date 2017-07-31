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
                        this.skipFrames = skipFrames;
                        area.Invoke();
                    }
                    finally
                    {
                        this.skipFrames = old;
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
        /// 将日志推入日志处理器
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        /// <exception cref="InvalidArgumentException">当传入的日志等级无效</exception>
        private void ExecLog(LogLevels level, object message, params object[] context)
        {
            var result = string.Format(message.ToString(), context);
            lock (syncRoot)
            {
                var entry = MakeLogEntry(level, result);
                foreach (var handler in handlers)
                {
                    handler.Handler(entry);
                }
            }
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
