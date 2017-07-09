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
using CatLib.API.Routing;
using CatLib.Stl;
using System;
using System.Collections.Generic;

namespace CatLib.Debugger
{
    /// <summary>
    /// 监控器
    /// </summary>
    [Routed]
    public sealed class Monitors : IMonitor
    {
        /// <summary>
        /// 监控处理器
        /// </summary>
        private readonly Dictionary<string, IMonitorHandler> monitors;

        /// <summary>
        /// 监控器
        /// </summary>
        public Monitors()
        {
            monitors = new Dictionary<string, IMonitorHandler>();
        }

        /// <summary>
        /// 设定监控处理器
        /// </summary>
        /// <param name="monitorName">监控名</param>
        /// <param name="handler">执行句柄</param>
        public void DefinedMoitor(string monitorName, IMonitorHandler handler)
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
            if (monitors.TryGetValue(monitorName, out handler))
            {
                handler.Handler(value);
            }
        }

        /// <summary>
        /// 获取监控的详细数据
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        [Routed("debug://monitors/get-monitors/{limit?}" , Defaults = "limit=>6")]
        public void GetMonitors(IRequest request, IResponse response)
        {
            response.SetContext("hello world");
        }
    }
}
