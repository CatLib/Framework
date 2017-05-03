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

using UnityEngine;

namespace CatLib.Resources
{
    /// <summary>
    /// 主资源包
    /// </summary>
    internal sealed class MainBundle
    {
        /// <summary>
        /// 资源包
        /// </summary>
		public AssetBundle Bundle { get; set; }

        /// <summary>
        /// 构建一个主资源包
        /// </summary>
        /// <param name="bundle">资源包</param>
        public MainBundle(AssetBundle bundle)
        {
            Bundle = bundle;
        }
    }
}