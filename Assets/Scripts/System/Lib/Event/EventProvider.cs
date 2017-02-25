using CatLib.API.Event;

namespace CatLib.Event
{

    /// <summary>
    /// 事件服务
    /// </summary>
    public class EventProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Bind<EventStore>().Alias<IEvent>().Alias<IEventAchieve>();
        }

    }

}