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

using CatLib.Support;
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
        private readonly Dictionary<string, IMonitorHandler> monitors;

        /// <summary>
        /// 监控处理器
        /// </summary>
        private readonly SortSet<IMonitorHandler, int> monitorsSort;

        /// <summary>
        /// 监控器
        /// </summary>
        public MonitorStore()
        {
            monitors = new Dictionary<string, IMonitorHandler>();
            monitorsSort = new SortSet<IMonitorHandler, int>();
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        IEnumerator<IMonitorHandler> IEnumerable<IMonitorHandler>.GetEnumerator()
        {
            return monitorsSort.GetEnumerator();
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
        /// 设定监控处理器
        /// </summary>
        /// <param name="monitorName">监控名</param>
        /// <param name="handler">监控处理器</param>
        /// <param name="sort">排序</param>
        public void DefinedMoitor(string monitorName, IMonitorHandler handler, int sort = int.MaxValue)
        {
            Guard.NotEmptyOrNull(monitorName, "moitorName");
            Guard.Requires<ArgumentNullException>(handler != null);
            Guard.Requires<ArgumentException>(!monitorsSort.Contains(handler));
            monitors.Add(monitorName, handler);
            monitorsSort.Add(handler, sort);
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
            if ((handler = FindMoitor(monitorName)) != null)
            {
                handler.Handler(value);
            }
        }

        /// <summary>
        /// 搜索监控处理器
        /// </summary>
        /// <param name="monitorName">监控名</param>
        internal IMonitorHandler FindMoitor(string monitorName)
        {
            IMonitorHandler handler;
            if (monitors.TryGetValue(monitorName, out handler))
            {
                return handler;
            }
            return null;
        }
    }
}
