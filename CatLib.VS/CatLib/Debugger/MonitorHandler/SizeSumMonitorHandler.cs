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
using CatLib.API.Debugger;
using CatLib.Stl;

namespace CatLib.Debugger.MonitorHandler
{
    /// <summary>
    /// 大小累加监控处理器
    /// </summary>
    public sealed class SizeSumMonitorHandler : IMonitorHandler
    {
        /// <summary>
        /// 监控值的单位描述
        /// </summary>
        public string Unit { get; private set; }

        /// <summary>
        /// 监控的值
        /// </summary>
        private int value;

        /// <summary>
        /// 实时的监控值
        /// </summary>
        public string Value
        {
            get
            {
                return value.ToString("#0.00");
            }
        }

        /// <summary>
        /// 累加监控处理器
        /// </summary>
        /// <param name="unit">单位描述</param>
        public SizeSumMonitorHandler(string unit)
        {
            Guard.Requires<ArgumentNullException>(unit != null);
            Unit = unit;
        }

        /// <summary>
        /// 处理句柄
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>返回数据将会被推送至显示端</returns>
        public void Handler(object value)
        {
            this.value = (int)value;
        }
    }
}
