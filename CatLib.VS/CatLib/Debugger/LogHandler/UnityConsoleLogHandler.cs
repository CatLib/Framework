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
using UnityEngine;

namespace CatLib.Debugger.LogHandler
{
    /// <summary>
    /// Unity控制台日志处理器
    /// </summary>
    class UnityConsoleLogHandler : ILogHandler
    {
        /// <summary>
        /// 实际处理方法
        /// </summary>
        private Dictionary<LogLevels, Action<object>> mapping;

        /// <summary>
        /// Unity控制台日志处理器
        /// </summary>
        public UnityConsoleLogHandler()
        {
            mapping = new Dictionary<LogLevels, Action<object>>()
            {
                { LogLevels.Emergency , Debug.LogError },
                { LogLevels.Alert , Debug.LogError },
                { LogLevels.Critical , Debug.LogError },
                { LogLevels.Error, Debug.LogError },
                { LogLevels.Warning, Debug.LogWarning },
                { LogLevels.Notice, Debug.Log },
                { LogLevels.Info, Debug.Log },
                { LogLevels.Debug , Debug.Log }
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
