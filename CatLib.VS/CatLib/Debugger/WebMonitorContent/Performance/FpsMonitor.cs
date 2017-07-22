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
using CatLib.API.Time;
using CatLib.Debugger.WebMonitor.Handler;
using System;

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// Fps监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class FpsMonitor
    {
        /// <summary>
        /// 构建一个Fps监控
        /// </summary>
        /// <param name="monitor">监控</param>
        /// <param name="time">使用的时间</param>
        public FpsMonitor([Inject(Required = true)]IMonitor monitor,
                            [Inject(Required = true)]ITime time)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.performance.fps", "unit.second.pre", new[] {"tags.common"},
                () => Math.Floor(1.0f / time.SmoothDeltaTime)));
        }
    }
}
