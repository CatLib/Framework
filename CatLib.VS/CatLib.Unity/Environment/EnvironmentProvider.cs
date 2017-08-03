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
    /// 环境服务提供者
    /// </summary>
    public sealed class EnvironmentProvider : IServiceProvider
    {
        /// <summary>
        /// 调试等级
        /// </summary>
        public DebugLevels DebugLevel = DebugLevels.Auto;

        /// <summary>
        /// 资源路径
        /// </summary>
        public string AssetPath = string.Empty;

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
                var env = (UnityEnvironment)obj;
                var config = App.Make<IConfig>();

                env.SetDebugLevel(config.SafeGet("EnvironmentProvider.DebugLevel", DebugLevel));
                env.SetAssetPath(config.SafeGet("EnvironmentProvider.AssetPath", AssetPath));

                return obj;
            });
        }
    }
}
#endif