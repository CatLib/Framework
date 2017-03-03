using CatLib.API;

namespace CatLib
{

    public class CoreProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<Env>().Alias<IEnv>();
        }
    }

}