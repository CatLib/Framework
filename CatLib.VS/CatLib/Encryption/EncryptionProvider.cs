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

using System.Text;
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
        [Config(null)]
        public string Key { get; set; }

        /// <summary>
        /// 加密类型
        /// </summary>
        [Config("AES-128-CBC")]
        public string Cipher { get; set; }

        /// <summary>
        /// 密钥编码
        /// </summary>
        public Encoding Encoding = Encoding.Default;

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
            App.Singleton<Encrypter>((_, __) =>
            {
                if (string.IsNullOrEmpty(Key))
                {
                    throw new RuntimeException("Please set config [EncryptionProvider.Key]");
                }

                Encoding = Encoding ?? Encoding.Default;
                return new Encrypter(Encoding.GetBytes(Key), Cipher);
            }).Alias<IEncrypter>();
        }
    }
}
