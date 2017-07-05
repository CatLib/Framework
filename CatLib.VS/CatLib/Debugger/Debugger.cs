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
using CatLib.API;
using CatLib.API.Debugger;
using CatLib.Stl;

namespace CatLib.Debugger
{
    /// <summary>
    /// 调试器
    /// </summary>
    internal sealed class Debugger : IDebugger
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// 监控处理器
        /// </summary>
        private readonly Dictionary<string ,IMonitorHandler> monitors;

        /// <summary>
        /// 监控处理结果
        /// </summary>
        private readonly Dictionary<string, string> monitorResults;

        /// <summary>
        /// 构造一个调试器
        /// </summary>
        internal Debugger()
        {
            monitors = new Dictionary<string, IMonitorHandler>();
            monitorResults = new Dictionary<string, string>();
        }

        /// <summary>
        /// 设定记录器实例接口
        /// </summary>
        /// <param name="logger">记录器</param>
        public void SetLogger(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 定义命名空间对应的分类
        /// </summary>
        /// <param name="namespaces">该命名空间下的输出的调试语句将会被归属当前定义的组</param>
        /// <param name="categroyName">分类名(用于在调试控制器显示)</param>
        public void DefinedCategory(string namespaces, string categroyName)
        {
            
        }

        /// <summary>
        /// 设定监控处理器
        /// </summary>
        /// <param name="monitorName">监控名</param>
        /// <param name="handler">执行句柄</param>
        public void SetMonitorHandler(string monitorName, IMonitorHandler handler)
        {
            Guard.NotEmptyOrNull(monitorName, "moitorName");
            Guard.Requires<ArgumentNullException>(handler != null);
            monitors[monitorName] = handler;
        }

        /// <summary>
        /// 监控一个内容
        /// </summary>
        /// <param name="monitorName">监控名</param>
        /// <param name="value">监控值</param>
        public void Monitor(string monitorName, object value)
        {
            Guard.NotEmptyOrNull(monitorName, "moitorName");
            IMonitorHandler handler;
            if (!monitors.TryGetValue(monitorName, out handler))
            {
                throw new RuntimeException("You must SetMonitorHandler with [" + monitorName + "]");
            }

            var result = handler.Handler(value);
            monitorResults[monitorName] = result;
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
            if (logger != null)
            {
                logger.Log(level, message, context);
            }
        }

        /// <summary>
        /// 输出一条调试级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Debug(object message, params object[] context)
        {
            if (logger != null)
            {
                logger.Debug(message, context);
            }
        }

        /// <summary>
        /// 输出一条信息级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Info(object message, params object[] context)
        {
            if (logger != null)
            {
                logger.Info(message, context);
            }
        }

        /// <summary>
        /// 输出一条通知级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Notice(object message, params object[] context)
        {
            if (logger != null)
            {
                logger.Notice(message, context);
            }
        }

        /// <summary>
        /// 输出一条警告级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Warning(object message, params object[] context)
        {
            if (logger != null)
            {
                logger.Warning(message, context);
            }
        }

        /// <summary>
        /// 输出一条错误级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Error(object message, params object[] context)
        {
            if (logger != null)
            {
                logger.Error(message, context);
            }
        }

        /// <summary>
        /// 输出一条关键级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Critical(object message, params object[] context)
        {
            if (logger != null)
            {
                logger.Critical(message, context);
            }
        }

        /// <summary>
        /// 输出一条警报级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Alert(object message, params object[] context)
        {
            if (logger != null)
            {
                logger.Alert(message, context);
            }
        }

        /// <summary>
        /// 输出一条紧急级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Emergency(object message, params object[] context)
        {
            if (logger != null)
            {
                logger.Emergency(message, context);
            }
        }
    }
}
