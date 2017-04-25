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

namespace CatLib.API.Flux
{
    /// <summary>
    /// Flux调度器
    /// </summary>
    public interface IFluxDispatcher
    {
        /// <summary>
        /// 是否处于调度中
        /// </summary>
        /// <returns>是否处于调度中</returns>
        bool IsDispatching { get; }

        /// <summary>
        /// 注册一个匿名调度事件
        /// </summary>
        /// <param name="action">响应调度事件的回调</param>
        string On(Action<IAction> action);

        /// <summary>
        /// 注册一个调度事件
        /// </summary>
        /// <param name="token">标识符</param>
        /// <param name="action">响应调度事件的回调</param>
        void On(string token, Action<IAction> action);

        /// <summary>
        /// 解除调度事件
        /// </summary>
        /// <param name="token">标识符</param>
        void Off(string token);

        /// <summary>
        /// 等待调度器完成另外的调度
        /// </summary>
        /// <param name="token">标识符</param>
        /// <param name="action">行为</param>
        void WaitFor(string token, IAction action);

        /// <summary>
        /// 将行为调度到指定标识符的Store中
        /// </summary>
        /// <param name="token">标识符</param>
        /// <param name="action">行为</param>
        void Dispatch(string token, IAction action);

        /// <summary>
        /// 调度行为
        /// </summary>
        /// <param name="action">行为</param>
        void Dispatch(IAction action);
    }
}