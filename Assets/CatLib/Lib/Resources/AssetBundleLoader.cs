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
using System.Collections;
using System.Collections.Generic;
using CatLib.API;
using CatLib.API.IO;
using CatLib.API.Resources;
using System.IO;

namespace CatLib.Resources {

    public class AssetBundleLoader :  IAssetBundle
    {

        [Dependency]
        public IIOFactory IO { get; set; }

        [Dependency]
        public IEnv Env { get; set; }

        [Dependency]
        public IApplication App { get; set; }

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
        /// 被加载的主资源包
        /// </summary>
        protected Dictionary<string, MainBundle> loadAssetBundles = new Dictionary<string, MainBundle>();

        /// <summary>
        /// 依赖的资源包
        /// </summary>
        protected Dictionary<string, DependenciesBundle> dependenciesBundles = new Dictionary<string, DependenciesBundle>();

        /// <summary>
        /// 加载中的资源包
        /// </summary>
        protected List<string> onLoadingAssetBundles = new List<string>();

        /// <summary>
        /// 被保护的资源包列表（不能被卸载）
        /// </summary>
        protected Dictionary<string , int> protectedList = new Dictionary<string , int>();

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


        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
        /// 强制卸载全部资源包（一般情况请不要调用）
        /// </summary>
        public bool UnloadAll()
        {
            if (onLoadingAssetBundles.Count > 0) { return false; }
            foreach (var asset in loadAssetBundles)
            {
                if (asset.Value.Bundle != null)
                {
                    asset.Value.Bundle.Unload(true);
                }
            }
            foreach (var asset in dependenciesBundles)
            {
                if (asset.Value.Bundle != null)
                {
                    asset.Value.Bundle.Unload(true);
                }
            }
            loadAssetBundles.Clear();
            dependenciesBundles.Clear();
            protectedList.Clear();
            return true;
        }

        /// <summary>
        /// 卸载指定资源包
        /// </summary>
        /// <param name="assetbundlePath">资源包路径</param>
        public bool UnloadAssetBundle(string assetbundlePath)
        {
            if (protectedList.ContainsKey(assetbundlePath)) { return false; }
            if (loadAssetBundles.ContainsKey(assetbundlePath))
            {
                //如果除了作为主包外还被其他包引用那么就不释放
                if (!dependenciesBundles.ContainsKey(assetbundlePath))
                {
                    //防止assetbundle因为一些意外原因已经被释放了
                    if (loadAssetBundles[assetbundlePath].Bundle != null)
                    {
                        loadAssetBundles[assetbundlePath].Bundle.Unload(true);
                    }
                }
                loadAssetBundles.Remove(assetbundlePath);

                //释放依赖
                foreach (string dependencies in assetBundleManifest.GetAllDependencies(assetbundlePath))
                {
                    UnloadDependenciesAssetBundle(dependencies);
                }
            }
            return true;
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

            //加入保护的列表
            if (!protectedList.ContainsKey(relPath))
            {
                protectedList.Add(relPath, 1);
            }else
            {
                protectedList[relPath]++;
            }

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

            protectedList[relPath]--;
            if (protectedList[relPath] <= 0) { protectedList.Remove(relPath); }

        }

