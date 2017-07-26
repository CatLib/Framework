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
    /// 输入触摸相关监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class InputMonitor
    {
        /// <summary>
        /// 输入触摸相关监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public InputMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("Input.touchSupported", string.Empty, new[] { "tags.input" },
                () => Input.touchSupported));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.touchPressureSupported", string.Empty, new[] { "tags.input" },
                () => Input.touchPressureSupported));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.stylusTouchSupported", string.Empty, new[] { "tags.input" },
                () => Input.stylusTouchSupported));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.simulateMouseWithTouches", string.Empty, new[] { "tags.input" },
                () => Input.simulateMouseWithTouches));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.multiTouchEnabled", string.Empty, new[] { "tags.input" },
                () => Input.multiTouchEnabled));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.touchCount", string.Empty, new[] { "tags.input" },
                () => Input.touchCount));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.touches", string.Empty, new[] { "tags.input" },
                () =>
                {
                    var touches = Input.touches;
                    var touchStrings = new string[touches.Length];
                    for (var i = 0; i < touches.Length; i++)
                    {
                        touchStrings[i] = string.Format("pos {0}, delta pos {1}, raw pos {2}, pressure {3}, {4}", 
                                                            touches[i].position, 
                                                            touches[i].deltaPosition, 
                                                            touches[i].rawPosition, 
                                                            touches[i].pressure, 
                                                            touches[i].phase);
                    }
                    return string.Join("; ", touchStrings);
                }));

            monitor.Monitor(new OnceRecordMonitorHandler("Input.backButtonLeavesApp", string.Empty, new[] { "tags.input" },
                () => Input.backButtonLeavesApp));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.deviceOrientation", string.Empty, new[] { "tags.input" },
                () => Input.deviceOrientation));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.mousePresent", string.Empty, new[] { "tags.input" },
                () => Input.mousePresent));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.mousePosition", string.Empty, new[] { "tags.input" },
                () => Input.mousePosition));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.mouseScrollDelta", string.Empty, new[] { "tags.input" },
                () => Input.mouseScrollDelta));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.anyKey", string.Empty, new[] { "tags.input" },
                () => Input.anyKey));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.imeIsSelected", string.Empty, new[] { "tags.input" },
                () => Input.imeIsSelected));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.imeCompositionMode", string.Empty, new[] { "tags.input" },
                () => Input.imeCompositionMode));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compensateSensors", string.Empty, new[] { "tags.input" },
                () => Input.compensateSensors));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compositionCursorPos", string.Empty, new[] { "tags.input" },
                () => Input.compositionCursorPos));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compositionString", string.Empty, new[] { "tags.input" },
                () => Input.compositionString));
        }
    }
}
