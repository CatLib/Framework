﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

namespace CatLib.TimeQueue
{
    /// <summary>
    /// 时间任务类型
    /// </summary>
    internal enum TimeTaskActionTypes
    {
        /// <summary>
        /// 延迟时间执行
        /// </summary>
        DelayTime,

        /// <summary>
        /// 延迟帧数执行
        /// </summary>
        DelayFrame,

        /// <summary>
        /// 循环执行指定时间
        /// </summary>
        LoopTime,

        /// <summary>
        /// 循环执行直到函数返回false
        /// </summary>
        LoopFunc,

        /// <summary>
        /// 循环执行指定帧数
        /// </summary>
        LoopFrame,
    }
}
