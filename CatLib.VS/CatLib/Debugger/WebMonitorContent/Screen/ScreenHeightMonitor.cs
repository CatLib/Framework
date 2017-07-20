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
using CatLib.Debugger.WebMonitor.Handler;
using UnityEngine;

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// 屏幕宽度监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ScreenHeightMonitor
    {
        /// <summary>
        /// 构建一个屏幕宽度
        /// </summary>
        /// <param name="monitor">监控</param>
        public ScreenHeightMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.DefinedMoitor("screen.width",
                new CallbackMonitorHandler(new OnceRecordMonitorHandler("Screen Height", "px"), GetScreenHeight), 1010);
        }

        /// <summary>
        /// 获取屏幕宽度
        /// </summary>
        /// <returns>Fps</returns>
        private object GetScreenHeight()
        {
            return Screen.width;
        }
    }
}
