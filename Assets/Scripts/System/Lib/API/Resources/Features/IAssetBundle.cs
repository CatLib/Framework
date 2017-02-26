
using UnityEngine;

namespace CatLib.API.Resources{

	public interface IAssetBundle{

		//string Variant{ get; set; }

		Object LoadAsset(string path);

		Object[] LoadAssetAll(string path);

        UnityEngine.Coroutine LoadAssetAsync(string path , System.Action<Object> callback);

        UnityEngine.Coroutine LoadAssetAllAsync(string path , System.Action<Object[]> callback);

        void UnloadAll();

        void UnloadAssetBundle(string assetbundlePath);

    }

}