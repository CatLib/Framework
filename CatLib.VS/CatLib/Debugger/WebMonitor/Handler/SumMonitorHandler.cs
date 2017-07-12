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
    /// 累加监控处理器
    /// </summary>
    public sealed class SumMonitorHandler : IMonitorHandler
    {
        /// <summary>
        /// 监控的名字
        /// </summary>
        public string Title
        {
            get
            {
                return baseHandler.Title;
            }
        }

        /// <summary>
        /// 监控值的单位描述
        /// </summary>
        public string Unit
        {
            get
            {
                return baseHandler.Unit;
            }
        }

        /// <summary>
        /// 实时的监控值
        /// </summary>
        public string Value
        {
            get
            {
                return baseHandler.Value;
            }
        }

        /// <summary>
        /// 基础处理器
        /// </summary>
        private readonly IMonitorHandler baseHandler;

        /// <summary>
        /// 监控的值
        /// </summary>
        private long value;

        /// <summary>
        /// 累加监控处理器
        /// </summary>
        /// <param name="baseHandler">基础处理器</param>
        public SumMonitorHandler(IMonitorHandler baseHandler)
        {
            Guard.Requires<ArgumentNullException>(baseHandler != null);
            this.baseHandler = baseHandler;
        }

        /// <summary>
        /// 处理句柄
        /// </summary>
        /// <param name="value">值</param>
        public void Handler(object value)
        {
            this.value += (long)value;
            baseHandler.Handler(this.value);
        }
    }
}
