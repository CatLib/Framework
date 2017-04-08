/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using CatLib.API.CsvStore;
using CatLib.API.Config;
using CatLib.API.IO;
using CatLib.API;
using System.Collections;

namespace CatLib.CsvStore{

	public class CsvStoreProvider : ServiceProvider{

		public override void Register()
        {

            App.Singleton<CsvStore>().Alias<ICsvStore>().Alias("csv.store").OnResolving((app , bind, obj)=>{

				CsvStore store = obj as CsvStore;

				IConfigStore confStore = app.Make<IConfigStore>();
				
				if(confStore != null){

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

				}

				return store;

			});;

        }

	}

}