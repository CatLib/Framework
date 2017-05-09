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

namespace CatLib.CsvStore
{
    /// <summary>
    /// Csv容器服务提供商
    /// </summary>
    public class CsvStoreProvider : ServiceProvider
    {
        /// <summary>
        /// 注册一个Csv容器服务提供商
        /// </summary>
        public override void Register()
        {
            App.Singleton<CsvStore>().Alias<ICsvStore>().Alias("csv.store").OnResolving((bind, obj) =>
            {
                var store = obj as CsvStore;
                var confStore = App.Make<IConfigStore>();

                if (confStore == null)
                {
                    return store;
                }

                var root = confStore.Get(typeof(CsvStore), "root", null);

                if (root == null)
                {
                    return store;
                }

                var env = App.Make<IEnv>();
                var io = App.Make<IIOFactory>();
                var disk = io.Disk();

                disk.SetConfig(new Hashtable { { "root", env.AssetPath } });
                store.SetDirctory(disk.Directory(root));
                return store;
            });
        }
    }
}