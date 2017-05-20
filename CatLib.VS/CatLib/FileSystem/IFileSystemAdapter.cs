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
    /// 文件系统适配器
    /// </summary>
    public interface IFileSystemAdapter
    {
        /// <summary>
        /// 文件或文件夹是否存在
        /// </summary>
        /// <param name="path">文件或文件夹是否存在</param>
        /// <returns>是否存在</returns>
        bool Has(string path);

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="contents">写入数据</param>
        /// <returns>是否成功</returns>
        bool Write(string path, byte[] contents);

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>读取的数据</returns>
        byte[] Read(string path);

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="path">旧的文件路径</param>
        /// <param name="newPath">新的文件路径</param>
        /// <returns>是否成功</returns>
        bool Rename(string path, string newPath);

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="copyPath">复制到的文件路径</param>
        /// <returns>是否成功</returns>
        bool Copy(string path, string copyPath);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>是否成功</returns>
        bool Delete(string path);

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>是否成功</returns>
        bool CreateDir(string path);

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件</returns>
        IFile GetFile(string path);

        /// <summary>
        /// 获取文件夹
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件夹</returns>
        IDirectory GetDir(string path);
    }
}
