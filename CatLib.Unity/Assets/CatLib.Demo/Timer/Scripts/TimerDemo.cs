using System.Collections;
using CatLib.API;
using CatLib.API.Timer;
using UnityEngine;

namespace CatLib.Demo.Timer
{
    /// <summary>
    /// 计时器Demo
    /// </summary>
    public class TimerDemo : ServiceProvider
    {
        public override IEnumerator Init()
        {
            App.On(ApplicationEvents.OnStartComplete, (sender, e) =>
            {
                var timerManager = App.Make<ITimerManager>();

                Debug.Log("frame count: " + UnityEngine.Time.frameCount);

                var statu = 0;
                timerManager.Make(() =>
                {
                    Debug.Log("tick: " + (++statu) + " / " + UnityEngine.Time.frameCount);
                }).IntervalFrame(2);

                timerManager.Make(() =>
                {
                    Debug.Log("delay tick: " + (++statu) + " / " + UnityEngine.Time.frameCount);
                }).DelayFrame(1);

                timerManager.Make(() =>
                {
                    Debug.Log("loop frame tick: " + (++statu) + " / " + UnityEngine.Time.frameCount);
                }).LoopFrame(3);
            });

            yield return base.Init();
        }

        public override void Register()
        {

        }
    }
}
