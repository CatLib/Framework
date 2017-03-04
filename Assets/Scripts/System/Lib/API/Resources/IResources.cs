using UnityEngine;

namespace CatLib.API.Resources
{

    public interface IResources
    {

        void AddExtension(System.Type type, string extension);

        IObjectInfo Load(string path);

        IObjectInfo Load(string path , System.Type type);

        IObjectInfo Load<T>(string path) where T : Object;

        IObjectInfo[] LoadAll(string path);

        IObjectInfo[] LoadAll<T>(string path) where T : Object;

        UnityEngine.Coroutine LoadAsync(string path, System.Action<IObjectInfo> callback);

        UnityEngine.Coroutine LoadAsync(string path , System.Type type, System.Action<IObjectInfo> callback);

        UnityEngine.Coroutine LoadAsync<T>(string path, System.Action<IObjectInfo> callback) where T : Object;

        UnityEngine.Coroutine LoadAllAsync(string path , System.Action<IObjectInfo[]> callback);

        UnityEngine.Coroutine LoadAllAsync<T>(string path, System.Action<IObjectInfo[]> callback) where T : Object;

    }

}