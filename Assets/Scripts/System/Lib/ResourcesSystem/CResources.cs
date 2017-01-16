using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Support;
using System.Collections.Generic;

namespace CatLib.ResourcesSystem {

    public class CResources : CManagerBase
    {

        protected static CResources instance;

        public static CResources Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new CNullReferenceException("resources manager not instance");
                }
                return instance;
            }
        }

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


        public override void Awake()
        {
            instance = this;
        }

        public Coroutine LoadAsyn<T>(string path , System.Action<T> callback) where T : Object
        {
            this.LoadManifest();
            return base.StartCoroutine(this.LoadAssetAsyn<T>(path , callback));

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

            string variant = string.IsNullOrEmpty(Variant) ? string.Empty : "." + Variant;
            string objName = NameWithTypeToSuffix<T>(path.Substring(path.LastIndexOf('/') + 1), variant);
            string relPath = path.Substring(0, path.LastIndexOf('/')) + variant;

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
                loadAssetBundle.Add(relPath, assetTargetBundleRequest.assetBundle);
            }else
            {
                assetTarget = loadAssetBundle[relPath];
            }

            AssetBundleRequest targetAssetRequest = assetTarget.LoadAssetAsync<T>(objName);
            yield return targetAssetRequest;

            T targetAsset = targetAssetRequest.asset as T;

            callback.Invoke(targetAsset);

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
