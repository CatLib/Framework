﻿/*
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
            monitor.Monitor(new OnceRecordMonitorHandler("Profiler.supported" , string.Empty, new[] { "tag@Profiler" },
                () => Profiler.supported));
            monitor.Monitor(new OnceRecordMonitorHandler("Profiler.enabled", string.Empty, new[] { "tag@Profiler" },
                () => Profiler.enabled));
            monitor.Monitor(new OnceRecordMonitorHandler("Profiler.enabledBinaryLog@file", string.Empty, new[] { "tag@Profiler" },
                () => Profiler.enableBinaryLog? Profiler.logFile : "code.unenable"));
            monitor.Monitor(new SizeMonitorHandler("Profiler.GetMonoUsedSize@memory", new[] { "tag@Profiler" },
                () => Profiler.GetMonoUsedSizeLong()));
            monitor.Monitor(new SizeMonitorHandler("Profiler.GetMonoHeapSize@memory", new[] { "tag@Profiler" },
                () => Profiler.GetMonoHeapSizeLong()));
            monitor.Monitor(new SizeMonitorHandler("Profiler.GetTotalAllocatedMemory", new[] { "tag@Profiler" },
                () => Profiler.GetTotalAllocatedMemoryLong()));
            monitor.Monitor(new SizeMonitorHandler("Profiler.GetTotalReservedMemory", new[] { "tag@Profiler" },
                () => Profiler.GetTotalReservedMemoryLong()));
            monitor.Monitor(new SizeMonitorHandler("Profiler.GetTotalUnusedReservedMemory", new[] { "tag@Profiler" },
                () => Profiler.GetTotalUnusedReservedMemoryLong()));
            monitor.Monitor(new SizeMonitorHandler("Profiler.GetTempAllocatorSize@memory", new[] { "tag@Profiler" },
                () => (long)Profiler.GetTempAllocatorSize()));
            monitor.Monitor(new OnceRecordMonitorHandler("fps", "unit.second.pre", new[] { "tag@Profiler" },
                () => Math.Floor(1.0f / time.SmoothDeltaTime)));
        }
    }
}
