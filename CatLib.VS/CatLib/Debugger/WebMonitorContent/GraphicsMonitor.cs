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
    /// 显卡相关监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class GraphicsMonitor
    {
        /// <summary>
        /// 显卡相关监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public GraphicsMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.graphicsDeviceID", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsDeviceID));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.graphicsDeviceName", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsDeviceName));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.graphicsDeviceVendorID", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsDeviceVendorID));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.graphicsDeviceVendor", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsDeviceVendor));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.graphicsDeviceType", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsDeviceType));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.graphicsDeviceVersion", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsDeviceVersion));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.graphicsMemorySize","unit.size.mb", new[] { "tags.graphics" },
                () => SystemInfo.graphicsMemorySize));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.graphicsMultiThreaded", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsMultiThreaded));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.npotSupport", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.npotSupport));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.maxTextureSize", "unit.px", new[] { "tags.graphics" },
                () => SystemInfo.maxTextureSize));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.maxCubemapSize", "unit.px", new[] { "tags.graphics" },
                () => SystemInfo.maxCubemapSize));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.copyTextureSupport", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.copyTextureSupport));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportedRenderTargetCount", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportedRenderTargetCount));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsSparseTextures", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsSparseTextures));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supports3DTextures", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supports3DTextures));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supports3DRenderTextures", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supports3DRenderTextures));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supports2DArrayTextures", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supports2DArrayTextures));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsShadows", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsShadows));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsRawShadowDepthSampling", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsRawShadowDepthSampling));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsRenderToCubemap", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsRenderToCubemap));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsComputeShaders", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsComputeShaders));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsInstancing", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsInstancing));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsImageEffects", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsImageEffects));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsCubemapArrayTextures", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsCubemapArrayTextures));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.graphicsUVStartsAtTop", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsUVStartsAtTop));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.usesReversedZBuffer", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.usesReversedZBuffer));
        }
    }
}
