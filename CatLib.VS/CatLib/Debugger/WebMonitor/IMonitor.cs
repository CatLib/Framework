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

namespace CatLib.Debugger
{
    /// <summary>
    /// 监控
    /// </summary>
    public interface IMonitor
    {
        /// <summary>
        /// 增加监控
        /// </summary>
        /// <param name="handler">监控句柄</param>
        void Monitor(IMonitorHandler handler);
    }
}
