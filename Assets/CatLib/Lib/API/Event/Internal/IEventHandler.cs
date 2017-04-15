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

namespace CatLib.API.Event
{
    /// <summary>
    /// 事件句柄
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// 取消注册的事件
        /// </summary>
        bool Cancel();

        /// <summary>
        /// 剩余的调用次数，当为0时事件会被释放
        /// </summary>
        int Life { get; }

        /// <summary>
        /// 事件是否是有效的
        /// </summary>
        bool IsLife { get; }
    }
}