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

using System.Collections.Generic;
using CatLib.API.Routing;
using CatLib.Debugger.WebMonitor.Protocol;

namespace CatLib.Debugger.WebMonitor.Controller
{
    /// <summary>
    /// 监控
    /// </summary>
    [Routed("debug://monitor")]
    public class Monitor
    {
        /// <summary>
        /// 获取监控的详细数据
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="monitorStore">监控存储</param>
        [Routed("get-monitors/{page}")]
        public void GetMonitors(IRequest request, IResponse response, MonitorStore monitorStore)
        {
            var outputs = new GetMonitors();

            foreach (var monitor in monitorStore)
            {
                outputs.WriteLine(monitor);
            }

            response.SetContext(outputs);
        }
    }
}
