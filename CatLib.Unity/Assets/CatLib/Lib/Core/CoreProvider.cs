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

using CatLib.API;
using CatLib.API.Config;

namespace CatLib
{
    /// <summary>
    /// 核心服务提供商
    /// </summary>
    public sealed class CoreProvider : ServiceProvider
    {
        /// <summary>
        /// 注册核心服务提供商
        /// </summary>
        public override void Register()
        {
            App.Singleton<Env>().Alias<IEnv>().OnResolving((bind, obj) =>
            {
                var env = obj as Env;
                if (env == null)
                {
                    return null;
                }

                var configManager = App.Make<IConfigManager>();
                if (configManager == null)
                {
                    return obj;
                }

                env.SetDebugLevel(configManager.Get().Get("env.debug", DebugLevels.Auto));

                return obj;
            });
        }
    }
}