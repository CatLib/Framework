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

namespace CatLib
{
    /// <summary>
    /// 依赖标记
    /// </summary>
    public sealed class DependencyAttribute : Attribute
    {
        /// <summary>
        /// 依赖服务的别名或者服务名
        /// </summary>
        public string Alias { get; protected set; }

        /// <summary>
        /// 声明依赖
        /// </summary>
        /// <param name="alias">依赖服务的别名或者服务名</param>
        public DependencyAttribute(string alias)
        {
            Alias = alias;
        }

        /// <summary>
        /// 声明依赖
        /// </summary>
        public DependencyAttribute() { }
    }
}