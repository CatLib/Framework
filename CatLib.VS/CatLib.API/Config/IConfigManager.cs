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

using CatLib.API.Stl;

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
    }
}
