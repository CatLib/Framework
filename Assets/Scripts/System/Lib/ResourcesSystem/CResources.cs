using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Support;

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

        public override void Awake()
        {
            instance = this;
        }

        public T Load<T>(string path) where T : Object
        {
            this.LoadManifest();
            this.LoadAsset<GameObject>("prefab/asset5/dep-prefab");
            return default(T);
        }

        protected void LoadManifest()
        {

            if (assetBundleManifest != null) { return; }

            string manifestPath = CEnv.AssetPath + "/" + CEnv.PlatformToName(CEnv.SwitchPlatform);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(manifestPath);
            assetBundleManifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        }

        protected T LoadAsset<T>(string path) where T : Object
        {

            string objName = path.Substring(path.LastIndexOf('/') + 1);
            string relPath = path.Substring(0, path.LastIndexOf('/'));
            string assetPath = CEnv.AssetPath + "/" + relPath /*+ ".sd"*/;
            Debug.Log(path);
            foreach (string dependencies in assetBundleManifest.GetAllDependencies(relPath /*+ ".sd"*/))
            {
                Debug.Log(CEnv.AssetPath + "/" + dependencies);
                AssetBundle assetBundleDependencies = AssetBundle.LoadFromFile(CEnv.AssetPath + "/" + dependencies);
                assetBundleDependencies.LoadAllAssets();
            }

            AssetBundle assetTarget = AssetBundle.LoadFromFile(assetPath);

            T t = assetTarget.LoadAsset<T>(objName /*+ ".sd" */+ TypeToSuffix<T>());

            GameObject obj = GameObject.Instantiate(t as GameObject);
            Debug.Log(t);

            foreach (string s in assetTarget.GetAllAssetNames())
            {
                Debug.Log(s);
            }

            return default(T);


        }

        protected string TypeToSuffix<T>() where T : Object
        {
            System.Type t = typeof(T);
            if (t == typeof(GameObject))
                return ".prefab";
            return string.Empty;
        }



    }

}
