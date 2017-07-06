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

using CatLib.API.Debugger;

namespace CatLib.Debugger.MonitorHandler
{
    /// <summary>
    /// 单次监控处理器
    /// </summary>
    public sealed class OnceRecordMonitorHandler : IMonitorHandler
    {
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
        /// <param name="unit"></param>
        public OnceRecordMonitorHandler(string unit)
        {
            Unit = unit;
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
