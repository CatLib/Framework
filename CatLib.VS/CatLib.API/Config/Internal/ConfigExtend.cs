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

namespace CatLib
{
    /// <summary>
    /// 配置扩展
    /// </summary>
    public static class ConfigExtend
    {
        /// <summary>
        /// 安全的获取配置
        /// </summary>
        /// <typeparam name="T">默认类型</typeparam>
        /// <param name="config">配置值</param>
        /// <param name="name">配置名字</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static T SafeGet<T>(this IConfig config, string name, T def = default(T))
        {
            return config == null ? def : config.Get(name, def);
        }
    }
}
