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
        /// 页面对应的监控
        /// </summary>
        private readonly IDictionary<string, string[]> pages;

        /// <summary>
        /// 监控
        /// </summary>
        public Monitor()
        {
            pages = new Dictionary<string, string[]>
            {
                { "index" , new []{"fps.counter","memory.heap","memory.total"}},
                { "test" , new[]{ "test" } }
            };
        }

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

            string[] monitors;
            if (pages.TryGetValue(request.Get("page"), out monitors))
            {
                foreach (var monitor in monitors)
                {
                    var handler = monitorStore.FindMoitor(monitor);
                    if (handler != null)
                    {
                        outputs.WriteLine(handler);
                    }
                }
            }

            response.SetContext(outputs);
        }
    }
}
