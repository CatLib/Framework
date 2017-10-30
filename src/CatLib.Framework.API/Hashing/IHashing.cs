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

using System;
using System.Text;

namespace CatLib.API.Hashing
{
    /// <summary>
    /// 哈希
    /// </summary>
    public interface IHashing
    {
        /// <summary>
        /// 使用默认的校验算法计算校验和
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>校验和</returns>
        long Checksum(string input);

        /// <summary>
        /// 使用默认的校验算法计算校验和
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="checksum">使用校验类类型</param>
        /// <returns>校验和</returns>
        long Checksum(string input, Checksums checksum);

        /// <summary>
        /// 使用默认的校验算法计算校验和
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="encoding">编码</param>
        /// <param name="checksum">使用校验类类型</param>
        /// <returns>校验和</returns>
        long Checksum(string input, Encoding encoding, Checksums checksum);

        /// <summary>
        /// 计算校验和
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <returns>校验和</returns>
        long Checksum(byte[] buffer);

        /// <summary>
        /// 计算校验和
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="checksum">使用校验类类型</param>
        /// <returns>校验和</returns>
        long Checksum(byte[] buffer, Checksums checksum);

        /// <summary>
        /// 将字节数组添加到数据校验和，闭包区间内校验器是专享的。
        /// </summary>
        /// <param name="callback">回调闭包(buffer , offset , count)</param>
        /// <param name="checksum">使用校验类类型</param>
        /// <returns>闭包区间内的总校验和</returns>
        long Checksum(Action<Action<byte[], int, int>> callback, Checksums checksum);

        /// <summary>
        /// 对输入值进行加密性Hash
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="factor">加密因子</param>
        /// <returns>哈希值</returns>
        string HashPassword(string input, int factor = 10);

        /// <summary>
        /// 验证输入值和加密性哈希值是否匹配
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="hash">哈希值</param>
        /// <returns>是否匹配</returns>
        bool CheckPassword(string input, string hash);

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <returns>哈希值</returns>
        [Obsolete("HashString is obsolete, please use Checksum")]
        uint HashString(string input);

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="hash">使用的哈希算法</param>
        /// <returns>哈希值</returns>
        [Obsolete("HashString is obsolete, please use Checksum")]
        uint HashString(string input, Hashes hash);

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="encoding">编码</param>
        /// <param name="hash">使用的哈希算法</param>
        /// <returns>哈希值</returns>
        [Obsolete("HashString is obsolete, please use Checksum")]
        uint HashString(string input, Encoding encoding, Hashes hash);

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <returns>哈希值</returns>
        [Obsolete("HashString is obsolete, please use Checksum")]
        uint HashByte(byte[] input);

        /// <summary>
        /// 对输入值进行非加密哈希
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="hash">使用的哈希算法</param>
        /// <returns>哈希值</returns>
        [Obsolete("HashString is obsolete, please use Checksum")]
        uint HashByte(byte[] input, Hashes hash);
    }
}
