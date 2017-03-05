using CatLib.API.TimeQueue;

namespace CatLib.TimeQueue
{

    public class TimeQueueProvider : ServiceProvider
    {
        public override void Register()
        {
            App.Bind<TimeQueue>((app , param)=>{
                
                return app.Make<TimeRunner>().CreateQueue();

            }).Alias<ITimeQueue>();
            App.Singleton<TimeRunner>();
        }
    }

}