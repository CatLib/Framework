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

namespace CatLib.API.Timer
{
    /// <summary>
    /// 时间任务组
    /// </summary>
    public interface ITimerGroup
    {
        /// <summary>
        /// 当组的所有计时器完成时
        /// </summary>
        /// <param name="onComplete">完成时</param>
        /// <returns>当前组实例</returns>
        ITimerGroup OnComplete(Action onComplete);
    }
}
