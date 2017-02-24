using System;

namespace CatLib.ResourcesSystem {

    public class AssetFileField {

        /// <summary>资源文件</summary>
        public string AssetBundle { get; protected set; }

        /// <summary>是否被加密的</summary>
        public bool IsEncryption { get; protected set; }

        public AssetFileField(string str)
        {

            string[] lst = str.Split('\t');

            if (lst.Length < 2) { throw new Exception("asset file data field data error"); }

            AssetBundle = lst[0];
            IsEncryption = bool.Parse(lst[1]);

        }

        public AssetFileField(string assetBundle, bool isEncryption)
        {
            AssetBundle = assetBundle;
            IsEncryption = isEncryption;
        }

        public override string ToString()
        {

            return AssetBundle + "\t" + IsEncryption + "\n";

        }
    }

}
