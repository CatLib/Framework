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
using FairyGUI;
using CatLib.API.Resources;
using CatLib.API.FairyGUI;

namespace CatLib.FairyGUI
{
    /// <summary>
    /// 包
    /// </summary>
    public class Package : IPackage
    {
        /// <summary>
        /// 资源系统
        /// </summary>
        [Inject]
        public IResources Resources { get; set; }

        /// <summary>
        /// AssetBundle
        /// </summary>
        [Inject]
        public IAssetBundle AssetBundle { get; set; }

        /// <summary>
        /// 增加一个包
        /// </summary>
        /// <param name="assetPath">资源包路径</param>
        /// <returns>FairyGUI UIPackage</returns>
        public UIPackage AddPackage(string assetPath)
        {
            IObject obj = null;
            var package = UIPackage.AddPackage(assetPath, (name, extension, type) =>
           {
               obj = Resources.Load(name + extension, type);
               return obj == null ? null : obj.Original;
           });
            if (package != null && obj != null)
            {
                obj.Get(package);
            }
            return package;
        }

        /// <summary>
        /// 异步增加一个包
        /// </summary>
        /// <param name="assetBundlePath">资源包路径</param>
        /// <returns>协程</returns>
        public UnityEngine.Coroutine AddPackageAsync(string assetBundlePath , Action<UIPackage> complete)
        {
            return AssetBundle.LoadAssetBundleAsync(assetBundlePath, bundle =>
            {
                var package = UIPackage.AddPackage(bundle);
                complete.Invoke(package);
            });
        }
    }
}