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

using CatLib.API.Hash;

namespace CatLib.Hash
{
    /// <summary>
    /// 哈希
    /// </summary>
    public class Hash : IHash
    {
        /// <summary>
        /// 生成的盐
        /// </summary>
        private string generateSalt;

        /// <summary>
        /// 盐
        /// </summary>
        public string GenerateSalt
        {
            get { return generateSalt; }
        }

        /// <summary>
        /// 设定盐
        /// </summary>
        /// <param name="salt">盐</param>
        public void SetGenerateSalt(string salt)
        {
            if (!string.IsNullOrEmpty(salt))
            {
                generateSalt = salt;
            }
        }

        /// <summary>
        /// 设定加密因子并获得盐
        /// </summary>
        /// <param name="factor">加密因子</param>
        public void SetFactor(int factor)
        {
            generateSalt = BCrypt.Net.BCrypt.GenerateSalt(factor);
        }

        /// <summary>
        /// 哈希一个输入值
        /// </summary>
        /// <param name="input">输入值</param>
        /// <returns></returns>
        public string Make(string input)
        {
            if (string.IsNullOrEmpty(generateSalt))
            {
                SetFactor(6);
            }
            return BCrypt.Net.BCrypt.HashPassword(input, generateSalt);
        }

        /// <summary>
        /// 检查是否匹配
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="hash">需要验证的hash</param>
        /// <returns>是否匹配</returns>
        public bool Check(string input, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(input, hash);
        }

        /// <summary>
        /// 用于计算文件的md5，您不应该用它进行密码等高敏感的hash加密
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>md5加密的值</returns>
        public string FileMd5(string path)
        {
            return Md5.ParseFile(path);
        }

        /// <summary>
        /// 字符串md5，您不应该用它进行密码等高敏感的hash加密
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <returns>md5加密的值</returns>
        public string StringMd5(string input)
        {
            return Md5.ParseString(input);
        }
    }
}