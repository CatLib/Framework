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

using CatLib.API.FileSystem;

namespace CatLib.FileSystem
{
    /// <summary>
    /// 文件
    /// </summary>
    public sealed class File : Handler, IFile
    {
        /// <summary>
        /// 文件
        /// </summary>
        /// <param name="fileSystem">文件系统</param>
        /// <param name="path">文件路径</param>
        public File(FileSystem fileSystem, string path) :
            base(fileSystem, path)
        {
        }

        /// <summary>
        /// 写入数据
        /// 如果数据已经存在则覆盖
        /// </summary>
        /// <param name="contents">写入数据</param>
        public void Write(byte[] contents)
        {
            FileSystem.Write(Path, contents);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <returns>读取的数据</returns>
        public byte[] Read()
        {
            return FileSystem.Read(Path);
        }
    }
}
