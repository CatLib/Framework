
using CatLib.Contracts.Time;

namespace CatLib.Time
{

    /// <summary>
    /// 时间系统
    /// </summary>
    public class TimeSystem : TimeRunner, ITime
    {

        public float Time
        {
            get
            {
                return UnityEngine.Time.time;
            }
        }

        public float DeltaTime
        {
            get
            {
                return UnityEngine.Time.deltaTime;
            }
        }


        public float FixedTime { get { return UnityEngine.Time.fixedTime; } }

        public float TimeSinceLevelLoad { get { return UnityEngine.Time.timeSinceLevelLoad; } }

        public float FixedDeltaTime { get { return UnityEngine.Time.fixedDeltaTime; } }

        public float MaximumDeltaTime { get { return UnityEngine.Time.maximumDeltaTime; } }

        public float SmoothDeltaTime { get { return UnityEngine.Time.smoothDeltaTime; } }

        public float TimeScale { get { return UnityEngine.Time.timeScale; } }

        public float FrameCount { get { return UnityEngine.Time.frameCount; } }

        public float RealtimeSinceStartup { get { return UnityEngine.Time.realtimeSinceStartup; } }

        public float CaptureFramerate { get { return UnityEngine.Time.captureFramerate; } }

        public float UnscaledDeltaTime { get { return UnityEngine.Time.unscaledDeltaTime; } }

        public float UnscaledTime { get { return UnityEngine.Time.unscaledTime; } }


    }

}