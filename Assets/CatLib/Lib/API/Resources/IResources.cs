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

        IObject Load(string path , LoadTypes loadType = LoadTypes.AssetBundle);

        IObject Load(string path , System.Type type, LoadTypes loadType = LoadTypes.AssetBundle);

        IObject Load<T>(string path, LoadTypes loadType = LoadTypes.AssetBundle) where T : Object;

        IObject[] LoadAll(string path, LoadTypes loadType = LoadTypes.AssetBundle);

        IObject[] LoadAll<T>(string path, LoadTypes loadType = LoadTypes.AssetBundle) where T : Object;

        UnityEngine.Coroutine LoadAsync(string path, System.Action<IObject> callback, LoadTypes loadType = LoadTypes.AssetBundle);

        UnityEngine.Coroutine LoadAsync(string path , System.Type type, System.Action<IObject> callback, LoadTypes loadType = LoadTypes.AssetBundle);

        UnityEngine.Coroutine LoadAsync<T>(string path, System.Action<IObject> callback, LoadTypes loadType = LoadTypes.AssetBundle) where T : Object;

        UnityEngine.Coroutine LoadAllAsync(string path , System.Action<IObject[]> callback, LoadTypes loadType = LoadTypes.AssetBundle);

        UnityEngine.Coroutine LoadAllAsync<T>(string path, System.Action<IObject[]> callback, LoadTypes loadType = LoadTypes.AssetBundle) where T : Object;

    }

}