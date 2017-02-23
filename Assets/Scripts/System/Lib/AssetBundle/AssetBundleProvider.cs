using System;
using CatLib.API.AssetBundle;
using CatLib.API.IO;

namespace CatLib.AssetBundle{

	public class AssetBundleProvider : ServiceProvider {

		public override Type[] ProviderDepend { get { return new Type[] { typeof(IIO) }; } }

		public override void Register()
        {
            App.Singleton<AssetBundleLoader>().Alias<IAssetBundle>();
        }

	}

}