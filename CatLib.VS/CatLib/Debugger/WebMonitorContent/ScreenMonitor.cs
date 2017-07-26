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
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.dpi", "unit.dpi", new[] { "tags.screen" },
                () => Screen.dpi));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.height", "unit.px", new[] { "tags.screen" },
                () => Screen.height));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.width", "unit.px", new[] { "tags.screen" },
                () => Screen.width));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.orientation", "", new[] { "tags.screen" },
                () => Screen.orientation.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.autorotateToLandscapeLeft", "", new[] { "tags.screen" },
                () => Screen.autorotateToLandscapeLeft.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.autorotateToLandscapeRight", "", new[] { "tags.screen" },
                () => Screen.autorotateToLandscapeRight.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.autorotateToPortrait", "", new[] { "tags.screen" },
                () => Screen.autorotateToPortrait.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.autorotateToPortraitUpsideDown", "", new[] { "tags.screen" },
                () => Screen.autorotateToPortraitUpsideDown.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.sleepTimeout", "", new[] { "tags.screen" },
                () =>
                {
                    switch (Screen.sleepTimeout)
                    {
                        case SleepTimeout.NeverSleep:
                            return "code.SleepTimeout.NeverSleep";
                        case SleepTimeout.SystemSetting:
                            return "code.sleepTimeout.SystemSetting";
                    }
                    return Screen.sleepTimeout.ToString();
                }));
        }
    }
}
