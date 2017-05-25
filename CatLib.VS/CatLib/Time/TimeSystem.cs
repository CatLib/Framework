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

using CatLib.API.Time;
using UTime = UnityEngine.Time;

namespace CatLib.Time
{
    /// <summary>
    /// 时间系统
    /// </summary>
    public class TimeSystem : ITime
    {
        /// <summary>
        /// 从游戏开始到现在所用的时间(秒)
        /// </summary>
        public virtual float Time
        {
            get { return UTime.time; }
        }

        /// <summary>
        /// 上一帧到当前帧的时间(秒)
        /// </summary>
        public virtual float DeltaTime
        {
            get { return UTime.deltaTime; }
        }

        /// <summary>
        /// 从游戏开始到现在的时间（秒）使用固定时间来更新
        /// </summary>
        public virtual float FixedTime
        {
            get { return UTime.fixedTime; }
        }

        /// <summary>
        /// 从当前scene开始到目前为止的时间（秒）
        /// </summary>
        public virtual float TimeSinceLevelLoad
        {
            get { return UTime.timeSinceLevelLoad; }
        }

        /// <summary>
        /// 固定的更新时间（秒）
        /// </summary>
        public virtual float FixedDeltaTime
        {
            get { return UTime.fixedDeltaTime; }
            set { UTime.fixedDeltaTime = value; }
        }

        /// <summary>
        /// 能获取的最大更新时间
        /// </summary>
        public virtual float MaximumDeltaTime
        {
            get { return UTime.maximumDeltaTime; }
        }

        /// <summary>
        /// 平稳的更新时间，根据前N帧的加权平均值
        /// </summary>
        public virtual float SmoothDeltaTime
        {
            get { return UTime.smoothDeltaTime; }
        }

        /// <summary>
        /// 时间缩放系数
        /// </summary>
        public virtual float TimeScale
        {
            get { return UTime.timeScale; }
            set { UTime.timeScale = value; }
        }

        /// <summary>
        /// 总帧数
        /// </summary>
        public virtual float FrameCount
        {
            get { return UTime.frameCount; }
        }

        /// <summary>
        /// 自游戏开始后的总时间（暂停也会增加）
        /// </summary>
        public virtual float RealtimeSinceStartup
        {
            get { return UTime.realtimeSinceStartup; }
        }

        /// <summary>
        /// 每秒的帧率
        /// </summary>
        public virtual int CaptureFramerate
        {
            get { return UTime.captureFramerate; }
            set { UTime.captureFramerate = value; }
        }

        /// <summary>
        /// 不考虑时间缩放的更新时间
        /// </summary>
        public virtual float UnscaledDeltaTime
        {
            get { return UTime.unscaledDeltaTime; }
        }

        /// <summary>
        /// 不考虑时间缩放的从游戏开始到现在的时间
        /// </summary>
        public virtual float UnscaledTime
        {
            get { return UTime.unscaledTime; }
        }
    }
}