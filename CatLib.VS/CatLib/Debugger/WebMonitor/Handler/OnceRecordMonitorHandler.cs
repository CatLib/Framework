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
    /// 单次监控处理器
    /// </summary>
    public sealed class OnceRecordMonitorHandler : IMonitorHandler
    {
        /// <summary>
        /// 分类
        /// </summary>
        public IList<string> Category { get; private set; }

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
        public string Value { get; private set; }

        /// <summary>
        /// 单次记录监控处理器
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="unit">单位值</param>
        /// <param name="category">分类</param>
        public OnceRecordMonitorHandler(string title, string unit , IList<string> category = null)
        {
            Title = title;
            Unit = unit;
            Category = category ?? new string[]{};
        }

        /// <summary>
        /// 处理句柄
        /// </summary>
        /// <param name="value">值</param>
        public void Handler(object value)
        {
            Value = value.ToString();
        }
    }
}
