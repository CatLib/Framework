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

    public interface IFile
    {

        /// <summary>
        /// 文件扩展名
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
        /// 获取当前文件是否存在
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// 文件夹
        /// </summary>
        IDirectory Directory { get; }

        /// <summary>
        /// 文件长度
        /// </summary>
        long Length { get; }
        /// <summary>
        /// 删除当前文件
        /// </summary>
        void Delete();

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="targetFileName">目标路径</param>
        /// <returns></returns>
        IFile CopyTo(string targetFileName);
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="targetDirectory">目标目录</param>
        /// <returns></returns>
        IFile CopyTo(IDirectory targetDirectory);

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="targetDirectory">目标目录</param>
        void MoveTo(IDirectory targetDirectory);
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="targetDirectory">目标路径</param>
        void MoveTo(string targetDirectory);

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="newName">新命名</param>
        void Rename(string newName);

        /// <summary>
        /// 根据Byte 数据创建文件
        /// </summary>
        /// <param name="array"></param>
        void Create(byte[] array);

        /// <summary>
        /// 以异步的方式根据Byte 数组创建文件
        /// </summary>
        /// <param name="array"></param>
        /// <param name="callback"></param>
        void CreateAsync(byte[] array, Action callback);

        /// <summary>
        /// 读取当前文件
        /// </summary>
        /// <returns></returns>
        byte[] Read();

        /// <summary>
        /// 以异步的方式读取当前文件
        /// </summary>
        /// <param name="callback"></param>
        void ReadAsync(Action<byte[]> callback);

    }
}
