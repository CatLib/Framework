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
using CatLib.Debugger.LogHandler;
using CatLib.Stl;
using System;
using System.Collections.Generic;
using CatLib.API.Routing;

namespace CatLib.Debugger
{
    /// <summary>
    /// 日志系统
    /// </summary>
    public sealed class Logger : ILogger , ILogCategory
    {
        /// <summary>
        /// 日志处理器
        /// </summary>
        private readonly List<ILogHandler> handlers;

        /// <summary>
        /// 分组信息
        /// </summary>
        private readonly Dictionary<string, string> categroy;

        /// <summary>
        /// 构造一个日志系统
        /// </summary>
        public Logger()
        {
            handlers = new List<ILogHandler>();
            categroy = new Dictionary<string, string>();
        }

        /// <summary>
        /// 定义命名空间对应的分类
        /// </summary>
        /// <param name="namespaces">该命名空间下的输出的调试语句将会被归属当前定义的组</param>
        /// <param name="categroyName">分类名(用于在调试控制器显示)</param>
        public void DefinedCategory(string namespaces, string categroyName)
        {
            categroy[namespaces] = categroyName;
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
            var result = string.Format(message.ToString(), context);
            foreach (var handler in handlers)
            {
                handler.Handler(level, result);
            }
        }

        /// <summary>
        /// 输出一条调试级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Debug(object message, params object[] context)
        {
            Log(LogLevels.Debug, message , context);
        }

        /// <summary>
        /// 输出一条信息级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Info(object message, params object[] context)
        {
            Log(LogLevels.Info, message, context);
        }

        /// <summary>
        /// 输出一条通知级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Notice(object message, params object[] context)
        {
            Log(LogLevels.Notice, message, context);
        }

        /// <summary>
        /// 输出一条警告级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Warning(object message, params object[] context)
        {
            Log(LogLevels.Warning, message, context);
        }

        /// <summary>
        /// 输出一条错误级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Error(object message, params object[] context)
        {
            Log(LogLevels.Error, message, context);
        }

        /// <summary>
        /// 输出一条关键级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Critical(object message, params object[] context)
        {
            Log(LogLevels.Critical, message, context);
        }

        /// <summary>
        /// 输出一条警报级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Alert(object message, params object[] context)
        {
            Log(LogLevels.Alert, message, context);
        }

        /// <summary>
        /// 输出一条紧急级日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Emergency(object message, params object[] context)
        {
            Log(LogLevels.Emergency, message, context);
        }

        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        [Routed("get-logger/{lastId?}", Defaults = "lastId=>0")]
        public void GetLogger(IRequest request, IResponse response)
        {
            response.SetContext("hello world");
        }

        /// <summary>
        /// 传入一个命名空间字符串查找对应的分组
        /// </summary>
        /// <param name="namespaces">命名空间</param>
        /// <returns>对应分组名</returns>
        internal string FindCategroy(string namespaces)
        {
            string categroy;
            this.categroy.TryGetValue(namespaces, out categroy);
            return categroy;
        }
    }
}
