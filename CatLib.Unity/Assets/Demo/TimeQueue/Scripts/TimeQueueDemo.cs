using CatLib.API;
using CatLib.API.TimeQueue;
using UnityEngine;

namespace CatLib.Demo.TimeQueue
{

    public class TimeQueueDemo : ServiceProvider
    {

        public override void Init()
        {
            App.On(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
            {

                ITimeQueue timeQueue = App.Make<ITimeQueue>();

                Debug.Log("frame count: " + UnityEngine.Time.frameCount);

                ITimeTaskHandler handler = timeQueue.Task(() =>
                {
                    Debug.Log("time queue task:" + UnityEngine.Time.frameCount);
                }).LoopFrame(3).OnComplete(()=>
                {
                    Debug.Log("task1 is complete");
                }).Push();

                timeQueue.Task(() =>
                {
                    Debug.Log("hello world");
                }).Delay(3).Loop(3).Delay(3).Loop(()=> { return Random.Range(0, 100) < 90; }).OnComplete(() => { Debug.Log("task 2 on complete"); }).Push();

                timeQueue.Play();

            });
        }

        public override void Register()
        {

        }

    }
}
