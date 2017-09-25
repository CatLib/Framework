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

namespace CatLib.API.Config
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public interface IConfigManager : ISingleManager<IConfig>
    {
        /// <summary>
        /// 设定默认的配置名
        /// </summary>
        /// <param name="name">配置名</param>
        void SetDefault(string name);

        /// <summary>
        /// 对目标实例注入配置
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="name">使用的配置容器名字</param>
        void Config(object instance, string name = null);
    }
}
