using CatLib.API.IO;

namespace CatLib.IO
{

    /// <summary>
    /// IO服务提供商
    /// </summary>
    public class IOProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<IO>().Alias<IIOFactory>();
            App.Bind<LocalDisk>().Alias<IDisk>();
        }

    }

}