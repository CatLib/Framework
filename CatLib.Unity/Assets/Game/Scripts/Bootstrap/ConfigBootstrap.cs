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

namespace CatLib.Bootstrap
{
    /// <summary>
    /// 初始配置引导
    /// </summary>
    public class ConfigBootstrap : IBootstrap
    {
        /// <summary>
        /// 引导程序
        /// </summary>
        public void Bootstrap()
        {
            if (Configs.ConfigsMap == null)
            {
                return;
            }

            var config = App.Make<IConfigManager>();
            if (config == null)
            {
                return;
            }

            foreach (var kvp in Configs.ConfigsMap)
            {
                config.Default.Set(kvp.Key, kvp.Value);
            }
        }
    }
}