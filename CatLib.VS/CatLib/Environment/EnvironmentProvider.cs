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
using CatLib.API.Environment;

namespace CatLib.Environment
{
    /// <summary>
    /// 核心服务提供者
    /// </summary>
    public sealed class EnvironmentProvider : IServiceProvider
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 注册核心服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<UnityEnvironment>().Alias<IEnvironment>().OnResolving((bind, obj) =>
            {
                var env = obj as UnityEnvironment;

                var configManager = App.Make<IConfigManager>();
                if (configManager == null)
                {
                    return obj;
                }

                env.SetDebugLevel(configManager.Get().Get("env.debug", DebugLevels.Auto));
                env.SetAssetPath(configManager.Get().Get("env.asset.path", string.Empty));

                return obj;
            }).Alias("catlib.environment.unity");
        }
    }
}
#endif