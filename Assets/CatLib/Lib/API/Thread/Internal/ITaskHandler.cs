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

namespace CatLib.API.Thread
{
    /// <summary>
    /// 线程任务句柄
    /// </summary>
    public interface ITaskHandler
    {
        /// <summary>
        /// 撤销线程执行，只有在delay状态才能撤销
        /// </summary>
        void Cancel();
    }
}