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
using System;
using CatLib.API.Config;

namespace CatLib.Config{

	public class ConfigProvider : ServiceProvider {

		public override void Register()
		{

			App.Singleton<ConfigStore>().Alias<IConfigStore>().Alias("config").Resolving((app,bind,obj)=> {

                ConfigStore store = obj as ConfigStore;

                Type[] types = typeof(IConfig).GetChildTypesWithInterface();
                IConfig conf;
                for (int i = 0; i < types.Length; i++)
                {
                    conf = App.Make(types[i].ToString(), null) as IConfig;
                    store.AddConfig(conf);
                }

                return store;

            });

		}

	}

}