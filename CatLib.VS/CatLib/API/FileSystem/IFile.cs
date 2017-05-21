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

namespace CatLib.API.FileSystem
{
    /// <summary>
    /// 文件
    /// </summary>
    public interface IFile : IHandler
    {
        /// <summary>
        /// 写入数据
        /// 如果数据已经存在则覆盖
        /// </summary>
        /// <param name="contents">写入数据</param>
        /// <returns>是否成功</returns>
        bool Write(byte[] contents);

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>读取的数据</returns>
        byte[] Read();
    }
}
