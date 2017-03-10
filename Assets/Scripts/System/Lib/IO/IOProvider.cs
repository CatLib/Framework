using System.Collections;
using CatLib.API.Config;
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
            App.Singleton<IO>().Alias<IIOFactory>().Resolving((app , bind, obj)=>{

                IConfigStore config = app.Make<IConfigStore>();
                IO io = obj as IO;

                io.SetQuery((name) => config.Get<Hashtable>(typeof(IO) , name , null));

                return obj;

            });
            App.Bind<LocalDisk>().Alias<IDisk>();
        }

    }

}