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
using CatLib.API.Config;

namespace CatLib.Config
{
    /// <summary>
    /// 配置服务提供商
    /// </summary>
    public sealed class ConfigProvider : ServiceProvider
    {
        /// <summary>
        /// 注册配置服务
        /// </summary>
        public override void Register()
        {
            App.Singleton<ConfigStore>().Alias<IConfigStore>().Alias("config").OnResolving((obj) =>
            {
                var store = obj as ConfigStore;
                if (store == null)
                {
                    return null;
                }

                var types = typeof(IConfig).GetChildTypesWithInterface();

                IConfig conf;
                for (var i = 0; i < types.Length; i++)
                {
                    conf = App.Make(types[i].ToString(), null) as IConfig;
                    if (conf != null)
                    {
                        store.AddConfig(conf);
                    }
                }

                return store;
            });
        }
    }
}