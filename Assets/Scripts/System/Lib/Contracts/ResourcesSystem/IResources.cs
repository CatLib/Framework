using UnityEngine;
using XLua;

namespace CatLib.Contracts.ResourcesSystem
{

    public interface IResources
    {

        Object Load(string path);

        T Load<T>(string path) where T : Object;

        Object[] LoadAll(string path);

        T[] LoadAll<T>(string path) where T : Object;

        UnityEngine.Coroutine LoadAsyn(string path, System.Action<Object> callback);

        UnityEngine.Coroutine LoadAsyn<T>(string path, System.Action<T> callback) where T : Object;


        UnityEngine.Coroutine LoadAllAsyn(string path, System.Action<Object[]> callback);

        UnityEngine.Coroutine LoadAllAsyn<T>(string path, System.Action<T[]> callback) where T : Object;

        void Unload(bool unloadAllLoadedObjects);

        string Variant { get; set; }
    }

}