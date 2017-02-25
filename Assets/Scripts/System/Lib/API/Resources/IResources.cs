using UnityEngine;

namespace CatLib.API.Resources
{

    public interface IResources
    {

        void AddExtension(System.Type type, string extension);

        Object Load(string path);

        Object Load(string path , System.Type type);

        T Load<T>(string path) where T : Object;

        Object[] LoadAll(string path);

        T[] LoadAll<T>(string path) where T : Object;

        UnityEngine.Coroutine LoadAsync(string path, System.Action<Object> callback);

        UnityEngine.Coroutine LoadAsync(string path , System.Type type, System.Action<Object> callback);

        UnityEngine.Coroutine LoadAsync<T>(string path, System.Action<T> callback) where T : Object;

        UnityEngine.Coroutine LoadAllAsync(string path , System.Action<Object[]> callback);

        UnityEngine.Coroutine LoadAllAsync<T>(string path, System.Action<T[]> callback) where T : Object;

        void UnloadAll();

        void UnloadAssetBundle(string assetbundlePath);

    }

}