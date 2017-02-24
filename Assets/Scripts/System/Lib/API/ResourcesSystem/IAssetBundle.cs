
using System.Collections;
using UnityEngine;

namespace CatLib.API.ResourcesSystem{

	public interface IAssetBundle{

		string Variant{ get; set; }

		Object LoadAsset(string path);

		Object[] LoadAssetAll(string path);

		IEnumerator LoadAssetAsync(string path , System.Action<Object> callback);

		IEnumerator LoadAssetAllAsync(string path , System.Action<Object[]> callback);

	}

}