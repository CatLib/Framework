using CatLib.Contracts.Time;

namespace CatLib.Time
{

    public class TimeProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<TimeSystem>().Alias<ITime>();
        }
    }

}