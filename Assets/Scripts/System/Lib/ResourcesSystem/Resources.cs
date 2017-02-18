using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CatLib.FileSystem;
using CatLib.Contracts.ResourcesSystem;

namespace CatLib.ResourcesSystem {

    public class Resources : Component , IResources
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

        #region Load

        public Object Load(string path){

            return Load(path , typeof(Object));

        }

        public Object Load(string path , System.Type type){

            return LoadAsset(path , type);

        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T Load<T>(string path) where T : Object
        {
            return LoadAsset(path , typeof(T)) as T;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        protected Object LoadAsset(string path , System.Type type)
        {
            this.LoadManifest();
            string relPath, objName , envPath;
            LoadPath(path , type , out relPath, out objName);

            #if UNITY_EDITOR
                if (Env.DebugLevel == Env.DebugLevels.DEV)
                {
                    return UnityEditor.AssetDatabase.LoadAssetAtPath("Assets" + Env.ResourcesBuildPath + "/" + relPath + "/" + objName , type);
                }
            #endif

            if (Env.DebugLevel == Env.DebugLevels.STAGING)
            {
                envPath = Env.DataPath + Env.ReleasePath + "/" + Env.PlatformToName(Env.SwitchPlatform);
            }
            else
            {
                envPath = Env.AssetPath;
            }

            AssetBundle assetTarget = LoadAssetBundle(envPath , relPath);
            return assetTarget.LoadAsset(objName , type);
        }

        #endregion

        #region LoadAll

        public Object[] LoadAll(string path){

            return LoadAll(path , typeof(Object));

        }

        public Object[] LoadAll(string path , System.Type type){

            return LoadAssetAll(path , type);

        }

        public T[] LoadAll<T>(string path) where T : Object
        {
            return LoadAssetAll(path , typeof(T)).To<T>();
        }

        protected Object[] LoadAssetAll(string path , System.Type type)
        {
            this.LoadManifest();
            string relPath, objName, envPath;
            LoadPath(path , type , out relPath, out objName);

            #if UNITY_EDITOR
            if (Env.DebugLevel == Env.DebugLevels.DEV)
            {
                return UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Assets" + Env.ResourcesBuildPath + "/" + relPath + "/" + objName);
            }
            #endif

            if (Env.DebugLevel == Env.DebugLevels.STAGING)
            {
                envPath = Env.DataPath + Env.ReleasePath + "/" + Env.PlatformToName(Env.SwitchPlatform);
            }
            else
            {
                envPath = Env.AssetPath;
            }
           
            AssetBundle assetTarget = LoadAssetBundle(envPath , relPath + "/" + System.IO.Path.GetFileNameWithoutExtension(objName));
            return assetTarget.LoadAllAssets();
        }

        #endregion

        #region LoadAsyn

        public UnityEngine.Coroutine LoadAsyn(string path, System.Action<Object> callback){

            return LoadAsyn(path , typeof(Object) , callback);

        }

        public UnityEngine.Coroutine LoadAsyn(string path , System.Type type, System.Action<Object> callback){

            return App.StartCoroutine(this.LoadAssetAsyn(path , type , callback));

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
            return App.StartCoroutine(this.LoadAssetAsyn(path , typeof(T) , (obj) => callback(obj as T)));

        }

        /// <summary>
        /// 加载资源（异步） 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        protected IEnumerator LoadAssetAsyn(string path , System.Type type , System.Action<Object> callback)
        {

            this.LoadManifest();
            string relPath, objName , envPath;
            LoadPath(path , type, out relPath, out objName);

            #if UNITY_EDITOR
            if (Env.DebugLevel == Env.DebugLevels.DEV)
            {
                callback.Invoke(UnityEditor.AssetDatabase.LoadAssetAtPath("Assets" + Env.ResourcesBuildPath + "/" + relPath + "/" + objName , type));
                yield break;
            }
            #endif

            if (Env.DebugLevel == Env.DebugLevels.STAGING)
            {
                envPath = Env.DataPath + Env.ReleasePath;
            }
            else
            {
                envPath = Env.AssetPath;
            }

            AssetBundle assetTarget = null;

            yield return LoadAssetBundleAsyn(envPath , relPath, (ab) =>
            {
                assetTarget = ab;
            });

            AssetBundleRequest targetAssetRequest = assetTarget.LoadAssetAsync(objName, type);
            yield return targetAssetRequest;

            callback.Invoke(targetAssetRequest.asset);

        }

        #endregion

        #region LoadAllAsyn

        public UnityEngine.Coroutine LoadAllAsyn(string path, System.Action<Object[]> callback){

            return LoadAllAsyn(path , typeof(Object) , callback);

        }

        public UnityEngine.Coroutine LoadAllAsyn(string path , System.Type type, System.Action<Object[]> callback){

            return App.StartCoroutine(this.LoadAssetAllAsyn(path , type , callback));

        }

        public UnityEngine.Coroutine LoadAllAsyn<T>(string path, System.Action<T[]> callback) where T : Object
        {
            return App.StartCoroutine(this.LoadAssetAllAsyn(path , typeof(T), (obj) => callback(obj.To<T>())));
        }

        protected IEnumerator LoadAssetAllAsyn(string path , System.Type type, System.Action<Object[]> callback)
        {

            this.LoadManifest();
            string relPath, objName , envPath;
            LoadPath(path , type, out relPath, out objName);

            #if UNITY_EDITOR
                if (Env.DebugLevel == Env.DebugLevels.DEV)
                {
                    callback.Invoke(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Assets" + Env.ResourcesBuildPath + "/" + relPath + "/" + objName));
                    yield break;
                }
            #endif

            if (Env.DebugLevel == Env.DebugLevels.STAGING)
            {
                envPath = Env.DataPath + Env.ReleasePath + "/" + Env.PlatformToName(Env.SwitchPlatform);
            }
            else
            {
                envPath = Env.AssetPath;
            }

            AssetBundle assetTarget = null;

            yield return LoadAssetBundleAsyn(envPath , relPath + "/" + System.IO.Path.GetFileNameWithoutExtension(objName), (ab) =>
            {
                assetTarget = ab;
            });

            AssetBundleRequest targetAssetRequest = assetTarget.LoadAllAssetsAsync();
            yield return targetAssetRequest;
            
            callback.Invoke(targetAssetRequest.allAssets);

        }

        #endregion

        /// <summary>
        /// 加载依赖文件
        /// </summary>
        protected void LoadManifest()
        {
            if (assetBundleManifest != null) { return; }

            #if UNITY_EDITOR
            if (Env.DebugLevel == Env.DebugLevels.DEV)
            {
                return;
            }
            #endif

            string envPath;
            if (Env.DebugLevel == Env.DebugLevels.STAGING)
            {
                envPath = Env.DataPath + Env.ReleasePath + "/" + Env.PlatformToName(Env.SwitchPlatform);
            }
            else
            {
                envPath = Env.AssetPath;
            }

            string manifestPath = envPath + "/" + Env.PlatformToName(Env.SwitchPlatform);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(manifestPath);
            assetBundleManifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        protected void LoadPath(string path , System.Type type , out string relPath , out string objName)
        {
            string variant = string.IsNullOrEmpty(Variant) ? string.Empty : "." + Variant;
            objName = NameWithTypeToSuffix(path.Substring(path.LastIndexOf('/') + 1), variant , type);
            relPath = path.Substring(0, path.LastIndexOf('/')) + variant;

            if (!CFile.Exists(Env.AssetPath + "/" + relPath))
            {
                objName = NameWithTypeToSuffix(path.Substring(path.LastIndexOf('/') + 1), string.Empty , type);
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
        protected string NameWithTypeToSuffix(string name , string variant , System.Type type)
        {
            string suffix = string.Empty;
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
            }
           
            if (name.Length < suffix.Length || (name.Length == suffix.Length && name != suffix) || (name.Length > suffix.Length && name.Substring(name.Length - suffix.Length) != suffix))
            {
                name += suffix;
            }
            return name + variant;
        }

    }

}
