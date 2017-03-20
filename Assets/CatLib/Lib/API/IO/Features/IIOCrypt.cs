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
 
namespace CatLib.API.IO
{

    /// <summary>
    /// 文件加解密接口
    /// </summary>
    public interface IIOCrypt
    {

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="data">二进制文件数据</param>
        /// <returns></returns>
        byte[] Decrypted(string path, byte[] data);

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="data">二进制文件数据</param>
        /// <returns></returns>
        byte[] Encrypted(string path, byte[] data);

    }

}