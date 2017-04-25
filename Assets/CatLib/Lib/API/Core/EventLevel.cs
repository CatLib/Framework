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

namespace CatLib.API
{
    /// <summary>
    /// 事件等级
    /// </summary>
    [Flags]
    public enum EventLevel
    {
        /// <summary>
        /// 不通知
        /// </summary>
        None = 0,

        /// <summary>
        /// 自身对象级通知
        /// </summary>
        Self = 1 << 1,

        /// <summary>
        /// 类型级通知
        /// </summary>
        Type = 1 << 2,

        /// <summary>
        /// 接口级通知
        /// </summary>
        Interface = 1 << 3,

        /// <summary>
        /// 全局通知
        /// </summary>
        Global = 1 << 4,

        /// <summary>
        /// 全部通知
        /// </summary>
        All = int.MaxValue,
    }
}