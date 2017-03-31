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
 
using UnityEngine;

namespace CatLib.API.Resources
{

    public interface IResources
    {

        void AddExtension(System.Type type, string extension);

        IObjectInfo Load(string path , LoadTypes loadType = LoadTypes.AssetBundle);

        IObjectInfo Load(string path , System.Type type, LoadTypes loadType = LoadTypes.AssetBundle);

        IObjectInfo Load<T>(string path, LoadTypes loadType = LoadTypes.AssetBundle) where T : Object;

        IObjectInfo[] LoadAll(string path, LoadTypes loadType = LoadTypes.AssetBundle);

        IObjectInfo[] LoadAll<T>(string path, LoadTypes loadType = LoadTypes.AssetBundle) where T : Object;

        UnityEngine.Coroutine LoadAsync(string path, System.Action<IObjectInfo> callback, LoadTypes loadType = LoadTypes.AssetBundle);

        UnityEngine.Coroutine LoadAsync(string path , System.Type type, System.Action<IObjectInfo> callback, LoadTypes loadType = LoadTypes.AssetBundle);

        UnityEngine.Coroutine LoadAsync<T>(string path, System.Action<IObjectInfo> callback, LoadTypes loadType = LoadTypes.AssetBundle) where T : Object;

        UnityEngine.Coroutine LoadAllAsync(string path , System.Action<IObjectInfo[]> callback, LoadTypes loadType = LoadTypes.AssetBundle);

        UnityEngine.Coroutine LoadAllAsync<T>(string path, System.Action<IObjectInfo[]> callback, LoadTypes loadType = LoadTypes.AssetBundle) where T : Object;

    }

}