using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CatLib.API.IO;
using CatLib.API.ResourcesSystem;

namespace CatLib.ResourcesSystem {

    public class AssetBundleLoader : Component , IAssetBundle
    {

        [Dependency]
        public IIO IO { get; set; }

        /// <summary>
        /// 主依赖文件
        /// </summary>
        protected UnityEngine.AssetBundleManifest assetBundleManifest;

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
            string relPath, objName , envPath;
            ParsePath(path , out relPath, out objName);

            envPath = Env.AssetPath;

            UnityEngine.AssetBundle assetTarget = LoadAssetBundle(envPath , relPath);
            return assetTarget.LoadAsset(objName);
        }


        public Object[] LoadAssetAll(string path)
        {
            LoadManifest();
            string relPath, objName, envPath;
            ParsePath(path , out relPath, out objName);

            #if UNITY_EDITOR
            if (Env.DebugLevel == Env.DebugLevels.AUTO)
            {
                return UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Assets" + Env.ResourcesBuildPath + IO.PathSpliter + relPath + IO.PathSpliter + objName);
            }
            #endif

            if (Env.DebugLevel == Env.DebugLevels.STAGING)
            {
                envPath = Env.DataPath + Env.ReleasePath + IO.PathSpliter + Env.PlatformToName(Env.SwitchPlatform);
            }
            else
            {
                envPath = Env.AssetPath;
            }
           
            UnityEngine.AssetBundle assetTarget = LoadAssetBundle(envPath , relPath + IO.PathSpliter + System.IO.Path.GetFileNameWithoutExtension(objName));
            return assetTarget.LoadAllAssets();
        }

        /// <summary>
        /// 加载资源（异步） 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator LoadAssetAsync(string path , System.Action<Object> callback)
        {

            LoadManifest();
            string relPath, objName , envPath;
            ParsePath(path, out relPath, out objName);

            envPath = Env.AssetPath;

            UnityEngine.AssetBundle assetTarget = null;

            yield return LoadAssetBundleAsync(envPath , relPath, (ab) =>
            {
                assetTarget = ab;
            });

            AssetBundleRequest targetAssetRequest = assetTarget.LoadAssetAsync(objName);
            yield return targetAssetRequest;

            callback.Invoke(targetAssetRequest.asset);

        }

        public IEnumerator LoadAssetAllAsync(string path , System.Action<Object[]> callback)
        {

            this.LoadManifest();
            string relPath, objName , envPath;
            ParsePath(path, out relPath, out objName);

            envPath = Env.AssetPath;

            UnityEngine.AssetBundle assetTarget = null;

            yield return LoadAssetBundleAsync(envPath , relPath + IO.PathSpliter + System.IO.Path.GetFileNameWithoutExtension(objName), (ab) =>
            {
                assetTarget = ab;
            });

            if(assetTarget == null){

                callback.Invoke(new Object[]{});
                yield break;

            }

            AssetBundleRequest targetAssetRequest = assetTarget.LoadAllAssetsAsync();
            yield return targetAssetRequest;
            
            callback.Invoke(targetAssetRequest.allAssets);

        }


        /// <summary>
        /// 加载依赖文件
        /// </summary>
        protected void LoadManifest()
        {
            if (assetBundleManifest != null) { return; }

            #if UNITY_EDITOR
            if (Env.DebugLevel == Env.DebugLevels.AUTO)
            {
                return;
            }
            #endif

            string envPath = Env.AssetPath;

            string manifestPath = envPath + IO.PathSpliter + Env.PlatformToName(Env.SwitchPlatform);
            UnityEngine.AssetBundle assetBundle = UnityEngine.AssetBundle.LoadFromFile(manifestPath);
            assetBundleManifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        protected UnityEngine.AssetBundle LoadAssetBundle(string envPath , string relPath)
        {
            
            LoadDependenciesAssetBundle(envPath , relPath);

            UnityEngine.AssetBundle assetTarget;
            if (!loadAssetBundles.ContainsKey(relPath))
            {
                assetTarget = UnityEngine.AssetBundle.LoadFromFile(envPath + IO.PathSpliter + relPath);
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

            foreach (string dependencies in assetBundleManifest.GetAllDependencies(relPath))
            {
                if (!dependenciesBundles.ContainsKey(dependencies))
                {

                    UnityEngine.AssetBundle assetBundle = UnityEngine.AssetBundle.LoadFromFile(envPath + IO.PathSpliter + dependencies);
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
        protected IEnumerator LoadAssetBundleAsync(string envPath , string relPath , System.Action<UnityEngine.AssetBundle> complete) 
        {

            yield return LoadDependenciesAssetBundleAsync(envPath , relPath);

            UnityEngine.AssetBundle assetTarget;
            if (!loadAssetBundle.ContainsKey(relPath))
            {
                AssetBundleCreateRequest assetTargetBundleRequest = UnityEngine.AssetBundle.LoadFromFileAsync(envPath + IO.PathSpliter + relPath);
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
                if (!loadAssetBundle.ContainsKey(dependencies))
                {
                    AssetBundleCreateRequest assetBundleDependencies = UnityEngine.AssetBundle.LoadFromFileAsync(envPath + IO.PathSpliter + dependencies);
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

            









            objName = NameWithTypeToSuffix(path.Substring(path.LastIndexOf(IO.PathSpliter) + 1), variant );
            relPath = path.Substring(0, path.LastIndexOf(IO.PathSpliter)) + variant;

            if (!IO.File(Env.AssetPath + IO.PathSpliter + relPath).Exists)
            {
                objName = NameWithTypeToSuffix(path.Substring(path.LastIndexOf(IO.PathSpliter) + 1), string.Empty );
                relPath = path.Substring(0, path.LastIndexOf(IO.PathSpliter));
            }
        }

        /// <summary>
        /// 类型对应的后缀
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected string NameWithTypeToSuffix(string name , string variant )
        {
            string suffix = string.Empty;
            /*
            if(type == typeof(Object)){

                if(name.LastIndexOf('.') == -1){
                    
                    suffix = ".prefab";

                }

            }else if (type == typeof(GameObject))
            {
                suffix = ".prefab";
            }else if(type == typeof(TextAsset)){

                suffix = ".txt";
                   
            }else if(type == typeof(Material))
            {
                suffix = ".mat";
            }else if(type == typeof(Shader))
            {
                suffix = ".shader";
            }*/
           
            if (name.Length < suffix.Length || (name.Length == suffix.Length && name != suffix) || (name.Length > suffix.Length && name.Substring(name.Length - suffix.Length) != suffix))
            {
                name += suffix;
            }
            return name + variant;
        }





        //一下是需要调整的代码

        /// <summary>
        /// 被加载的资源包
        /// </summary>
        protected Dictionary<string , UnityEngine.AssetBundle> loadAssetBundle = new Dictionary<string, UnityEngine.AssetBundle>();

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="unloadAllLoadedObjects">是否释放已经被加载的Asset资源</param>
        public void Unload(bool unloadAllLoadedObjects)
        {
            foreach(UnityEngine.AssetBundle bundle in loadAssetBundle.ToArray())
            {
                bundle.Unload(unloadAllLoadedObjects);
            }
        }

    }

}
