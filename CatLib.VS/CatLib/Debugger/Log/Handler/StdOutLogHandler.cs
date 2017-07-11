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

namespace CatLib.Debugger.Log.Handler
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
        /// <param name="log">日志条目</param>
        public void Handler(ILogEntry log)
        {
            Action<object> handler;
            if (mapping.TryGetValue(log.Level, out handler))
            {
                handler.Invoke(log.Message);
            }
        }
    }
}