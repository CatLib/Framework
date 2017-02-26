
namespace CatLib.API.Resources
{

    /// <summary>
    /// AssetBundle解密接口
    /// </summary>
    public interface IDecrypted
    {


        /// <summary>
        /// 是否是被加密的
        /// </summary>
        /// <param name="abName">资源包路径</param>
        /// <returns></returns>
        bool IsEncryption(string assetBundlePath);

        /// <summary>
        /// 解密资源包
        /// </summary>
        /// <param name="assetBundlePath">资源包路径</param>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Decrypted(string assetBundlePath, byte[] data);

    }

}