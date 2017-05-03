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

namespace CatLib.API.IO
{
    /// <summary>
    /// 文件
    /// </summary>
    public interface IFile
    {
        /// <summary>
		/// 文件拓展名
		/// </summary>
        string Extension { get; }

        /// <summary>
		/// 文件路径
		/// </summary>
        string FullName { get; }

        /// <summary>
		/// 文件名
		/// </summary>
        string Name { get; }

        /// <summary>
		/// 文件名不包含路径
		/// </summary>
        string NameWithoutExtension { get; }

        /// <summary>
		/// 是否存在
		/// </summary>
        bool Exists { get; }

        /// <summary>
		/// 所属的文件夹
		/// </summary>
        IDirectory Directory { get; }

        /// <summary>
		/// 文件长度
		/// </summary>
        long Length { get; }

        /// <summary>
        /// 删除文件
        /// </summary>
        void Delete();

        /// <summary>
		/// 复制文件
		/// </summary>
		/// <param name="targetFileName">目标文件</param>
        IFile CopyTo(string targetFileName);

        /// <summary>
		/// 复制文件
		/// </summary>
		/// <param name="targetDirectory">目标文件夹</param>
        IFile CopyTo(IDirectory targetDirectory);

        /// <summary>
		/// 移动到指定文件夹
		/// </summary>
		/// <param name="targetDirectory">目标文件夹</param>
        void MoveTo(IDirectory targetDirectory);

        /// <summary>
		/// 移动到指定文件
		/// </summary>
		/// <param name="targetDirectory">目标文件夹</param>
        void MoveTo(string targetDirectory);

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="newName">新的名字</param>
        void Rename(string newName);

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="data">文件数据</param>
        void Create(byte[] data);

        /// <summary>
        /// 异步创建文件
        /// </summary>
        /// <param name="data">文件数据</param>
        /// <param name="callback">当完成时的回调</param>
        void CreateAsync(byte[] data, Action callback);

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <returns>文件数据</returns>
        byte[] Read();

        /// <summary>
        /// 异步读取文件
        /// </summary>
        /// <param name="callback">读取的数据回调</param>
        void ReadAsync(Action<byte[]> callback);
    }
}
