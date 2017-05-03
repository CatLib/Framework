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
    /// 资源
    /// </summary>
    public interface IResources
    {
        /// <summary>
        /// 增加后缀关系
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="extension">对应后缀</param>
        void AddExtension(System.Type type, string extension);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>加载的对象</returns>
        IObject Load(string path, LoadTypes loadType = LoadTypes.AssetBundle);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>加载的对象</returns>
        IObject Load(string path, System.Type type, LoadTypes loadType = LoadTypes.AssetBundle);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>加载的对象</returns>
        IObject Load<T>(string path, LoadTypes loadType = LoadTypes.AssetBundle) where T : Object;

        /// <summary>
        /// 加载全部
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>加载的对象数组</returns>
        IObject[] LoadAll(string path, LoadTypes loadType = LoadTypes.AssetBundle);

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="path">加载路径</param>
        /// <param name="callback">回调</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>协程</returns>
        UnityEngine.Coroutine LoadAsync(string path, System.Action<IObject> callback, LoadTypes loadType = LoadTypes.AssetBundle);

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="path">加载路径</param>
        /// <param name="type">资源类型</param>
        /// <param name="callback">回调</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>协程</returns>
        UnityEngine.Coroutine LoadAsync(string path, System.Type type, System.Action<IObject> callback, LoadTypes loadType = LoadTypes.AssetBundle);

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">加载路径</param>
        /// <param name="callback">回调</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>协程</returns>
        UnityEngine.Coroutine LoadAsync<T>(string path, System.Action<IObject> callback, LoadTypes loadType = LoadTypes.AssetBundle) where T : Object;

        /// <summary>
        /// 异步加载全部资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="callback">回调</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>协程</returns>
        UnityEngine.Coroutine LoadAllAsync(string path, System.Action<IObject[]> callback, LoadTypes loadType = LoadTypes.AssetBundle);
    }
}