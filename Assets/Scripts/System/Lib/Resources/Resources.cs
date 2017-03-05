using CatLib.API;
using CatLib.API.IO;
using CatLib.API.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace CatLib.Resources {

    public class Resources : Component , IResources
    {

        [Dependency]
        public AssetBundleLoader assetBundleLoader { get; set; }

        public IResourcesHosted resourcesHosted { get; set; }

        [Dependency]
        public IIOFactory IO { get; set; }

        [Dependency]
        public IEnv Env { get; set; }

        private Configs config;

        [Dependency]
        public Configs Config
        {
            get
            {
                return config;
            }set
            {
                config = value;
                if (config != null)
                {
                    string hosted = config.Get<object>("hosted").ToString();
                    if (!string.IsNullOrEmpty(hosted))
                    {
                        resourcesHosted = App.Make<IResourcesHosted>(hosted);
                    }else
                    {
                        resourcesHosted = null;
                    }
                }else
                {
                    resourcesHosted = App.Make<IResourcesHosted>(typeof(IResourcesHosted).ToString());
                }
            }
        }

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

        public IObjectInfo Load(string path)
        {
            return Load(path , typeof(Object));
        }

        public IObjectInfo Load<T>(string path) where T : Object
        {
            return Load(path, typeof(T));
        }

        public IObjectInfo Load(string path, System.Type type)
        {
            path = PathFormat(path, type);
            #if UNITY_EDITOR
            if (Env.DebugLevel == DebugLevels.Auto)
            {
                return new DefaultObjectInfo(UnityEditor.AssetDatabase.LoadAssetAtPath("Assets" + Env.ResourcesBuildPath + Path.AltDirectorySeparatorChar + path, type));
            }
            #endif

            IObjectInfo hosted;
            if (resourcesHosted != null)
            {
                hosted = resourcesHosted.Get(path);
                if (hosted != null) { return hosted; }
            }

            Object obj = assetBundleLoader.LoadAsset(path);

            if (resourcesHosted != null)
            {
                hosted = resourcesHosted.Hosted(path, obj);
            }
            else
            {
                hosted = new DefaultObjectInfo(obj);
            }
            return hosted;
        }

        public IObjectInfo[] LoadAll<T>(string path) where T : Object
        {
            return LoadAll(path);
        }

        public IObjectInfo[] LoadAll(string path)
        {
            #if UNITY_EDITOR
                if (Env.DebugLevel == DebugLevels.Auto)
                {
                    throw new System.Exception("not support [LoadAll] in auto env");
                }
            #endif
            
            Object[] objs = assetBundleLoader.LoadAssetAll(path);
            IObjectInfo[] hosted = new IObjectInfo[objs.Length];
            for(int i = 0; i < objs.Length; i++)
            {
                hosted[i] = new DefaultObjectInfo(objs[i]);
            }

            return hosted;
        }

        public UnityEngine.Coroutine LoadAsync(string path, System.Action<IObjectInfo> callback)
        {
            return LoadAsync(path , typeof(Object), callback);
        }

        public UnityEngine.Coroutine LoadAsync<T>(string path, System.Action<IObjectInfo> callback) where T : Object
        {
            return LoadAsync(path, typeof(T), obj => callback(obj));
        }

        public UnityEngine.Coroutine LoadAsync(string path, System.Type type, System.Action<IObjectInfo> callback)
        {
            path = PathFormat(path, type);
            #if UNITY_EDITOR
                if (Env.DebugLevel == DebugLevels.Auto)
                {
                    return App.StartCoroutine(EmptyIEnumerator(() =>
                    {
                        callback(new DefaultObjectInfo(UnityEditor.AssetDatabase.LoadAssetAtPath("Assets" + Env.ResourcesBuildPath + Path.AltDirectorySeparatorChar + path, type)));
                    }));
                }
            #endif

            IObjectInfo hosted;
            if (resourcesHosted != null)
            {
                hosted = resourcesHosted.Get(path);
                if (hosted != null)
                {
                    return App.StartCoroutine(EmptyIEnumerator(() =>{ callback(hosted); }));
                }
            }

            return assetBundleLoader.LoadAssetAsync(PathFormat(path , type), (obj)=>
            {
                if (resourcesHosted != null)
                {
                    hosted = resourcesHosted.Hosted(path, obj);
                }
                else
                {
                    hosted = new DefaultObjectInfo(obj);
                }
                callback(hosted);
            }); 
        }

        public UnityEngine.Coroutine LoadAllAsync<T>(string path, System.Action<IObjectInfo[]> callback) where T : Object
        {
            return LoadAllAsync(path, obj => callback(obj));
        }

        public UnityEngine.Coroutine LoadAllAsync(string path, System.Action<IObjectInfo[]> callback)
        {

            #if UNITY_EDITOR
                if (Env.DebugLevel == DebugLevels.Auto)
                {
                    throw new System.Exception("not support [LoadAllAsync] in auto env");
                }
            #endif
            return assetBundleLoader.LoadAssetAllAsync(path, (objs)=>
            {
                IObjectInfo[] hosted = new IObjectInfo[objs.Length];
                for (int i = 0; i < objs.Length; i++)
                {
                    hosted[i] = new DefaultObjectInfo(objs[i]);
                }
                callback(hosted);
            });
        }

        protected IEnumerator EmptyIEnumerator(System.Action callback)
        {
            callback.Invoke();
            yield break;
        }

        protected string PathFormat(string path , System.Type type)
        {

            string extension = Path.GetExtension(path);
            if (extension != string.Empty) { return path; }

            if (extensionDict.ContainsKey(type))
            {
                return path + extensionDict[type];
            }

            return path;

        }

    }

}
