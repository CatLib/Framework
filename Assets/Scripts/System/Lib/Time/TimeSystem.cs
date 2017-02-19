
using CatLib.Contracts.Time;

namespace CatLib.Time
{

    /// <summary>
    /// 时间系统
    /// </summary>
    public class TimeSystem : ITime
    {

        public float Time { get { return UnityEngine.Time.time; } }

    }

}