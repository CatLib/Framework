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

namespace CatLib.API.Resources{

	public interface IAssetBundle{

        //string Variant{ get; set; }

        AssetBundle LoadBundle(string path);

        UnityEngine.Coroutine LoadBundleAsync(string path, System.Action<AssetBundle> callback);

        Object LoadAsset(string path);

		Object[] LoadAssetAll(string path);

        UnityEngine.Coroutine LoadAssetAsync(string path , System.Action<Object> callback);

        UnityEngine.Coroutine LoadAssetAllAsync(string path , System.Action<Object[]> callback);

        bool UnloadAll();

        bool UnloadAssetBundle(string assetbundlePath);

    }

}