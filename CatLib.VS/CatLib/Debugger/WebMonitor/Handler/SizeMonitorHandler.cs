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

namespace CatLib.Debugger.WebMonitor.Handler
{
    /// <summary>
    /// 基于尺寸大小的监控处理器(字节)
    /// </summary>
    public sealed class SizeMonitorHandler : IMonitorHandler
    {
        /// <summary>
        /// 分类
        /// </summary>
        public IList<string> Category { get; private set; }

        /// <summary>
        /// 监控的名字
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 单位映射
        /// </summary>
        private readonly Dictionary<long, string> unitMapping;

        /// <summary>
        /// 监控值的单位描述
        /// </summary>
        public string Unit { get; private set; }

        /// <summary>
        /// 监控的值
        /// </summary>
        private double value;

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
        /// <param name="title">监控名字</param>
        /// <param name="category">分组</param>
        public SizeMonitorHandler(string title, string[] category = null)
        {
            Title = title;
            unitMapping = new Dictionary<long, string>
            {
                { 1024 , "B"},
                { 1048576 , "KB" },
                { 1073741824 ,"MB" },
                { 1099511627776 , "GB" },
                { 1125899906842624 , "TB" },
                { long.MaxValue , "PB" }
            };
            Category = category ?? new string[] { };
        }

        /// <summary>
        /// 处理句柄
        /// </summary>
        /// <param name="value">值(字节)</param>
        public void Handler(object value)
        {
            var longValue = (long) value;
            foreach (var unit in unitMapping)
            {
                if ((long)value < unit.Key)
                {
                    continue;
                }
                this.value = (longValue / (double)unit.Key);
                Unit = unit.Value;
            }
        }
    }
}
