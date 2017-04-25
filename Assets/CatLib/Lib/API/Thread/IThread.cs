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
    /// 多线程运行器
    /// </summary>
    public interface IThread
    {
        /// <summary>
        /// 新建一个多线程任务
        /// </summary>
        /// <param name="task">任务内容</param>
        /// <returns>任务</returns>
        ITask Task(System.Action task);

        /// <summary>
        /// 新建一个多线程任务允许产生回调
        /// </summary>
        /// <param name="task">任务内容</param>
        /// <returns>任务</returns>
        ITask Task(System.Func<object> task);
    }
}