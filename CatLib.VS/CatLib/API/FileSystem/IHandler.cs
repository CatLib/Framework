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

using System.IO;

namespace CatLib.API.FileSystem
{
    /// <summary>
    /// 句柄
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// 文件/文件夹路径
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="newName">新的文件/文件夹名字</param>
        /// <returns>是否成功</returns>
        bool Rename(string newName);

        /// <summary>
        /// 复制文件或文件夹到指定路径
        /// </summary>
        /// <param name="copyPath">复制到的路径(不应该包含文件夹或者文件名)</param>
        /// <returns>是否成功</returns>
        bool Copy(string copyPath);

        /// <summary>
        /// 删除文件或者文件夹
        /// </summary>
        /// <returns>是否成功</returns>
        bool Delete();

        /// <summary>
        /// 获取文件/文件夹属性
        /// </summary>
        /// <param name="path">文件/文件夹路径</param>
        /// <returns>文件/文件夹属性</returns>
        FileAttributes GetAttributes(string path);
    }
}
