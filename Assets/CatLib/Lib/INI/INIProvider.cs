using CatLib.API.INI;

namespace CatLib.INI
{

    /// <summary>
    /// INI服务提供商
    /// </summary>
    public class INIProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<INILoader>().Alias<IINILoader>();
        }

    }

}