        protected IEnumerator LoadAssetAllAsyncIEnumerator(string path , System.Action<Object[]> callback)
        {

            LoadManifest();
            string relPath, objName;
            ParsePath(path, out relPath, out objName);

            AssetBundle assetTarget = null;

            if (!protectedList.ContainsKey(relPath))
            {
                protectedList.Add(relPath, 1);
            }
            else
            {
                protectedList[relPath]++;
            }

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

            protectedList[relPath]--;
            if (protectedList[relPath] <= 0) { protectedList.Remove(relPath); }

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

            if (onLoadingAssetBundles.Contains(relPath))
            {
                throw new System.Exception("this asset bundle has in async load");
            }

            LoadDependenciesAssetBundle(envPath, relPath);

            AssetBundle assetTarget;
            if (!loadAssetBundles.ContainsKey(relPath) || loadAssetBundles[relPath].Bundle == null)
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

                loadAssetBundles.Remove(relPath);
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
                if (!dependenciesBundles.ContainsKey(dependencies) || dependenciesBundles[dependencies].Bundle == null)
                {

                    AssetBundle assetBundle = AssetBundle.LoadFromFile(envPath + Path.AltDirectorySeparatorChar + dependencies);
                    dependenciesBundles.Remove(dependencies);
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

            //如果主包中已经包含了那么直接回调
            if (loadAssetBundles.ContainsKey(relPath))
            {
                complete(loadAssetBundles[relPath].Bundle);
                yield break;
            }

            //如果处于其他请求在处理的依赖包
            if (onLoadingAssetBundles.Contains(relPath))
            {
                while (true)
                {
                    yield return new WaitForEndOfFrame();
                    if (onLoadingAssetBundles.Contains(relPath)) { continue; }

                    //如果被别的主包加载请求进行了加载的话直接返回
                    if (loadAssetBundles.ContainsKey(relPath))
                    {
                        //保证bundle是有效的
                        if (loadAssetBundles[relPath].Bundle != null)
                        {
                            complete(loadAssetBundles[relPath].Bundle);
                            yield break;
                        }
                    }

                    //如果是其他依赖包建立的依赖加载,那么将依赖包加入主包列表
                    if (dependenciesBundles.ContainsKey(relPath))
                    {
                        //保证bundle是有效的
                        if (dependenciesBundles[relPath].Bundle != null)
                        {
                            loadAssetBundles.Remove(relPath);
                            loadAssetBundles.Add(relPath, new MainBundle(dependenciesBundles[relPath].Bundle));
                            complete(loadAssetBundles[relPath].Bundle);
                            yield break;
                        }
                    }
                }
            }

            //将 asset bundle 加入加载中列表
            onLoadingAssetBundles.Add(relPath);

            //加载依赖资源
            yield return LoadDependenciesAssetBundleAsync(envPath , relPath);

            //创建加载主包请求
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

            //等待主包加载完成
            yield return assetTargetBundleRequest;

            AssetBundle assetTarget = assetTargetBundleRequest.assetBundle;
            if (assetTarget != null)
            {
                loadAssetBundles.Remove(relPath);
                loadAssetBundles.Add(relPath, new MainBundle(assetTarget));
            }
            //从加载列表中移除
            onLoadingAssetBundles.Remove(relPath);

            complete(assetTarget);

        }

        /// <summary>
        /// 异步加载资源依赖
        /// </summary>
        /// <param name="relPath"></param>
        /// <returns></returns>
        protected IEnumerator LoadDependenciesAssetBundleAsync(string envPath , string relPath){

            string[] dependenciesAssetBundles = assetBundleManifest.GetAllDependencies(relPath);
     
            string dependencies;
            for (int i = 0; i < dependenciesAssetBundles.Length; i++)
            {

                dependencies = dependenciesAssetBundles[i];

                //如果处于其他请求在处理的依赖包
                if (onLoadingAssetBundles.Contains(relPath))
                {
                    //挂起等待其他程序的加载完成
                    while (true)
                    {
                        yield return new WaitForEndOfFrame();
                        if (onLoadingAssetBundles.Contains(relPath)){ continue; }
                        break;
                    }

                    //如果是其他依赖包发起的加载同时保证asset bundle是有效的，那么直接开始下一个依赖包的加载
                    if (dependenciesBundles.ContainsKey(dependencies) && dependenciesBundles[dependencies].Bundle != null) { continue; }

                    //如果是主包发起的加载同时保证asset bundle是有效的，那么这次请求只需要将主包拷贝入依赖列表
                    if (loadAssetBundles.ContainsKey(dependencies) && loadAssetBundles[dependencies].Bundle != null)
                    {
                        dependenciesBundles.Add(dependencies, new DependenciesBundle(loadAssetBundles[dependencies].Bundle));
                        continue;
                    }
                }
                
                //将 asset bundle 加入加载中列表
                onLoadingAssetBundles.Add(relPath);

                //建立创建请求
                AssetBundleCreateRequest assetBundleDependencies;
                if (Disk.IsCrypt)
                {
                    IFile file = Disk.File(envPath + Path.AltDirectorySeparatorChar + dependencies, PathTypes.Absolute);
                    assetBundleDependencies = AssetBundle.LoadFromMemoryAsync(file.Read());
                }
                else
                {
                    assetBundleDependencies = AssetBundle.LoadFromFileAsync(envPath + Path.AltDirectorySeparatorChar + dependencies);
                }

                //等待请求完成
                yield return assetBundleDependencies;

                //在依赖包中增加请求的asset bundle
                if (assetBundleDependencies.assetBundle != null)
                {
                    dependenciesBundles.Remove(dependencies);
                    dependenciesBundles.Add(dependencies, new DependenciesBundle(assetBundleDependencies.assetBundle));
                }

                //将 asset bundle 从加载中列表移除
                onLoadingAssetBundles.Remove(relPath);

            }

        } 

        /// <summary>
        /// 解析文件路径
        /// </summary>
        protected void ParsePath(string path, out string relPath , out string objName)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            string dirPath = Path.GetDirectoryName(path);

            objName = name + extension;
            relPath = dirPath;
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
                    //如果被依赖的资源包被当作主包，那么就不移除只删除依赖
                    if (!loadAssetBundles.ContainsKey(assetbundlePath))
                    {
                        //防止assetbundle因为一些意外原因已经被释放了
                        if (dependenciesBundles[assetbundlePath].Bundle != null)
                        {
                            dependenciesBundles[assetbundlePath].Bundle.Unload(true);
                        }
                    }
                    dependenciesBundles.Remove(assetbundlePath);
                }
            }
        }

    }

}
