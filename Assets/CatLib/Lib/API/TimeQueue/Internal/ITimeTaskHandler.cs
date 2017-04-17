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

namespace CatLib.API.TimeQueue
{
    /// <summary>
    /// 时间任务处理句柄
    /// </summary>
    public interface ITimeTaskHandler
    {
        /// <summary>
        /// 撤销任务执行
        /// </summary>
        void Cancel();
    }
}