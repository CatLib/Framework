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
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.deviceUniqueIdentifier", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.deviceUniqueIdentifier));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.deviceName", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.deviceName));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.deviceType", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.deviceType));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.deviceModel", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.deviceModel));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.processorType", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.processorType));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.processorCount", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.processorCount));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.processorFrequency", "MHz",
                new[] { "tags.systemInfo" },
                () => SystemInfo.processorFrequency));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.systemMemorySize", "MB",
                new[] { "tags.systemInfo" },
                () => SystemInfo.systemMemorySize));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.operatingSystemFamily", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.operatingSystemFamily));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.batteryStatus", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.batteryStatus));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.batteryLevel", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.batteryLevel < 0f ? "code.unavailable" : SystemInfo.batteryLevel.ToString("P0")));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.supportsAudio", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.supportsAudio));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.supportsLocationService", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.supportsLocationService));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.supportsAccelerometer", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.supportsAccelerometer));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.supportsGyroscope", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.supportsGyroscope));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.supportsVibration", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.supportsVibration));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.systemInfo.supportsMotionVectors", string.Empty,
                new[] { "tags.systemInfo" },
                () => SystemInfo.supportsMotionVectors));
        }
    }
}
