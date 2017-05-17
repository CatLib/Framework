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
    /// 配置
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <returns></returns>
        string this[string name] { get; }
    }
}