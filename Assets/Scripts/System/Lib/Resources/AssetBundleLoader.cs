using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CatLib.API;
using CatLib.API.IO;
using CatLib.API.Resources;
using System.IO;

namespace CatLib.Resources {

    public class AssetBundleLoader : Component , IAssetBundle
    {

        [Dependency]
        public IIOFactory IO { get; set; }

        [Dependency]
        public IEnv Env { get; set; }

        private IDisk disk;

        /// <summary>
        /// 磁盘
        /// </summary>
        private IDisk Disk{

            get{
                return disk ?? (disk = IO.Disk());
            }
        }

        /// <summary>
        /// 主依赖文件
        /// </summary>
        protected AssetBundleManifest assetBundleManifest;

        /// <summary>
        /// 被加载的资源包
        /// </summary>
        protected Dictionary<string, MainBundle> loadAssetBundles = new Dictionary<string, MainBundle>();

        /// <summary>
        /// 依赖的资源包
        /// </summary>
        protected Dictionary<string, DependenciesBundle> dependenciesBundles = new Dictionary<string, DependenciesBundle>();

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
        public Object LoadAsset(string path)
        {
            LoadManifest();
            string relPath, objName;
            ParsePath(path , out relPath, out objName);

            AssetBundle assetTarget = LoadAssetBundle(Env.AssetPath, relPath);
            if (assetTarget == null) { return null; }
            return assetTarget.LoadAsset(objName);
        }


        public Object[] LoadAssetAll(string path)
        {
            LoadManifest();
            string relPath, objName;
            ParsePath(path , out relPath, out objName);

            AssetBundle assetTarget = LoadAssetBundle(Env.AssetPath, relPath + Path.AltDirectorySeparatorChar + Path.GetFileNameWithoutExtension(objName));
            if (assetTarget == null) { return null; }
            return assetTarget.LoadAllAssets();
        }

        /// <summary>
        /// 加载资源（异步） 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public UnityEngine.Coroutine LoadAssetAsync(string path, System.Action<Object> callback)
        {
            return App.StartCoroutine(LoadAssetAsyncIEnumerator(path, callback));
        }

        /// <summary>
        /// 加载资源（异步） 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public UnityEngine.Coroutine LoadAssetAllAsync(string path, System.Action<Object[]> callback)
        {
            return App.StartCoroutine(LoadAssetAllAsyncIEnumerator(path, callback));
        }

        /// <summary>
        /// 卸载全部资源包
        /// </summary>
        public void UnloadAll()
        {
            foreach (var asset in loadAssetBundles)
            {
                asset.Value.Bundle.Unload(true);
            }
            foreach (var asset in dependenciesBundles)
            {
                asset.Value.Bundle.Unload(true);
            }
            loadAssetBundles.Clear();
            dependenciesBundles.Clear();
        }

        /// <summary>
        /// 卸载资源包
        /// </summary>
        /// <param name="assetbundlePath">资源包路径</param>
        public void UnloadAssetBundle(string assetbundlePath)
        {
            if (loadAssetBundles.ContainsKey(assetbundlePath))
            {
                foreach (string dependencies in assetBundleManifest.GetAllDependencies(assetbundlePath))
                {
                    UnloadDependenciesAssetBundle(dependencies);
                }
                loadAssetBundles[assetbundlePath].Bundle.Unload(true);
                loadAssetBundles.Remove(assetbundlePath);
            }
        }

        /// <summary>
        /// 加载资源（异步） 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        protected IEnumerator LoadAssetAsyncIEnumerator(string path, System.Action<Object> callback)
        {

            LoadManifest();
            string relPath, objName;
            ParsePath(path, out relPath, out objName);

            AssetBundle assetTarget = null;

            yield return LoadAssetBundleAsync(Env.AssetPath, relPath, (ab) =>
            {
                assetTarget = ab;
            });

            AssetBundleRequest targetAssetRequest = assetTarget.LoadAssetAsync(objName);
            yield return targetAssetRequest;

            if (targetAssetRequest != null)
            {
                callback.Invoke(targetAssetRequest.asset);
            }else
            {
                callback.Invoke(null);
            }

        }

        protected IEnumerator LoadAssetAllAsyncIEnumerator(string path , System.Action<Object[]> callback)
        {

            LoadManifest();
            string relPath, objName;
            ParsePath(path, out relPath, out objName);

            AssetBundle assetTarget = null;

            yield return LoadAssetBundleAsync(Env.AssetPath, relPath + System.IO.Path.PathSeparator + System.IO.Path.GetFileNameWithoutExtension(objName), (ab) =>
            {
                assetTarget = ab;
            });

            if(assetTarget == null){

                callback.Invoke(new Object[]{});
                yield break;

            }

            AssetBundleRequest targetAssetRequest = assetTarget.LoadAllAssetsAsync();
            yield return targetAssetRequest;

            if (targetAssetRequest != null)
            {
                callback.Invoke(targetAssetRequest.allAssets);
            }else
            {
                callback.Invoke(null);
            }

        }


