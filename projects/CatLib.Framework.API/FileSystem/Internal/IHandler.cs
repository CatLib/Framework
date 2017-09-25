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
        /// 文件/文件夹是否存在
        /// </summary>
        bool IsExists { get; }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="newName">新的文件/文件夹名字</param>
        void Rename(string newName);

        /// <summary>
        /// 将文件/文件夹移动到指定路径
        /// </summary>
        /// <param name="newPath">移动到的目标路径</param>
        void Move(string newPath);

        /// <summary>
        /// 复制文件或文件夹到指定路径
        /// </summary>
        /// <param name="copyPath">复制到的路径(不应该包含文件夹或者文件名)</param>
        void Copy(string copyPath);

        /// <summary>
        /// 删除文件或者文件夹
        /// </summary>
        void Delete();

        /// <summary>
        /// 获取文件/文件夹属性
        /// </summary>
        /// <returns>文件/文件夹属性</returns>
        FileAttributes GetAttributes();

        /// <summary>
        /// 是否是文件夹
        /// </summary>
        /// <returns>是否是文件夹</returns>
        bool IsDir { get; }

        /// <summary>
        /// 文件/文件夹大小
        /// </summary>
        long GetSize();
    }
}
