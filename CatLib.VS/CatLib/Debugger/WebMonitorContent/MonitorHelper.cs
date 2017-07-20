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

using System;
using CatLib.Debugger.WebMonitor;
using CatLib.Debugger.WebMonitor.Handler;

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// 监控辅助工具
    /// </summary>
    internal sealed class MonitorHelper
    {
        /// <summary>
        /// 单次记录监控
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="unit">单位</param>
        /// <param name="callback">回调获取值</param>
        /// <returns>监控句柄</returns>
        public static IMonitorHandler CallbackOnce(string title , string unit , Func<object> callback)
        {
            return new CallbackMonitorHandler(new OnceRecordMonitorHandler(title, unit), callback);
        }

        /// <summary>
        /// 单次尺寸记录监控
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="callback">回调获取值</param>
        /// <returns>监控句柄</returns>
        public static IMonitorHandler CallbackSize(string title, Func<object> callback)
        {
            return new CallbackMonitorHandler(new SizeMonitorHandler(title), callback);
        }
    }
}
