using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Support;
using System.Collections.Generic;
using CatLib.FileSystem;
using CatLib.Contracts.ResourcesSystem;
using CatLib.Support;

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

        public Object Load(string path){

            return Load<Object>(path);

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

        public Object[] LoadAll(string path){

            return LoadAll<Object>(path);

        }

        public T[] LoadAll<T>(string path ) where T : Object
        {
            this.LoadManifest();
            string relPath, objName, envPath;
            LoadPath<T>(path, out relPath, out objName);

            #if UNITY_EDITOR
            if (CEnv.DebugLevel == CEnv.DebugLevels.DEV)
            {
                return UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Assets" + CEnv.ResourcesBuildPath + "/" + relPath + "/" + objName).To<T>();
            }
            #endif

            if (CEnv.DebugLevel == CEnv.DebugLevels.STAGING)
            {
                envPath = CEnv.DataPath + CEnv.ReleasePath + "/" + CEnv.PlatformToName(CEnv.SwitchPlatform);
            }
            else
            {
                envPath = CEnv.AssetPath;
            }
           
            AssetBundle assetTarget = LoadAssetBundle(envPath , relPath + "/" + objName);
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
           
            string relPath, objName , envPath;
            LoadPath<T>(path, out relPath, out objName);

            #if UNITY_EDITOR
                if (CEnv.DebugLevel == CEnv.DebugLevels.DEV)
                {
                    return UnityEditor.AssetDatabase.LoadAssetAtPath<T>("Assets" + CEnv.ResourcesBuildPath + "/" + relPath + "/" + objName);
                }
            #endif

            if (CEnv.DebugLevel == CEnv.DebugLevels.STAGING)
            {
                envPath = CEnv.DataPath + CEnv.ReleasePath + "/" + CEnv.PlatformToName(CEnv.SwitchPlatform);
            }
            else
            {
                envPath = CEnv.AssetPath;
            }

            AssetBundle assetTarget = LoadAssetBundle(envPath , relPath);
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

        protected AssetBundle LoadAssetBundle(string envPath , string relPath)
        {
            foreach (string dependencies in assetBundleManifest.GetAllDependencies(relPath))
            {
                if (!loadAssetBundle.ContainsKey(dependencies))
                {
                    AssetBundle assetBundle = AssetBundle.LoadFromFile(envPath + "/" + dependencies);
                    loadAssetBundle.Add(dependencies, assetBundle);
                }
            }

            AssetBundle assetTarget;
            if (!loadAssetBundle.ContainsKey(relPath))
            {
                assetTarget = AssetBundle.LoadFromFile(envPath + "/" + relPath);
                loadAssetBundle.Add(relPath, assetTarget);
            }
            else
            {
                assetTarget = loadAssetBundle[relPath];
            }

            return assetTarget;

        }

        public UnityEngine.Coroutine LoadAsyn(string path, System.Action<Object> callback){

            return LoadAsyn<Object>(path , callback);

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

        public UnityEngine.Coroutine LoadAllAsyn(string path, System.Action<Object[]> callback){

            return LoadAllAsyn<Object>(path , callback);

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

            #if UNITY_EDITOR
            if (CEnv.DebugLevel == CEnv.DebugLevels.DEV)
            {
                return;
            }
            #endif

            string envPath;
            if (CEnv.DebugLevel == CEnv.DebugLevels.STAGING)
            {
                envPath = CEnv.DataPath + CEnv.ReleasePath + "/" + CEnv.PlatformToName(CEnv.SwitchPlatform);
            }
            else
            {
                envPath = CEnv.AssetPath;
            }

            string manifestPath = envPath + "/" + CEnv.PlatformToName(CEnv.SwitchPlatform);
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

            string relPath, objName , envPath;
            LoadPath<T>(path, out relPath, out objName);


            #if UNITY_EDITOR
            if (CEnv.DebugLevel == CEnv.DebugLevels.DEV)
            {
                callback.Invoke(UnityEditor.AssetDatabase.LoadAssetAtPath<T>("Assets" + CEnv.ResourcesBuildPath + "/" + relPath + "/" + objName));
                yield break;
            }
            #endif

            if (CEnv.DebugLevel == CEnv.DebugLevels.STAGING)
            {
                Debug.Log(CEnv.DataPath + CEnv.ReleasePath + "/" + CEnv.PlatformToName(CEnv.SwitchPlatform));
                envPath = CEnv.DataPath + CEnv.ReleasePath;
            }
            else
            {
                envPath = CEnv.AssetPath;
            }


            AssetBundle assetTarget = null;

            yield return LoadAssetBundleAsyn(envPath , relPath, (ab) =>
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

            string relPath, objName , envPath;
            LoadPath<T>(path, out relPath, out objName);

            #if UNITY_EDITOR
                if (CEnv.DebugLevel == CEnv.DebugLevels.DEV)
                {
                    callback.Invoke(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Assets" + CEnv.ResourcesBuildPath + "/" + relPath + "/" + objName).To<T>());
                    yield break;
                }
            #endif

            if (CEnv.DebugLevel == CEnv.DebugLevels.STAGING)
            {
                envPath = CEnv.DataPath + CEnv.ReleasePath + "/" + CEnv.PlatformToName(CEnv.SwitchPlatform);
            }
            else
            {
                envPath = CEnv.AssetPath;
            }

            AssetBundle assetTarget = null;

            yield return LoadAssetBundleAsyn(envPath , relPath + "/" + objName, (ab) =>
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
        protected IEnumerator LoadAssetBundleAsyn(string envPath , string relPath , System.Action<AssetBundle> assetBundle) 
        {

            foreach (string dependencies in assetBundleManifest.GetAllDependencies(relPath))
            {
                if (!loadAssetBundle.ContainsKey(dependencies))
                {
                    AssetBundleCreateRequest assetBundleDependencies = AssetBundle.LoadFromFileAsync(envPath + "/" + dependencies);
                    yield return assetBundleDependencies;
                    loadAssetBundle.Add(dependencies, assetBundleDependencies.assetBundle);
                }
            }

            AssetBundle assetTarget;
            if (!loadAssetBundle.ContainsKey(relPath))
            {
                AssetBundleCreateRequest assetTargetBundleRequest = AssetBundle.LoadFromFileAsync(envPath + "/" + relPath);
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
