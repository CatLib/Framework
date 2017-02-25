using CatLib.API.IO;
using CatLib.API.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Resources {

    public class Resources : Component , IResources
    {

        [Dependency]
        public AssetBundleLoader assetBundleLoader { get; set; }

        [Dependency]
        public IIO IO { get; set; }

        private Dictionary<System.Type, string> extensionDict = new Dictionary<System.Type, string>();

        public Resources()
        {
            extensionDict.Add(typeof(Object), ".prefab");
            extensionDict.Add(typeof(GameObject), ".prefab");
            extensionDict.Add(typeof(TextAsset), ".txt");
            extensionDict.Add(typeof(Material), ".mat");
            extensionDict.Add(typeof(Shader), ".shader");
        }

        public void AddExtension(System.Type type, string extension)
        {
            extensionDict.Remove(type);
            extensionDict.Add(type, extension);
        }

        public Object Load(string path)
        {
            return Load(path , typeof(Object));
        }

        public T Load<T>(string path) where T : Object
        {
            return Load(path, typeof(T)) as T;
        }

        public Object Load(string path, System.Type type)
        {
            path = PathFormat(path, type);
            #if UNITY_EDITOR
            if (Env.DebugLevel == Env.DebugLevels.AUTO)
            {
                return UnityEditor.AssetDatabase.LoadAssetAtPath("Assets" + Env.ResourcesBuildPath + IO.PathSpliter + path, type);
            }
            #endif
            return assetBundleLoader.LoadAsset(path);
        }

        public T[] LoadAll<T>(string path) where T : Object
        {
            return LoadAll(path).To<T>();
        }

        public Object[] LoadAll(string path)
        {
            #if UNITY_EDITOR
                if (Env.DebugLevel == Env.DebugLevels.AUTO)
                {
                    throw new System.Exception("not support [LoadAll] in auto env");
                }
            #endif
            return assetBundleLoader.LoadAssetAll(path);
        }

        public UnityEngine.Coroutine LoadAsync(string path, System.Action<Object> callback)
        {
            return LoadAsync(path , typeof(Object), callback);
        }

        public UnityEngine.Coroutine LoadAsync<T>(string path, System.Action<T> callback) where T : Object
        {
            return LoadAsync(path, typeof(T), obj => callback(obj as T));
        }

        public UnityEngine.Coroutine LoadAsync(string path, System.Type type, System.Action<Object> callback)
        {
            path = PathFormat(path, type);
            #if UNITY_EDITOR
                if (Env.DebugLevel == Env.DebugLevels.AUTO)
                {
                    return App.StartCoroutine(EmptyIEnumerator(() =>
                    {
                        callback(UnityEditor.AssetDatabase.LoadAssetAtPath("Assets" + Env.ResourcesBuildPath + IO.PathSpliter + path, type));
                    }));
                }
            #endif
            return assetBundleLoader.LoadAssetAsync(PathFormat(path , type), callback); 
        }

        public UnityEngine.Coroutine LoadAllAsync<T>(string path, System.Action<T[]> callback) where T : Object
        {
            return LoadAllAsync(path, obj => callback(obj.To<T>()));
        }

        public UnityEngine.Coroutine LoadAllAsync(string path, System.Action<Object[]> callback)
        {

            #if UNITY_EDITOR
                if (Env.DebugLevel == Env.DebugLevels.AUTO)
                {
                    throw new System.Exception("not support [LoadAllAsync] in auto env");
                }
            #endif
            return assetBundleLoader.LoadAssetAllAsync(path, callback);
        }

        public void UnloadAll()
        {
            assetBundleLoader.UnloadAll();
        }

        public void UnloadAssetBundle(string assetbundlePath)
        {
            assetBundleLoader.UnloadAssetBundle(assetbundlePath);
        }

        protected IEnumerator EmptyIEnumerator(System.Action callback)
        {
            callback.Invoke();
            yield break;
        }

        protected string PathFormat(string path , System.Type type)
        {

            string extension = System.IO.Path.GetExtension(path);
            if (extension != string.Empty) { return path; }

            if (extensionDict.ContainsKey(type))
            {
                return path + extensionDict[type];
            }

            return path;

        }

    }

}
