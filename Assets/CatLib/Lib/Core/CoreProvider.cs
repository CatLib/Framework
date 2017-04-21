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
using CatLib.API;
using CatLib.API.Config;

namespace CatLib
{
    /// <summary>
    /// 核心服务提供商
    /// </summary>
    public class CoreProvider : ServiceProvider
    {
        /// <summary>
        /// 注册核心服务提供商
        /// </summary>
        public override void Register()
        {
            App.Singleton<Env>().Alias<IEnv>().OnResolving((bind, obj) =>
            {
                var config = App.Make<IConfigStore>();

                if (config == null)
                {
                    return obj;
                }

                var env = obj as Env;
                var t = typeof(Env);

                env.SetDebugLevel(config.Get(t, "debug", DebugLevels.Auto));
                env.SetAssetPath(config.Get(t, "asset.path", null));
                env.SetReleasePath(config.Get(t, "release.path", null));
                env.SetResourcesBuildPath(config.Get(t, "build.asset.path", null));
                env.SetResourcesNoBuildPath(config.Get(t, "nobuild.asset.path", null));

                return obj;
            });
        }
    }
}