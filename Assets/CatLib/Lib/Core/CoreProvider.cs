using System;
using CatLib.API;
using CatLib.API.Config;

namespace CatLib
{

    public class CoreProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<Env>().Alias<IEnv>().Resolving((app, bind, obj)=>{

                IConfigStore config = app.Make<IConfigStore>();
                Env env = obj as Env;

                Type t = typeof(Env);

                env.SetDebugLevel(config.Get<DebugLevels>(t, "debug" , DebugLevels.Auto));
                env.SetAssetPath(config.Get(t , "asset.path" , null));   
                env.SetReleasePath(config.Get(t , "release.path" , null));
                env.SetResourcesBuildPath(config.Get(t , "build.asset.path" , null));
                env.SetResourcesNoBuildPath(config.Get(t , "nobuild.asset.path" , null));

                return obj;
                  
            });
        }
    }

}