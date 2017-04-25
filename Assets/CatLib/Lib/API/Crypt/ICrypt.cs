
namespace CatLib.API.Crypt
{
    /// <summary>
    /// 加解密
    /// </summary>
    public interface ICrypt
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="str">需要被加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        string Encrypt(string str);

        /// <summary>
        /// 解密被加密的字符串
        /// </summary>
        /// <param name="str">需要被解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        string Decrypt(string str);
    }
}

