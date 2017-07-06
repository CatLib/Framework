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
    /// 监控器
    /// </summary>
    public sealed class Monitors : IMonitor
    {
        /// <summary>
        /// 监控处理器
        /// </summary>
        private readonly Dictionary<string, IMonitorHandler> monitors;

        /// <summary>
        /// 监控处理结果
        /// </summary>
        private readonly Dictionary<string, string> monitorResults;

        /// <summary>
        /// 监控器
        /// </summary>
        public Monitors()
        {
            monitors = new Dictionary<string, IMonitorHandler>();
            monitorResults = new Dictionary<string, string>();
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
    }
}
