using CatLib.Contracts.IO;
using CatLib.IO;

namespace CatLib.ResourcesSystem
{

    /// <summary>
    /// IO服务提供商
    /// </summary>
    public class IOProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<Directory>().Alias<IDirectory>();
            App.Singleton<File>().Alias<IFile>();
        }

    }

}