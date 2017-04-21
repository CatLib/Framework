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

namespace CatLib.API.Resources
{
    /// <summary>
    /// Assetbundle
    /// </summary>
    public interface IAssetBundle
    {
        //string Variant{ get; set; }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path">加载路径</param>
        /// <returns>加载的对象</returns>
        Object LoadAsset(string path);

        /// <summary>
        /// 加载全部资源
        /// </summary>
        /// <param name="path">加载路径</param>
        /// <returns></returns>
        Object[] LoadAssetAll(string path);

        /// <summary>
        /// 加载资源（异步） 
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="callback">回调</param>
        /// <returns>协程</returns>
        UnityEngine.Coroutine LoadAssetAsync(string path, System.Action<Object> callback);

        /// <summary>
        /// 加载资源（异步） 
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="callback">回调</param>
        /// <returns>协程</returns>
        UnityEngine.Coroutine LoadAssetAllAsync(string path, System.Action<Object[]> callback);

        /// <summary>
        /// 强制卸载全部资源包（一般情况请不要调用）
        /// </summary>
        bool UnloadAll();

        /// <summary>
        /// 卸载指定资源包
        /// </summary>
        /// <param name="assetbundlePath">资源包路径</param>
        bool UnloadAssetBundle(string assetbundlePath);
    }
}