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

#if CATLIB
using CatLib.API.Encryption;

namespace CatLib.Encryption
{
    /// <summary>
    /// 加解密服务
    /// </summary>
    public sealed class EncryptionProvider : IServiceProvider
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 加密类型
        /// </summary>
        public string Cipher { get; set; }

        /// <summary>
        /// 加解密服务
        /// </summary>
        public EncryptionProvider()
        {
            Cipher = "AES-128-CBC";
        }

        /// <summary>
        /// 服务提供者初始化
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<Encrypter>().OnResolving((_, obj) =>
            {
                var encrypter = (Encrypter)obj;
                encrypter.Extend(() => MakeEncrypter(Key, Cipher));
                return encrypter;
            }).Alias<IEncrypter>().Alias<IEncryptionManager>();
        }

        /// <summary>
        /// 根据加密方式生成加密器
        /// </summary>
        /// <param name="key">使用的key</param>
        /// <param name="cipher">加密方式</param>
        private AesEncrypter MakeEncrypter(string key, string cipher)
        {
            if (string.IsNullOrEmpty(Key))
            {
                throw new RuntimeException("Please set config [EncryptionProvider.Key]");
            }
            return new AesEncrypter(key, cipher == "AES-128-CBC" ? 128 : 256);
        }
    }
}
#endif