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

namespace CatLib.Debugger.LogHandler
{
    /// <summary>
    /// 标准输出日志处理器
    /// </summary>
    public sealed class StdOutLogHandler : ILogHandler
    {
        /// <summary>
        /// 实际处理方法
        /// </summary>
        private readonly Dictionary<LogLevels, Action<object>> mapping;

        /// <summary>
        /// 标准输出日志处理器
        /// </summary>
        public StdOutLogHandler()
        {
            mapping = new Dictionary<LogLevels, Action<object>>
            {
                {LogLevels.Emergency, Console.WriteLine},
                {LogLevels.Alert, Console.WriteLine},
                {LogLevels.Critical, Console.WriteLine},
                {LogLevels.Error, Console.WriteLine},
                {LogLevels.Warning, Console.WriteLine},
                {LogLevels.Notice, Console.WriteLine},
                {LogLevels.Info, Console.WriteLine},
                {LogLevels.Debug, Console.WriteLine}
            };
        }

        /// <summary>
        /// 日志处理器
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="message">日志内容</param>
        public void Handler(LogLevels level, string message)
        {
            Action<object> handler;
            if (mapping.TryGetValue(level, out handler))
            {
                handler.Invoke(message);
            }
        }
    }
}