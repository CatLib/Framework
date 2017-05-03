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

namespace CatLib.TimeQueue
{
    /// <summary>
    /// 时间任务行为
    /// </summary>
    internal sealed class TimeTaskAction
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public TimeTaskActionTypes Type { get; set; }

        /// <summary>
        /// 整型参数
        /// </summary>
        public int[] IntArgs { get; set; }

        /// <summary>
        /// 浮点型参数
        /// </summary>
        public float[] FloatArgs { get; set; }

        /// <summary>
        /// 布尔回调函数
        /// </summary>
        public Func<bool> FuncBoolArg { get; set; }
    }
}