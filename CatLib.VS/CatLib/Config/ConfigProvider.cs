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
            App.Singleton<Config>().Alias<IConfig>().Alias("config").OnResolving((bind, obj) =>
            {
                var store = obj as Config;

                return store;
            });
        }
    }
}