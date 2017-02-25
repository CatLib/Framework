using CatLib.API.ResourcesSystem;

public class AssetDecrypted : IDecrypted
{

    /// <summary>
    /// 是否是被加密的
    /// </summary>
    /// <param name="abName">资源包路径</param>
    /// <returns></returns>
    public bool IsEncryption(string assetBundlePath)
    {
        return true;
    }

    /// <summary>
    /// 解密资源包
    /// </summary>
    /// <param name="assetBundlePath">资源包路径</param>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] Decrypted(string assetBundlePath, byte[] data)
    {
        return data;
    }
}
