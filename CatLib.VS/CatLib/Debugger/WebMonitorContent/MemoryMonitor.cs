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

using CatLib.Debugger.WebMonitor.Handler;
using UnityEngine.Profiling;

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// 内存监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class MemoryMonitor
    {
        /// <summary>
        /// 内存监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public MemoryMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new SizeMonitorHandler("monitor.memory.heap", new[] { "tags.common" },
                () => Profiler.GetMonoUsedSizeLong()));
            monitor.Monitor(new SizeMonitorHandler("monitor.memory.total", new[] { "tags.common" },
                () => Profiler.GetTotalAllocatedMemoryLong()));
        }
    }
}
