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
        private int elapseSeconds;

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
        public HeartBeatState(int interval)
        {
            this.interval = interval;
            Reset();
        }

        /// <summary>
        /// 设定心跳包间隔
        /// </summary>
        /// <param name="interval">间隔</param>
        public void SetInterval(int interval)
        {
            this.interval = interval;
        }

        /// <summary>
        /// 触发心跳流逝
        /// </summary>
        /// <param name="elapseSeconds">流逝时间</param>
        public void Tick(int elapseSeconds)
        {
            if (interval <= 0)
            {
                return;
            }

            this.elapseSeconds += Math.Max(elapseSeconds, 0);

            if (!(this.elapseSeconds >= interval))
            {
                return;
            }

            missCount++;
            this.elapseSeconds = 0;
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
            elapseSeconds = 0;
            missCount = 0;
        }
    }
}
