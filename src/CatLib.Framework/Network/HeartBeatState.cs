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

namespace CatLib.Network
{
    /// <summary>
    /// 心跳包状态
    /// </summary>
    internal sealed class HeartBeatState
    {
        /// <summary>
        /// 流逝时间
        /// </summary>
        private int elapseMillisecond;

        /// <summary>
        /// 触发间隔
        /// </summary>
        private int interval;

        /// <summary>
        /// 心跳丢失次数
        /// </summary>
        private int missCount;

        /// <summary>
        /// 当丢失心跳包
        /// </summary>
        public event Action<int> OnMissHeartBeat;

        /// <summary>
        /// 心跳包状态
        /// </summary>
        /// <param name="intervalMillisecond">毫秒</param>
        public HeartBeatState(int intervalMillisecond)
        {
            interval = intervalMillisecond;
            Reset();
        }

        /// <summary>
        /// 设定心跳包间隔
        /// </summary>
        /// <param name="intervalMillisecond">间隔毫秒</param>
        public void SetInterval(int intervalMillisecond)
        {
            interval = intervalMillisecond;
        }

        /// <summary>
        /// 触发心跳流逝
        /// </summary>
        /// <param name="elapseMillisecond">流逝时间</param>
        public void Tick(int elapseMillisecond)
        {
            if (interval <= 0)
            {
                return;
            }

            this.elapseMillisecond += Math.Max(elapseMillisecond, 0);

            if (!(this.elapseMillisecond >= interval))
            {
                return;
            }

            missCount++;
            this.elapseMillisecond = 0;
            if (OnMissHeartBeat != null)
            {
                OnMissHeartBeat.Invoke(missCount);
            }
        }

        /// <summary>
        /// 重置心跳包状态
        /// </summary>
        public void Reset()
        {
            elapseMillisecond = 0;
            missCount = 0;
        }
    }
}
