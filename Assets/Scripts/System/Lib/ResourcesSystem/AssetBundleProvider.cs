using System;
using CatLib.API.IO;
using CatLib.API.ResourcesSystem;

namespace CatLib.ResourcesSystem{

	public class AssetBundleProvider : ServiceProvider {

		public override Type[] ProviderDepend { get { return new Type[] { typeof(IIO) }; } }

		public override void Register()
        {
            App.Singleton<AssetBundleLoader>().Alias<IAssetBundle>();
        }

	}

}