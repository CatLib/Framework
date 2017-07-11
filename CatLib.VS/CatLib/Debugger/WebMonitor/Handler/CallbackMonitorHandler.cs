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
using CatLib.Stl;

namespace CatLib.Debugger.WebMonitor.Handler
{
    /// <summary>
    /// 回调获取监控值处理器
    /// </summary>
    public sealed class CallbackMonitorHandler : IMonitorHandler
    {
        /// <summary>
        /// 监控的标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 监控值的单位描述
        /// </summary>
        public string Unit { get; private set; }

        /// <summary>
        /// 实时的监控值
        /// </summary>
        public string Value
        {
            get { return callback.Invoke().ToString(); }
        }

        /// <summary>
        /// 回调
        /// </summary>
        private Func<object> callback;

        /// <summary>
        /// 单次记录监控处理器
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="unit">单位值</param>
        /// <param name="callback">回调获取监控值</param>
        public CallbackMonitorHandler(string title, string unit, Func<object> callback)
        {
            Guard.Requires<ArgumentNullException>(callback != null);
            Title = title;
            Unit = unit;
            this.callback = callback;
        }

        /// <summary>
        /// 处理句柄
        /// </summary>
        /// <param name="value">值</param>
        public void Handler(object value)
        {
        }
    }
}
