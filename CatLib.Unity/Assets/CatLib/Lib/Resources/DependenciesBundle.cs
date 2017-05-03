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

namespace CatLib.Resources
{
    /// <summary>
    /// 依赖资源包
    /// </summary>
    internal sealed class DependenciesBundle
    {
        /// <summary>
        /// 引用计数
        /// </summary>
        public int RefCount { get; set; }

        /// <summary>
        /// AssetBundle
        /// </summary>
        public UnityEngine.AssetBundle Bundle { get; set; }

        /// <summary>
        /// 构建一个依赖资源包
        /// </summary>
        /// <param name="assetBundle">AssetBundle</param>
        public DependenciesBundle(UnityEngine.AssetBundle assetBundle)
        {
            Bundle = assetBundle;
            RefCount = 1;
        }
    }
}