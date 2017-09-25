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
using System.Collections;
using System.Collections.Generic;

namespace CatLib.Debugger.WebMonitor
{
    /// <summary>
    /// 监控器
    /// </summary>
    public sealed class MonitorStore : IMonitor , IEnumerable<IMonitorHandler>
    {
        /// <summary>
        /// 监控处理器
        /// </summary>
        private readonly List<IMonitorHandler> monitors;

        /// <summary>
        /// 监控处理器字典
        /// </summary>
        private readonly Dictionary<string, IMonitorHandler> monitorsDict;

        /// <summary>
        /// 监控器
        /// </summary>
        public MonitorStore()
        {
            monitorsDict = new Dictionary<string, IMonitorHandler>();
            monitors = new List<IMonitorHandler>();
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        IEnumerator<IMonitorHandler> IEnumerable<IMonitorHandler>.GetEnumerator()
        {
            return monitors.GetEnumerator();
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return monitors.GetEnumerator();
        }

        /// <summary>
        /// 增加监控
        /// </summary>
        /// <param name="handler">监控句柄</param>
        public void Monitor(IMonitorHandler handler)
        {
            Guard.Requires<ArgumentNullException>(handler != null);
            if (monitorsDict.ContainsKey(handler.Name))
            {
                throw new RuntimeException("Monitor [" + handler.Name + "] is already exists");
            }
            monitors.Add(handler);
            monitorsDict.Add(handler.Name, handler);
        }

        /// <summary>
        /// 搜索监控处理器
        /// </summary>
        /// <param name="monitorName">监控名</param>
        internal IMonitorHandler FindMoitor(string monitorName)
        {
            IMonitorHandler handler;
            if (monitorsDict.TryGetValue(monitorName, out handler))
            {
                return handler;
            }
            return null;
        }
    }
}
