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
    /// 屏幕监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ScreenMonitor
    {
        /// <summary>
        /// 屏幕监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public ScreenMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.screen.dpi", "unit.dpi", new[] { "tags.screen" },
                () => Screen.dpi));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.screen.height", "unit.px", new[] { "tags.screen" },
                () => Screen.height));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.screen.width", "unit.px", new[] { "tags.screen" },
                () => Screen.width));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.screen.landscape.left", "", new[] { "tags.screen" },
                () => Screen.autorotateToLandscapeLeft.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.screen.landscape.right", "", new[] { "tags.screen" },
                () => Screen.autorotateToLandscapeRight.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.screen.landscape.portrait", "", new[] { "tags.screen" },
                () => Screen.autorotateToPortrait.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.screen.landscape.portrait.upside.down", "", new[] { "tags.screen" },
                () => Screen.autorotateToPortraitUpsideDown.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("monitor.screen.sleep.time", "", new[] { "tags.screen" },
                () =>
                {
                    switch (Screen.sleepTimeout)
                    {
                        case SleepTimeout.NeverSleep:
                            return "code.screen.sleep.time.never.sleep";
                        case SleepTimeout.SystemSetting:
                            return "code.screen.sleep.time.system.setting";
                    }
                    return Screen.sleepTimeout.ToString();
                }));
        }
    }
}
