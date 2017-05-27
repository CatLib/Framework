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
using CatLib.Stl;

namespace CatLib.Config
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public sealed class ConfigManager : Manager<IConfig> , IConfigManager
    {
        /// <summary>
        /// 默认的配置名字
        /// </summary>
        private string defaultName;

        /// <summary>
        /// 设定默认的配置名字
        /// </summary>
        /// <param name="name">默认配置名字</param>
        public void SetDefault(string name)
        {
            if (name == string.Empty)
            {
                name = null;
            }

            defaultName = name;
        }

        /// <summary>
        /// 获取默认的配置名字
        /// </summary>
        /// <returns>默认的文件系统名字</returns>
        protected override string GetDefaultName()
        {
            return defaultName ?? "default";
        }
    }
}
