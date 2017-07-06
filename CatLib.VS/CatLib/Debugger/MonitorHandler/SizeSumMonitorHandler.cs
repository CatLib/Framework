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

using System.Collections.Generic;
using CatLib.API.Debugger;

namespace CatLib.Debugger.MonitorHandler
{
    /// <summary>
    /// 大小累加监控处理器
    /// </summary>
    public sealed class SizeSumMonitorHandler : IMonitorHandler
    {
        /// <summary>
        /// 单位映射
        /// </summary>
        private readonly Dictionary<long, string> unitMapping;

        /// <summary>
        /// 监控值的单位描述
        /// </summary>
        public string Unit
        {
            get
            {
                foreach (var unit in unitMapping)
                {
                    if (value < unit.Key)
                    {
                        return unit.Value;
                    }
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 监控的值
        /// </summary>
        private long value;

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
        public SizeSumMonitorHandler()
        {
            unitMapping = new Dictionary<long, string>()
            {
                { 1024 , "B"},
                { 1048576 , "KB" },
                { 1073741824 ,"MB" },
                { 1099511627776 , "GB" },
                { 1125899906842624 , "TB" },
                { long.MaxValue , "PB" }
            };
        }

        /// <summary>
        /// 处理句柄
        /// </summary>
        /// <param name="value">值</param>
        public void Handler(object value)
        {
            this.value += (long)value;
        }
    }
}
