using CatLib.Contracts.Event;

namespace CatLib.Event
{

    /// <summary>
    /// 事件调度服务
    /// </summary>
    public class DispatcherProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Bind<EventStore>().Alias<IEvent>().Alias<IEventAchieve>();
        }

    }

}