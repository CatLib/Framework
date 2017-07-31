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

#if CATLIB
using CatLib.API.Config;
using CatLib.API.Converters;
using CatLib.Config.Locator;

namespace CatLib.Config
{
    /// <summary>
    /// 配置服务提供者
    /// </summary>
    public sealed class ConfigProvider : IServiceProvider
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 注册配置服务
        /// </summary>
        public void Register()
        {
            RegisterManager();
            RegisterDefaultConfig();
        }

        /// <summary>
        /// 注册管理器
        /// </summary>
        private void RegisterManager()
        {
            App.Singleton<ConfigManager>().Alias<IConfigManager>().OnResolving((bind, obj) =>
            {
                var configManager = obj as ConfigManager;

                configManager.Extend(() =>
                {
                    var config = new Config(App.Make<IConverters>() , new CodeConfigLocator());
                    return config;
                });

                return configManager;
            });
        }

        /// <summary>
        /// 注册默认的配置
        /// </summary>
        private void RegisterDefaultConfig()
        {
            App.Bind<IConfig>((container, @params) =>
            {
                return App.Make<IConfigManager>().Default;
            });
        }
    }
}
#endif