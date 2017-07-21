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

using CatLib.Debugger.WebMonitor;
using UnityEngine.Profiling;

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// 堆内存
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class HeapMemoryMonitor
    {
        /// <summary>
        /// 构建一个堆内存监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public HeapMemoryMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.DefinedMoitor("memory.heap",
                MonitorHelper.CallbackSize("monitor.memory.heap", () => Profiler.GetMonoUsedSizeLong())
                , 40);
        }
    }
}
