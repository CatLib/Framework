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
    /// 累加监控处理器
    /// </summary>
    public sealed class SumMonitorHandler<TType> : IMonitorHandler where TType : struct
    {
        /// <summary>
        /// 累加值
        /// </summary>
        private TType value;

        /// <summary>
        /// 单位描述
        /// </summary>
        private string unit;

        /// <summary>
        /// 累加监控处理器
        /// </summary>
        /// <param name="unit">单位描述</param>
        public SumMonitorHandler(string unit)
        {
            Guard.Requires<ArgumentNullException>(unit != null);
            this.unit = unit;
        }

        /// <summary>
        /// 处理句柄
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>返回数据将会被推送至显示端</returns>
        public string Handler(object value)
        {
            this.value = (TType)value;
            return string.Empty;
        }
    }
}
