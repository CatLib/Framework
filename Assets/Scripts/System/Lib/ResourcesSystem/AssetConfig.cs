
namespace CatLib.ResourcesSystem {

    /// <summary>
    /// 资源配置信息
    /// </summary>
    public class AssetConfig {


        private AssetFile assetFile;

        public bool IsEncryption(string assetBundle)
        {

            if(assetFile == null)
            {
                AssetFileStore store = new AssetFileStore();
                assetFile = store.LoadFromPath(Env.AssetPath);
            }

            AssetFileField field = assetFile.Find(assetBundle);
            if (field == null) { return false; }

            return field.IsEncryption;

        }

    }

}
