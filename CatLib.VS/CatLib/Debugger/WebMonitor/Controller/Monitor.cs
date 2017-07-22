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
using CatLib.API.MonoDriver;
using CatLib.API.Routing;
using CatLib.Debugger.WebMonitor.Protocol;
using System.Threading;

namespace CatLib.Debugger.WebMonitor.Controller
{
    /// <summary>
    /// 监控
    /// </summary>
    [Routed("debug://monitor")]
    public class Monitor : IMiddleware
    {
        /// <summary>
        /// Mono驱动器
        /// </summary>
        [Inject]
        public IMonoDriver Driver { get; set; }

        /// <summary>
        /// 路由请求过滤链
        /// </summary>
        public IFilterChain<IRequest, IResponse> Middleware
        {
            get
            {
                var filterChain = new FilterChain<IRequest, IResponse>();

                if (Driver != null)
                {
                    filterChain.Add((request, response, next) =>
                    {
                        var wait = new AutoResetEvent(false);
                        Driver.MainThread(() =>
                        {
                            try
                            {
                                next(request, response);
                            }
                            finally
                            {
                                wait.Set();
                            }
                        });
                        wait.WaitOne();
                    });
                }

                return filterChain;
            }
        }

        /// <summary>
        /// 获取首页的监控
        /// </summary>
        /// <param name="response">响应</param>
        /// <param name="indexShow">首先显示的列表</param>
        /// <param name="monitorStore">容器存储</param>
        [Routed("get-monitors-index")]
        public void GetMonitorsIndex(IResponse response , [Inject("Debugger.WebMonitor.Monitor.IndexMonitor")]IEnumerable<string> indexShow, MonitorStore monitorStore)
        {
            var outputs = new GetMonitors();

            if (indexShow != null)
            {
                foreach (var monitor in indexShow)
                {
                    var result = monitorStore.FindMoitor(monitor);
                    if (result != null)
                    {
                        outputs.WriteLine(result);
                    }
                }
            }

            response.SetContext(outputs);
        }

        /// <summary>
        /// 获取监控的详细数据
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="monitorStore">监控存储</param>
        [Routed("get-monitors")]
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
