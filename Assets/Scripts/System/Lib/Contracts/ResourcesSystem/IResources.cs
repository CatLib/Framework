using UnityEngine;

namespace CatLib.Contracts.ResourcesSystem
{

    public interface IResources
    {

        T Load<T>(string path) where T : Object;

        T[] LoadAll<T>(string path) where T : Object;

        UnityEngine.Coroutine LoadAsyn<T>(string path, System.Action<T> callback) where T : Object;


        UnityEngine.Coroutine LoadAllAsyn<T>(string path, System.Action<T[]> callback) where T : Object;

        void Unload(bool unloadAllLoadedObjects);

        string Variant { get; set; }
    }

}