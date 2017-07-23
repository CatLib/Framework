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
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.graphicsDeviceID", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsDeviceID));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.graphicsDeviceName", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsDeviceName));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.graphicsDeviceVendorID", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsDeviceVendorID));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.graphicsDeviceVendor", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsDeviceVendor));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.graphicsDeviceType", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsDeviceType));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.graphicsDeviceVersion", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsDeviceVersion));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.graphicsMemorySize","unit.size.mb", new[] { "tags.graphics" },
                () => SystemInfo.graphicsMemorySize));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.graphicsMultiThreaded", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsMultiThreaded));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.npotSupport", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.npotSupport));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.maxTextureSize", "unit.px", new[] { "tags.graphics" },
                () => SystemInfo.maxTextureSize));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.maxCubemapSize", "unit.px", new[] { "tags.graphics" },
                () => SystemInfo.maxCubemapSize));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.copyTextureSupport", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.copyTextureSupport));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.supportedRenderTargetCount", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportedRenderTargetCount));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.supportsSparseTextures", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsSparseTextures));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.supports3DTextures", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supports3DTextures));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.supports3DRenderTextures", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supports3DRenderTextures));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.supports2DArrayTextures", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supports2DArrayTextures));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.supportsShadows", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsShadows));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.supportsRawShadowDepthSampling", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsRawShadowDepthSampling));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.supportsRenderToCubemap", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsRenderToCubemap));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.supportsComputeShaders", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsComputeShaders));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.supportsInstancing", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsInstancing));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.supportsImageEffects", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsImageEffects));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.supportsCubemapArrayTextures", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.supportsCubemapArrayTextures));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.graphicsUVStartsAtTop", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.graphicsUVStartsAtTop));
            monitor.Monitor(new OnceRecordMonitorHandler("systemInfo.usesReversedZBuffer", string.Empty, new[] { "tags.graphics" },
                () => SystemInfo.usesReversedZBuffer));
        }
    }
}
