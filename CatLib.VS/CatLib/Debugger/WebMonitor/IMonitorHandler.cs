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
    /// 监控句柄
    /// </summary>
    public interface IMonitorHandler
    {
        /// <summary>
        /// 监控的名字
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 标签(第0位：分类)
        /// </summary>
        string[] Tags { get; }

        /// <summary>
        /// 监控值的单位
        /// </summary>
        string Unit { get; }

        /// <summary>
        /// 监控值
        /// </summary>
        string Value { get; }
    }
}
