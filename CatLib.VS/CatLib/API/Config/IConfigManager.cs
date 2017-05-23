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

namespace CatLib.API.Config
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// 获取配置方案
        /// </summary>
        /// <param name="name">配置名</param>
        /// <returns>配置方案</returns>
        IConfig Get(string name = null);

        /// <summary>
        /// 获取配置方案
        /// </summary>
        /// <param name="name">配置名</param>
        /// <returns>配置方案</returns>
        IConfig this[string name] { get; }

        /// <summary>
        /// 设定默认的配置名
        /// </summary>
        /// <param name="name">配置名</param>
        void SetDefault(string name);

        /// <summary>
        /// 扩展配置方案
        /// </summary>
        /// <param name="resolve">配置方案</param>
        /// <param name="name">配置名</param>
        void Extend(Func<IConfig> resolve, string name = null);
    }
}
