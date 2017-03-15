using CatLib.API.IO;

public class IOCrypted : IIOCrypt
{
    /// <summary>
    /// 解密文件数据
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] Decrypted(string path, byte[] data)
    {
        UnityEngine.Debug.Log("file decrypted：" + path);
        return data;
    }

    /// <summary>
    /// 加密文件数据
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] Encrypted(string path, byte[] data)
    {
        return data;
    }
}
