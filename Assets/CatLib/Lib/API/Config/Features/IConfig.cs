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
    /// 配置
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// 配置名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 配置内容
        /// </summary>
        object[] Config { get; }
    }
}