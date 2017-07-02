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

namespace CatLib.API.Debugger
{
    /// <summary>
    /// 监控处理
    /// </summary>
    public interface IMonitor
    {
        /// <summary>
        /// 处理句柄
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>返回数据将会被推送至显示端</returns>
        string Handler(object value);
    }
}
