
using CatLib.API.Time;

namespace CatLib.Time
{

    /// <summary>
    /// 时间系统
    /// </summary>
    public class TimeSystem : Component, ITime
    {

        public virtual float Time
        {
            get
            {
                return UnityEngine.Time.time;
            }
        }

        public virtual float DeltaTime
        {
            get
            {
                return UnityEngine.Time.deltaTime;
            }
        }


        public virtual float FixedTime { get { return UnityEngine.Time.fixedTime; } }

        public virtual float TimeSinceLevelLoad { get { return UnityEngine.Time.timeSinceLevelLoad; } }

        public virtual float FixedDeltaTime { get { return UnityEngine.Time.fixedDeltaTime; } }

        public virtual float MaximumDeltaTime { get { return UnityEngine.Time.maximumDeltaTime; } }

        public virtual float SmoothDeltaTime { get { return UnityEngine.Time.smoothDeltaTime; } }

        public virtual float TimeScale { get { return UnityEngine.Time.timeScale; } }

        public virtual float FrameCount { get { return UnityEngine.Time.frameCount; } }

        public virtual float RealtimeSinceStartup { get { return UnityEngine.Time.realtimeSinceStartup; } }

        public virtual float CaptureFramerate { get { return UnityEngine.Time.captureFramerate; } }

        public virtual float UnscaledDeltaTime { get { return UnityEngine.Time.unscaledDeltaTime; } }

        public virtual float UnscaledTime { get { return UnityEngine.Time.unscaledTime; } }


    }

}