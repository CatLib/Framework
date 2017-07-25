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

namespace CatLib.API.Routing
{
    /// <summary>
    /// 路由标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class RoutedAttribute : Attribute
    {
        /// <summary>
        /// 路由组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 条件
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Defaults { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        public RoutedAttribute()
        {
        }

        /// <summary>
        /// 路由路径
        /// </summary>
        /// <param name="path"></param>
        public RoutedAttribute(string path)
        {
            Path = path;
        }
    }
}