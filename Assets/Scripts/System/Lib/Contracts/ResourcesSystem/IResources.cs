using UnityEngine;

namespace CatLib.Contracts.ResourcesSystem
{

    public interface IResources
    {

        Object Load(string path);

        Object Load(string path , System.Type type);

        T Load<T>(string path) where T : Object;

        Object[] LoadAll(string path);

        Object[] LoadAll(string path , System.Type type);

        T[] LoadAll<T>(string path) where T : Object;

        UnityEngine.Coroutine LoadAsyn(string path, System.Action<Object> callback);

        UnityEngine.Coroutine LoadAsyn(string path , System.Type type, System.Action<Object> callback);

        UnityEngine.Coroutine LoadAsyn<T>(string path, System.Action<T> callback) where T : Object;


        UnityEngine.Coroutine LoadAllAsyn(string path, System.Action<Object[]> callback);

        UnityEngine.Coroutine LoadAllAsyn(string path , System.Type type, System.Action<Object[]> callback);

        UnityEngine.Coroutine LoadAllAsyn<T>(string path, System.Action<T[]> callback) where T : Object;

        void Unload(bool unloadAllLoadedObjects);

        string Variant { get; set; }

    }

}