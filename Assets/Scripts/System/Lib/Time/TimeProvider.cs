using CatLib.Contracts.Time;

namespace CatLib.Time
{

    public class TimeProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Bind<TimeQueue>().Alias<ITimeQueue>();
            App.Singleton<TimeSystem>().Alias<ITime>();
        }
    }

}