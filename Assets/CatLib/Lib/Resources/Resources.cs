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

using CatLib.API;
using CatLib.API.IO;
using CatLib.API.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace CatLib.Resources
{
    /// <summary>
    /// 资源
    /// </summary>
    public sealed class Resources : IResources
    {
        /// <summary>
        /// AssetBundle 加载器
        /// </summary>
        [Inject]
        public AssetBundleLoader AssetBundleLoader { get; set; }

        /// <summary>
        /// 资源托管服务
        /// </summary>
        public IResourcesHosted ResourcesHosted { get; set; }

        /// <summary>
        /// 环境
        /// </summary>
        [Inject]
        public IEnv Env { get; set; }

        /// <summary>
        /// 应用程序
        /// </summary>
        [Inject]
        public IApplication App { get; set; }

        /// <summary>
        /// 设定托管
        /// </summary>
        /// <param name="hosted">托管程序</param>
        public void SetHosted(IResourcesHosted hosted)
        {
            ResourcesHosted = hosted;
        }

        /// <summary>
        /// 后缀名字典
        /// </summary>
        private readonly Dictionary<System.Type, string> extensionDict = new Dictionary<System.Type, string>();

        /// <summary>
        /// 构造资源服务
        /// </summary>
        public Resources()
        {
            extensionDict.Add(typeof(Object), ".prefab");
            extensionDict.Add(typeof(GameObject), ".prefab");
            extensionDict.Add(typeof(TextAsset), ".txt");
            extensionDict.Add(typeof(Material), ".mat");
            extensionDict.Add(typeof(Shader), ".shader");
        }

        /// <summary>
        /// 增加后缀关系
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="extension">对应后缀</param>
        public void AddExtension(System.Type type, string extension)
        {
            extensionDict.Remove(type);
            extensionDict.Add(type, extension);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>加载的对象</returns>
        public IObject Load(string path, LoadTypes loadType = LoadTypes.AssetBundle)
        {
            return Load(path, typeof(Object), loadType);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>加载的对象</returns>
        public IObject Load<T>(string path, LoadTypes loadType = LoadTypes.AssetBundle) where T : Object
        {
            return Load(path, typeof(T), loadType);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>加载的对象</returns>
        public IObject Load(string path, System.Type type, LoadTypes loadType = LoadTypes.AssetBundle)
        {
            if (loadType == LoadTypes.Resources)
            {
                return MakeDefaultObjectInfo(UnityEngine.Resources.Load(path, type));
            }

            path = PathFormat(path, type);

#if UNITY_EDITOR
            if (Env.DebugLevel == DebugLevels.Auto || Env.DebugLevel == DebugLevels.Dev)
            {
                return MakeDefaultObjectInfo(UnityEditor.AssetDatabase.LoadAssetAtPath("Assets" + Env.ResourcesBuildPath + Path.AltDirectorySeparatorChar + path, type));
            }
#endif

            IObject hosted;
            if (ResourcesHosted != null)
            {
                hosted = ResourcesHosted.Get(path);
                if (hosted != null)
                {
                    return hosted;
                }
            }

            var obj = AssetBundleLoader.LoadAsset(path);
            hosted = ResourcesHosted != null ? ResourcesHosted.Hosted(path, obj) : MakeDefaultObjectInfo(obj);
            return hosted;
        }

        /// <summary>
        /// 加载全部
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>加载的对象数组</returns>
        public IObject[] LoadAll(string path, LoadTypes loadType = LoadTypes.AssetBundle)
        {
            if (loadType == LoadTypes.Resources)
            {
                return MakeDefaultObjectInfos(UnityEngine.Resources.LoadAll(path));
            }

#if UNITY_EDITOR
            if (Env.DebugLevel == DebugLevels.Auto || Env.DebugLevel == DebugLevels.Dev)
            {
                throw new System.Exception("not support [LoadAll] in auto env");
            }
#endif

            var objs = AssetBundleLoader.LoadAssetAll(path);
            return MakeDefaultObjectInfos(objs);
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="path">加载路径</param>
        /// <param name="callback">回调</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>协程</returns>
        public UnityEngine.Coroutine LoadAsync(string path, System.Action<IObject> callback, LoadTypes loadType = LoadTypes.AssetBundle)
        {
            return LoadAsync(path, typeof(Object), callback, loadType);
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">加载路径</param>
        /// <param name="callback">回调</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>协程</returns>
        public UnityEngine.Coroutine LoadAsync<T>(string path, System.Action<IObject> callback, LoadTypes loadType = LoadTypes.AssetBundle) where T : Object
        {
            return LoadAsync(path, typeof(T), callback, loadType);
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="path">加载路径</param>
        /// <param name="type">资源类型</param>
        /// <param name="callback">回调</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>协程</returns>
        public UnityEngine.Coroutine LoadAsync(string path, System.Type type, System.Action<IObject> callback, LoadTypes loadType = LoadTypes.AssetBundle)
        {
            if (loadType == LoadTypes.Resources)
            {
                return App.StartCoroutine(LoadAsyncWithUnityResources(path, type, callback));
            }

            path = PathFormat(path, type);

#if UNITY_EDITOR
            if (Env.DebugLevel == DebugLevels.Auto || Env.DebugLevel == DebugLevels.Dev)
            {
                return App.StartCoroutine(IEnumeratorWrapper(() =>
                {
                    callback(new DefaultObjectWrapper(UnityEditor.AssetDatabase.LoadAssetAtPath("Assets" + Env.ResourcesBuildPath + Path.AltDirectorySeparatorChar + path, type)));
                }));
            }
#endif

            IObject hosted;
            if (ResourcesHosted != null)
            {
                hosted = ResourcesHosted.Get(path);
                if (hosted != null)
                {
                    return App.StartCoroutine(IEnumeratorWrapper(() => callback(hosted)));
                }
            }

            return AssetBundleLoader.LoadAssetAsync(PathFormat(path, type), (obj) =>
            {
                hosted = ResourcesHosted != null ? ResourcesHosted.Hosted(path, obj) : MakeDefaultObjectInfo(obj);
                callback(hosted);
            });
        }

        /// <summary>
        /// 异步加载全部资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="callback">回调</param>
        /// <param name="loadType">加载类型</param>
        /// <returns>协程</returns>
        public UnityEngine.Coroutine LoadAllAsync(string path, System.Action<IObject[]> callback, LoadTypes loadType = LoadTypes.AssetBundle)
        {
            if (loadType == LoadTypes.Resources)
            {

                return App.StartCoroutine(LoadAllAsyncWithUnityResources(path, callback));

            }
#if UNITY_EDITOR
            if (Env.DebugLevel == DebugLevels.Auto || Env.DebugLevel == DebugLevels.Dev)
            {
                throw new System.Exception("not support [LoadAllAsync] in auto env");
            }
#endif
            return AssetBundleLoader.LoadAssetAllAsync(path, (objs) =>
            {
                callback(MakeDefaultObjectInfos(objs));
            });
        }

        /// <summary>
        /// 迭代器包装
        /// </summary>
        /// <param name="callback">回调</param>
        /// <returns>迭代器</returns>
        private IEnumerator IEnumeratorWrapper(System.Action callback)
        {
            callback.Invoke();
            yield break;
        }

        /// <summary>
        /// 路径格式化
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="type">资源类型</param>
        /// <returns>格式化后的路径</returns>
        private string PathFormat(string path, System.Type type)
        {
            var extension = Path.GetExtension(path);
            if (extension != string.Empty)
            {
                return path;
            }
            if (extensionDict.ContainsKey(type))
            {
                return path + extensionDict[type];
            }
            return path;
        }

        /// <summary>
        /// 使用Unity资源进行加载
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <param name="callback">回调函数</param>
        /// <returns>迭代器</returns>
        private IEnumerator LoadAsyncWithUnityResources(string path, System.Type type, System.Action<IObject> callback)
        {
            var request = UnityEngine.Resources.LoadAsync(path, type);
            yield return request;

            callback(MakeDefaultObjectInfo(request.asset));
        }

        /// <summary>
        /// 使用Unity资源加载器加载全部
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="callback">回调函数</param>
        /// <returns>迭代器</returns>
        private IEnumerator LoadAllAsyncWithUnityResources(string path, System.Action<IObject[]> callback)
        {
            var objs = UnityEngine.Resources.LoadAll(path);
            callback(MakeDefaultObjectInfos(objs));
            yield break;
        }

        /// <summary>
        /// 制作默认的对象包装
        /// </summary>
        /// <param name="obj">加载的对象</param>
        /// <returns>包装的对象</returns>
        private IObject MakeDefaultObjectInfo(Object obj)
        {
            return new DefaultObjectWrapper(obj);
        }

        /// <summary>
        /// 制作默认的对象包装
        /// </summary>
        /// <param name="objs">加载的对象数组</param>
        /// <returns>包装的对象数组</returns>
        private IObject[] MakeDefaultObjectInfos(Object[] objs)
        {
            var hosted = new IObject[objs.Length];
            for (var i = 0; i < objs.Length; i++)
            {
                hosted[i] = MakeDefaultObjectInfo(objs[i]);
            }

            return hosted;
        }
    }
}
