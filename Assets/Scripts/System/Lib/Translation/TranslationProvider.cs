using System.Collections;
using CatLib.API;
using CatLib.API.INI;
using CatLib.API.IO;
using CatLib.API.Translator;

namespace CatLib.Translation{

public class TranslationProvider : ServiceProvider {

	public override void Register()
	{

		RegisterLoader();
		RegisterSelector();

		App.Singleton("translation" , (app , param) => {

			IFileLoader loader = app.Make("translation.loader") as IFileLoader;
			ISelector selector = app.Make("translation.selector") as ISelector;
			
			Translator tran = App.Make(typeof(Translator).ToString()) as Translator;
			tran.SetFileLoader(loader);
			tran.SetLocale("zh");
			tran.SetSelector(selector);

			return tran;

		}).Alias<ITranslator>();
	}

	protected void RegisterSelector(){

		App.Singleton("translation.selector", (app , param) => {

            return new MessageSelector();

        });

	}

	protected void RegisterLoader()
    {
        App.Singleton("translation.loader", (app , param) => {

			IEnv env = app.Make<IEnv>();

			IIOFactory factory = app.Make<IIOFactory>();
			IDisk disk = factory.Disk();

			#if UNITY_EDITOR
			if(env.DebugLevel == DebugLevels.Auto || env.DebugLevel == DebugLevels.Dev){

				disk.SetConfig(new Hashtable(){

					{"root" , env.AssetPath + env.ResourcesNoBuildPath}
					
				});

			}
			#endif

            return new FileLoader(disk, app.Make<IINILoader>());

        });
    }

}

}