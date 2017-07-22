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
using UnityEngine;

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// 系统信息监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class SystemInfoMonitor
    {
        /// <summary>
        /// 系统信息监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public SystemInfoMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.unique.id", string.Empty,
                new[] {"tags.systeminfo"},
                () => SystemInfo.deviceUniqueIdentifier));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.device.name", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.deviceName));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.device.type", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.deviceType));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.device.model", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.deviceModel));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.processor.type", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.processorType));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.processor.count", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.processorCount));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.processor.frequency", "MHz",
                new[] { "tags.systeminfo" },
                () => SystemInfo.processorFrequency));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.memory.size", "MB",
                new[] { "tags.systeminfo" },
                () => SystemInfo.systemMemorySize));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.operating.family", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.operatingSystemFamily));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.battery.status", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.batteryStatus));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.battery.level", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.batteryLevel < 0f ? "code.unavailable" : SystemInfo.batteryLevel.ToString("P0")));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.support.audio", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.supportsAudio));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.support.location.service", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.supportsLocationService));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.support.accelerometer", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.supportsAccelerometer));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.support.gyroscope", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.supportsGyroscope));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systeminfo.support.vibration", string.Empty,
                new[] { "tags.systeminfo" },
                () => SystemInfo.supportsVibration));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.application.genuine", string.Empty,
                new[] { "tags.systeminfo" },
                () => UnityEngine.Application.genuine));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.application.genuine.check.available", string.Empty,
                new[] { "tags.systeminfo" },
                () => UnityEngine.Application.genuineCheckAvailable));
        }
    }
}
