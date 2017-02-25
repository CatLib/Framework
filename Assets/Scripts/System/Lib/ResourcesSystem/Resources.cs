using CatLib.API.ResourcesSystem;
using UnityEngine;

namespace CatLib.ResourcesSystem {

    public class Resources : Component , IResources
    {

        [Dependency]
        public AssetBundleLoader assetBundleLoader { get; set; }

        public Object Load(string path)
        {
            return assetBundleLoader.LoadAsset(path);
        }

        public Object Load(string path, System.Type type)
        {
            return Load(path);
        }

        public T Load<T>(string path) where T : Object
        {
            return Load(path) as T;
        }

        public Object[] LoadAll(string path)
        {
            return assetBundleLoader.LoadAssetAll(path);
        }

        public Object[] LoadAll(string path, System.Type type)
        {
            return LoadAll(path);
        }

        public T[] LoadAll<T>(string path) where T : Object
        {
            return LoadAll(path).To<T>();
        }

        public UnityEngine.Coroutine LoadAsync(string path, System.Action<Object> callback)
        {
            return assetBundleLoader.LoadAssetAsync(path, callback);
        }

        public UnityEngine.Coroutine LoadAsync(string path, System.Type type, System.Action<Object> callback)
        {
            return LoadAsync(path, callback);
        }

        public UnityEngine.Coroutine LoadAsync<T>(string path, System.Action<T> callback) where T : Object
        {
            return LoadAsync(path, obj => callback(obj as T) );
        }

        public UnityEngine.Coroutine LoadAllAsync(string path, System.Action<Object[]> callback)
        {
            return assetBundleLoader.LoadAssetAllAsync(path, callback);
        }

        public UnityEngine.Coroutine LoadAllAsync(string path, System.Type type, System.Action<Object[]> callback)
        {
            return LoadAllAsync(path, callback);
        }

        public UnityEngine.Coroutine LoadAllAsync<T>(string path, System.Action<T[]> callback) where T : Object
        {
            return LoadAllAsync(path, obj => callback(obj.To<T>()));
        }

        public void UnloadAll()
        {
            assetBundleLoader.UnloadAll();
        }

        public void UnloadAssetBundle(string assetbundlePath)
        {
            assetBundleLoader.UnloadAssetBundle(assetbundlePath);
        }


        /* 
            #if UNITY_EDITOR
                if (Env.DebugLevel == Env.DebugLevels.AUTO)
                {
                    return UnityEditor.AssetDatabase.LoadAssetAtPath("Assets" + Env.ResourcesBuildPath + IO.PathSpliter + relPath + IO.PathSpliter + objName , type);
                }
            #endif

            if (Env.DebugLevel == Env.DebugLevels.STAGING)
            {
                envPath = Env.DataPath + Env.ReleasePath + IO.PathSpliter + Env.PlatformToName(Env.SwitchPlatform);
            }
            else
            {
                envPath = Env.AssetPath;
            }*/
    }

}
