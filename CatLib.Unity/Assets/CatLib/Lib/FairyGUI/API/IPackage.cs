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

namespace CatLib.API.FairyGUI
{
    /// <summary>
    /// 资源包
    /// </summary>
    public interface IPackage
    {
        /// <summary>
        /// 添加资源包
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        /// <returns></returns>
        UIPackage AddPackage(string assetPath);

        /// <summary>
        /// 异步加载Package
        /// </summary>
        /// <param name="assetBundlePath">资源包路径</param>
        /// <param name="complete">完成时的回调</param>
        /// <returns>协程</returns>
        UnityEngine.Coroutine AddPackageAsync(string assetBundlePath, Action<UIPackage> complete);
    }
}