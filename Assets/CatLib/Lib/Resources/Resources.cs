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
 
using CatLib.API;
using CatLib.API.IO;
using CatLib.API.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace CatLib.Resources {

    public class Resources : IResources
    {

        [Dependency]
        public AssetBundleLoader assetBundleLoader { get; set; }

        public IResourcesHosted resourcesHosted { get; set; }

        [Dependency]
        public IIOFactory IO { get; set; }

        [Dependency]
        public IEnv Env { get; set; }

        [Dependency]
        public IApplication App { get; set; }

        public void SetHosted(IResourcesHosted hosted){

            resourcesHosted = hosted;

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

        public IObjectInfo Load(string path , LoadTypes loadType = LoadTypes.AssetBundle)
        {
            return Load(path , typeof(Object) , loadType);
        }

        public IObjectInfo Load<T>(string path , LoadTypes loadType = LoadTypes.AssetBundle) where T : Object
        {
            return Load(path, typeof(T) , loadType);
        }

        public IObjectInfo Load(string path, System.Type type , LoadTypes loadType = LoadTypes.AssetBundle)
        {

            if(loadType == LoadTypes.Resources){

                return MakeDefaultObjectInfo(UnityEngine.Resources.Load(path , type));

            }

            path = PathFormat(path, type);

            #if UNITY_EDITOR
            if (Env.DebugLevel == DebugLevels.Auto || Env.DebugLevel == DebugLevels.Dev)
            {
                return MakeDefaultObjectInfo(UnityEditor.AssetDatabase.LoadAssetAtPath("Assets" + Env.ResourcesBuildPath + Path.AltDirectorySeparatorChar + path, type));
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
                hosted = MakeDefaultObjectInfo(obj);
            }
            return hosted;
        }

        public IObjectInfo[] LoadAll<T>(string path , LoadTypes loadType = LoadTypes.AssetBundle) where T : Object
        {
            return LoadAll(path , loadType);
        }

        public IObjectInfo[] LoadAll(string path , LoadTypes loadType = LoadTypes.AssetBundle)
        {

            if(loadType == LoadTypes.Resources){

                return MakeDefaultObjectInfos(UnityEngine.Resources.LoadAll(path));

            }

            #if UNITY_EDITOR
                if (Env.DebugLevel == DebugLevels.Auto || Env.DebugLevel == DebugLevels.Dev)
                {
                    throw new System.Exception("not support [LoadAll] in auto env");
                }
            #endif
            
            Object[] objs = assetBundleLoader.LoadAssetAll(path);
            return MakeDefaultObjectInfos(objs);
        }

        public UnityEngine.Coroutine LoadAsync(string path, System.Action<IObjectInfo> callback , LoadTypes loadType = LoadTypes.AssetBundle)
        {
            return LoadAsync(path , typeof(Object), callback ,loadType);
        }

        public UnityEngine.Coroutine LoadAsync<T>(string path, System.Action<IObjectInfo> callback , LoadTypes loadType = LoadTypes.AssetBundle) where T : Object
        {
            return LoadAsync(path, typeof(T), obj => callback(obj) , loadType);
        }

        public UnityEngine.Coroutine LoadAsync(string path, System.Type type, System.Action<IObjectInfo> callback , LoadTypes loadType = LoadTypes.AssetBundle)
        {

            if(loadType == LoadTypes.Resources){

                return App.StartCoroutine(LoadAsyncWithUnityResources(path ,type , callback));

            }

            path = PathFormat(path, type); 
            
            #if UNITY_EDITOR
                if (Env.DebugLevel == DebugLevels.Auto || Env.DebugLevel == DebugLevels.Dev)
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
                    hosted = MakeDefaultObjectInfo(obj);
                }
                callback(hosted);
            }); 
        }

        public UnityEngine.Coroutine LoadAllAsync<T>(string path, System.Action<IObjectInfo[]> callback , LoadTypes loadType = LoadTypes.AssetBundle) where T : Object
        {
            return LoadAllAsync(path, obj => callback(obj) , loadType);
        }

        public UnityEngine.Coroutine LoadAllAsync(string path, System.Action<IObjectInfo[]> callback , LoadTypes loadType = LoadTypes.AssetBundle)
        {
            if(loadType == LoadTypes.Resources){

                return App.StartCoroutine(LoadAllAsyncWithUnityResources(path , callback));

            }
            #if UNITY_EDITOR
                if (Env.DebugLevel == DebugLevels.Auto || Env.DebugLevel == DebugLevels.Dev)
                {
                    throw new System.Exception("not support [LoadAllAsync] in auto env");
                }
            #endif
            return assetBundleLoader.LoadAssetAllAsync(path, (objs)=>
            {
                callback(MakeDefaultObjectInfos(objs));
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

        protected IEnumerator LoadAsyncWithUnityResources(string path, System.Type type, System.Action<IObjectInfo> callback){

            ResourceRequest request = UnityEngine.Resources.LoadAsync(path , type);
            yield return request;

            callback(MakeDefaultObjectInfo(request.asset));

        }

        protected IEnumerator LoadAllAsyncWithUnityResources(string path, System.Action<IObjectInfo[]> callback){

            Object[] objs = UnityEngine.Resources.LoadAll(path);
            callback(MakeDefaultObjectInfos(objs));
            yield break;

        }

        protected IObjectInfo MakeDefaultObjectInfo(Object obj){

            return new DefaultObjectInfo(obj);

        }

        protected IObjectInfo[] MakeDefaultObjectInfos(Object[] objs){

            IObjectInfo[] hosted = new IObjectInfo[objs.Length];
            for(int i = 0; i < objs.Length; i++)
            {
                hosted[i] = MakeDefaultObjectInfo(objs[i]);
            }

            return hosted;
        }

    }

}
