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

namespace CatLib.Debugger.WebMonitor
{
    /// <summary>
    /// 监控处理器
    /// </summary>
    public interface IMonitorHandler
    {
        /// <summary>
        /// 监控的名字(用于UI端显示)
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 监控值的单位描述
        /// </summary>
        string Unit { get; }

        /// <summary>
        /// 实时的监控值
        /// </summary>
        string Value { get; }

        /// <summary>
        /// 处理句柄
        /// </summary>
        /// <param name="value">值</param>
        void Handler(object value);
    }
}
