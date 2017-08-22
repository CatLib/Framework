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

namespace CatLib.API.Hashing
{
    /// <summary>
    /// 哈希
    /// </summary>
    public interface IHashing
    {
        /// <summary>
        /// 计算校验和
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="checksum">使用校验类类型</param>
        /// <returns>校验和</returns>
        long Checksum(byte[] buffer , Checksums checksum = Checksums.Crc32);

        /// <summary>
        /// 对输入值进行Hash
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="factor">加密因子</param>
        /// <returns>加密值</returns>
        string Hash(string input, int factor = 10);

        /// <summary>
        /// 验证输入值和哈希值是否匹配
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="hash">哈希值</param>
        /// <returns>是否匹配</returns>
        bool Check(string input, string hash);
    }
}
