
using CatLib.API.CsvStore;
using CatLib.API.Config;
using CatLib.API.IO;
using CatLib.API;
using System.Collections;

namespace CatLib.CsvStore{

	public class CsvStoreProvider : ServiceProvider{

		public override void Register()
        {

            App.Singleton<CsvStore>().Alias<ICsvStore>().Alias("csv.store").Resolving((app , bind, obj)=>{

				CsvStore store = obj as CsvStore;

				IConfigStore confStore = app.Make<IConfigStore>();

				string root = confStore.Get(typeof(CsvStore), "root", null);

				if(root != null){
					
					IEnv env = app.Make<IEnv>();
					IIOFactory io = app.Make<IIOFactory>();
					IDisk disk = io.Disk();

					#if UNITY_EDITOR
					if(env.DebugLevel == DebugLevels.Auto || env.DebugLevel == DebugLevels.Dev){

						disk.SetConfig(new Hashtable(){

							{"root" , env.AssetPath + env.ResourcesNoBuildPath}
							
						});

					}
					#endif


					store.SetDirctory(disk.Directory(root));
				
				}

				return store;

			});;

        }

	}

}