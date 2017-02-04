using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Support;
using System.Collections.Generic;
using CatLib.Container;
using CatLib.FileSystem;
using CatLib.Contracts.ResourcesSystem;

namespace CatLib.ResourcesSystem {

    public class CResources : CComponent , IResources
    {

        /// <summary>
        /// 主依赖文件
        /// </summary>
        protected AssetBundleManifest assetBundleManifest;

        /// <summary>
        /// 被加载的资源包
        /// </summary>
        protected Dictionary<string , AssetBundle> loadAssetBundle = new Dictionary<string, AssetBundle>();

        /// <summary>
        /// 多样资源
        /// </summary>
        protected string variant;

        /// <summary>
        /// 多样资源
        /// </summary>
        public string Variant
        {
            get { return variant; }
            set { variant = value; }
        }


        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T Load<T>(string path) where T : Object
        {
            this.LoadManifest();
            return this.LoadAsset<T>(path);
        }

        public T[] LoadAll<T>(string path ) where T : Object
        {
            this.LoadManifest();
            string relPath, objName;
            LoadPath<T>(path, out relPath, out objName);
            AssetBundle assetTarget = LoadAssetBundle(relPath + "/" + objName);
            T[] targetAssets = assetTarget.LoadAllAssets<T>();
            return targetAssets;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        protected T LoadAsset<T>(string path) where T : Object
        {
            string relPath, objName;
            LoadPath<T>(path, out relPath, out objName);
            AssetBundle assetTarget = LoadAssetBundle(relPath);
            T targetAsset = assetTarget.LoadAsset<T>(objName);
            return targetAsset;

        }

        protected void LoadPath<T>(string path , out string relPath , out string objName) where T : Object
        {
            string variant = string.IsNullOrEmpty(Variant) ? string.Empty : "." + Variant;
            objName = NameWithTypeToSuffix<T>(path.Substring(path.LastIndexOf('/') + 1), variant);
            relPath = path.Substring(0, path.LastIndexOf('/')) + variant;

            if (!(CEnv.AssetPath + "/" + relPath).Exists())
            {
                objName = NameWithTypeToSuffix<T>(path.Substring(path.LastIndexOf('/') + 1), string.Empty);
                relPath = path.Substring(0, path.LastIndexOf('/'));
            }
        }

        protected AssetBundle LoadAssetBundle(string relPath)
        {
            foreach (string dependencies in assetBundleManifest.GetAllDependencies(relPath))
            {
                if (!loadAssetBundle.ContainsKey(dependencies))
                {
                    AssetBundle assetBundle = AssetBundle.LoadFromFile(CEnv.AssetPath + "/" + dependencies);
                    loadAssetBundle.Add(dependencies, assetBundle);
                }
            }

            AssetBundle assetTarget;
            if (!loadAssetBundle.ContainsKey(relPath))
            {
                assetTarget = AssetBundle.LoadFromFile(CEnv.AssetPath + "/" + relPath);
                loadAssetBundle.Add(relPath, assetTarget);
            }
            else
            {
                assetTarget = loadAssetBundle[relPath];
            }

            return assetTarget;

        }

        /// <summary>
        /// 加载资源（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public UnityEngine.Coroutine LoadAsyn<T>(string path , System.Action<T> callback) where T : Object
        {
            this.LoadManifest();
            return Application.StartCoroutine(this.LoadAssetAsyn<T>(path , callback));

        }

        public UnityEngine.Coroutine LoadAllAsyn<T>(string path, System.Action<T[]> callback) where T : Object
        {
            this.LoadManifest();
            return Application.StartCoroutine(this.LoadAssetAllAsyn<T>(path, callback));
        }

        /// <summary>
        /// 加载依赖文件
        /// </summary>
        protected void LoadManifest()
        {
            if (assetBundleManifest != null) { return; }
            string manifestPath = CEnv.AssetPath + "/" + CEnv.PlatformToName(CEnv.SwitchPlatform);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(manifestPath);
            assetBundleManifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        /// <summary>
        /// 加载资源（异步） 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        protected IEnumerator LoadAssetAsyn<T>(string path , System.Action<T> callback) where T : Object
        {

            string relPath, objName;
            LoadPath<T>(path, out relPath, out objName);

            AssetBundle assetTarget = null;

            yield return LoadAssetBundleAsyn(relPath, (ab) =>
            {
                assetTarget = ab;
            });

            AssetBundleRequest targetAssetRequest = assetTarget.LoadAssetAsync<T>(objName);
            yield return targetAssetRequest;
            T targetAsset = targetAssetRequest.asset as T;

            callback.Invoke(targetAsset);

        }

        protected IEnumerator LoadAssetAllAsyn<T>(string path, System.Action<T[]> callback) where T : Object
        {

            string relPath, objName;
            LoadPath<T>(path, out relPath, out objName);

            AssetBundle assetTarget = null;

            yield return LoadAssetBundleAsyn(relPath + "/" + objName, (ab) =>
            {
                assetTarget = ab;
            });

            AssetBundleRequest targetAssetRequest = assetTarget.LoadAllAssetsAsync<T>();
            yield return targetAssetRequest;
            T[] targetAsset = targetAssetRequest.allAssets.To<T>();

            callback.Invoke(targetAsset);

        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="relPath"></param>
        /// <returns></returns>
        protected IEnumerator LoadAssetBundleAsyn(string relPath , System.Action<AssetBundle> assetBundle) 
        {

            foreach (string dependencies in assetBundleManifest.GetAllDependencies(relPath))
            {
                if (!loadAssetBundle.ContainsKey(dependencies))
                {
                    AssetBundleCreateRequest assetBundleDependencies = AssetBundle.LoadFromFileAsync(CEnv.AssetPath + "/" + dependencies);
                    yield return assetBundleDependencies;
                    loadAssetBundle.Add(dependencies, assetBundleDependencies.assetBundle);
                }
            }

            AssetBundle assetTarget;
            if (!loadAssetBundle.ContainsKey(relPath))
            {
                AssetBundleCreateRequest assetTargetBundleRequest = AssetBundle.LoadFromFileAsync(CEnv.AssetPath + "/" + relPath);
                yield return assetTargetBundleRequest;
                assetTarget = assetTargetBundleRequest.assetBundle;
                loadAssetBundle.Add(relPath, assetTarget);
            }
            else
            {
                assetTarget = loadAssetBundle[relPath];
            }

            assetBundle(assetTarget);

        }


        /// <summary>
        /// 类型对应的后缀
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected string NameWithTypeToSuffix<T>(string name , string variant) where T : Object
        {
            string suffix = string.Empty;
            System.Type t = typeof(T);
            if (t == typeof(GameObject))
            {
                suffix = ".prefab";
            }else if(t == typeof(Material))
            {
                suffix = ".mat";
            }else if(t == typeof(Shader))
            {
                suffix = ".shader";
            }

            if (name.Length > suffix.Length && name.Substring(name.Length - suffix.Length) != suffix)
            {
                name += suffix;
            }

            return name + variant;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="unloadAllLoadedObjects">是否释放已经被加载的Asset资源</param>
        public void Unload(bool unloadAllLoadedObjects)
        {
            foreach(AssetBundle bundle in loadAssetBundle.ToArray())
            {
                bundle.Unload(unloadAllLoadedObjects);
            }
        }

    }

}