        /// <summary>
        /// 加载依赖文件
        /// </summary>
        protected void LoadManifest()
        {
            if (assetBundleManifest != null) { return; }

            #if UNITY_EDITOR
            if (Env.DebugLevel == DebugLevels.Auto)
            {
                return;
            }
            #endif

            AssetBundle assetBundle = LoadAssetBundle(Env.AssetPath, Env.PlatformToName(Env.SwitchPlatform));
            assetBundleManifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        protected AssetBundle LoadAssetBundle(string envPath, string relPath)
        {

            LoadDependenciesAssetBundle(envPath, relPath);

            AssetBundle assetTarget;
            if (!loadAssetBundles.ContainsKey(relPath))
            {

                if (Disk.IsCrypt)
                {
                    IFile file = Disk.File(envPath + Path.AltDirectorySeparatorChar + relPath, PathTypes.Absolute);
                    assetTarget = AssetBundle.LoadFromMemory(file.Read());
                }
                else
                {
                    assetTarget = AssetBundle.LoadFromFile(envPath + Path.AltDirectorySeparatorChar + relPath);
                }

                loadAssetBundles.Add(relPath, new MainBundle(assetTarget));

            }
            else
            {
                assetTarget = loadAssetBundles[relPath].Bundle;
            }

            return assetTarget;

        }

        /// <summary>
        /// 加载依赖的AssetBundle
        /// </summary>
        protected void LoadDependenciesAssetBundle(string envPath , string relPath){

            if (assetBundleManifest == null) { return; }
            foreach (string dependencies in assetBundleManifest.GetAllDependencies(relPath))
            {
                if (!dependenciesBundles.ContainsKey(dependencies))
                {

                    AssetBundle assetBundle = AssetBundle.LoadFromFile(envPath + Path.AltDirectorySeparatorChar + dependencies);
                    dependenciesBundles.Add(dependencies, new DependenciesBundle(assetBundle));

                }else{

                    dependenciesBundles[dependencies].RefCount++;

                }
            }

        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="relPath"></param>
        /// <returns></returns>
        protected IEnumerator LoadAssetBundleAsync(string envPath , string relPath , System.Action<AssetBundle> complete) 
        {

            yield return LoadDependenciesAssetBundleAsync(envPath , relPath);

            AssetBundle assetTarget;
            if (!loadAssetBundles.ContainsKey(relPath))
            {
                AssetBundleCreateRequest assetTargetBundleRequest;
                if (Disk.IsCrypt)
                {
                    IFile file = Disk.File(envPath + Path.AltDirectorySeparatorChar + relPath, PathTypes.Absolute);
                    assetTargetBundleRequest = AssetBundle.LoadFromMemoryAsync(file.Read());
                }
                else
                {
                    assetTargetBundleRequest = AssetBundle.LoadFromFileAsync(envPath + Path.AltDirectorySeparatorChar + relPath);
                }

                yield return assetTargetBundleRequest;
                assetTarget = assetTargetBundleRequest.assetBundle;
                loadAssetBundles.Add(relPath, new MainBundle(assetTarget));
            }
            else
            {
                assetTarget = loadAssetBundles[relPath].Bundle;
            }

            complete(assetTarget);

        }

        /// <summary>
        /// 异步加载资源依赖
        /// </summary>
        /// <param name="relPath"></param>
        /// <returns></returns>
        protected IEnumerator LoadDependenciesAssetBundleAsync(string envPath , string relPath){

            foreach (string dependencies in assetBundleManifest.GetAllDependencies(relPath))
            {
                if (!loadAssetBundles.ContainsKey(dependencies))
                {
                    AssetBundleCreateRequest assetBundleDependencies;
                    if (Disk.IsCrypt)
                    {
                        IFile file = Disk.File(envPath + Path.AltDirectorySeparatorChar + relPath, PathTypes.Absolute);
                        assetBundleDependencies = AssetBundle.LoadFromMemoryAsync(file.Read());
                    }
                    else
                    {
                        assetBundleDependencies = AssetBundle.LoadFromFileAsync(envPath + Path.AltDirectorySeparatorChar + dependencies);
                    }
                    yield return assetBundleDependencies;
                    dependenciesBundles.Add(dependencies, new DependenciesBundle(assetBundleDependencies.assetBundle));
                }else{
                    dependenciesBundles[dependencies].RefCount++;
                }
            }

        } 

        /// <summary>
        /// 解析文件路径
        /// </summary>
        protected void ParsePath(string path, out string relPath , out string objName)
        {
            string variant = string.IsNullOrEmpty(Variant) ? string.Empty : "." + Variant;
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            string extension =  System.IO.Path.GetExtension(path);
            string dirPath = System.IO.Path.GetDirectoryName(path);

            objName = name + variant + extension;
            relPath = dirPath + variant;
        }

        /// <summary>
        /// 卸载依赖资源包
        /// </summary>
        /// <param name="assetbundlePath">资源包路径</param>
        private void UnloadDependenciesAssetBundle(string assetbundlePath)
        {
            if (dependenciesBundles.ContainsKey(assetbundlePath))
            {
                dependenciesBundles[assetbundlePath].RefCount--;
                if (dependenciesBundles[assetbundlePath].RefCount <= 0)
                {
                    dependenciesBundles[assetbundlePath].Bundle.Unload(true);
                    dependenciesBundles.Remove(assetbundlePath);
                }
            }
        }

    }

}
