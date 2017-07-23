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

using CatLib.API.Time;
using CatLib.Debugger.WebMonitor.Handler;
using System;
using UnityEngine.Profiling;

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// 性能相关监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class PerformanceMonitor
    {
        /// <summary>
        /// 构建一个性能监控
        /// </summary>
        /// <param name="monitor">监控</param>
        /// <param name="time">使用的时间</param>
        public PerformanceMonitor([Inject(Required = true)]IMonitor monitor,
                            [Inject(Required = true)]ITime time)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.performance.profiler.supported" , string.Empty, new[] { "tags.performance" },
                () => Profiler.supported));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.performance.profiler.enabled", string.Empty, new[] { "tags.performance" },
                () => Profiler.enabled));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.performance.profiler.binaryLog.enabled", string.Empty, new[] { "tags.performance" },
                () => Profiler.enableBinaryLog? Profiler.logFile : "code.unenable"));
            monitor.Monitor(new SizeMonitorHandler("monitor.performance.memory.monoUsedSize", new[] { "tags.performance" },
                () => Profiler.GetMonoUsedSizeLong()));
            monitor.Monitor(new SizeMonitorHandler("monitor.performance.memory.monoHeapSize", new[] { "tags.performance" },
                () => Profiler.GetMonoHeapSizeLong()));
            monitor.Monitor(new SizeMonitorHandler("monitor.performance.memory.totalAllocatedMemory", new[] { "tags.performance" },
                () => Profiler.GetTotalAllocatedMemoryLong()));
            monitor.Monitor(new SizeMonitorHandler("monitor.performance.memory.totalReservedMemory", new[] { "tags.performance" },
                () => Profiler.GetTotalReservedMemoryLong()));
            monitor.Monitor(new SizeMonitorHandler("monitor.performance.memory.totalUnusedReservedMemory", new[] { "tags.performance" },
                () => Profiler.GetTotalUnusedReservedMemoryLong()));
            monitor.Monitor(new SizeMonitorHandler("monitor.performance.memory.tempAllocatorSize", new[] { "tags.performance" },
                () => (long)Profiler.GetTempAllocatorSize()));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.performance.fps", "unit.second.pre", new[] { "tags.performance" },
                () => Math.Floor(1.0f / time.SmoothDeltaTime)));
        }
    }
}
