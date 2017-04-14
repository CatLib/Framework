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
    /// 全局事件
    /// </summary>
    public interface IGlobalEvent
    {
        /// <summary>
        /// 增加事件响应接口
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns>全局事件实例</returns>
        IGlobalEvent AppendInterface<T>();

        /// <summary>
        /// 增加事件响应接口
        /// </summary>
        /// <param name="t">接口类型</param>
        /// <returns>全局事件实例</returns>
        IGlobalEvent AppendInterface(Type t);

        /// <summary>
        /// 设定事件等级
        /// </summary>
        /// <param name="level">事件等级</param>
        /// <returns>全局事件实例</returns>
        IGlobalEvent SetEventLevel(EventLevel level);

        /// <summary>
        /// 触发一个全局事件
        /// </summary>
        /// <param name="args">事件参数</param>
        void Trigger(EventArgs args = null);
    }
}