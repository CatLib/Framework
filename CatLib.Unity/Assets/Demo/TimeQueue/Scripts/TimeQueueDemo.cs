using CatLib.API;
using CatLib.API.Timer;
using UnityEngine;

namespace CatLib.Demo.TimeQueue
{

    public class TimeQueueDemo : ServiceProvider
    {

        public override void Init()
        {
            App.On(ApplicationEvents.OnApplicationStartComplete, (sender, e) =>
            {
                var timerManager = App.Make<ITimerManager>();

                Debug.Log("frame count: " + UnityEngine.Time.frameCount);

            });
        }

        public override void Register()
        {

        }

    }
}
