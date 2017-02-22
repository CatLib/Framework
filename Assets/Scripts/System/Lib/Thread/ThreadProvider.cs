using CatLib.API.Thread;

namespace CatLib.Thread
{

    public class ThreadProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<ThreadRuner>().Alias<IThread>();
        }
    }

}