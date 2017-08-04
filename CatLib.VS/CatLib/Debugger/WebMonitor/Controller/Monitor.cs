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
using CatLib.API.Routing;
using CatLib.Debugger.WebMonitor.Protocol;
using System.Collections.Generic;

namespace CatLib.Debugger.WebMonitor.Controller
{
    /// <summary>
    /// 监控
    /// </summary>
    [Routed("debug://monitor", Group = "Debugger.MainThreadCallWithContext")]
    public class Monitor
    {
        /// <summary>
        /// 获取首页的监控
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="indexShow">首先显示的列表</param>
        /// <param name="monitorStore">容器存储</param>
        [Routed("get-monitors-index")]
        public void GetMonitorsIndex(IRequest request, IResponse response, [Inject("DebuggerProvider.IndexMonitor")]IEnumerable<string> indexShow, MonitorStore monitorStore)
        {
            var outputs = new GetMonitors();
            Action action = () =>
            {
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
            };

            CallMainThread(request, action);
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

            Action action = () =>
            {
                foreach (var monitor in monitorStore)
                {
                    outputs.WriteLine(monitor);
                }

                response.SetContext(outputs);
            };

            CallMainThread(request, action);
        }

        /// <summary>
        /// 主线程调用
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="action">调用方法</param>
        private void CallMainThread(IRequest request , Action action)
        {
            var mainThread = request.GetContext() as Action<Action>;
            if (mainThread != null)
            {
                mainThread.Invoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
