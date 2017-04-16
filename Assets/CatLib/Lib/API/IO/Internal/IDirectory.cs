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
using System.IO;

namespace CatLib.API.IO
{
    /// <summary>
    /// 文件夹
    /// </summary>
    public interface IDirectory
    {
        /// <summary>
        /// 文件夹目录位置
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 文件夹名字
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 文件夹是否是空的
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 返回此目录的子目录（如果存在，反之抛出一个异常）
        /// </summary>
        /// <param name="directoryPath">子目录路径</param>
        /// <returns>子目录文件夹</returns>
        IDirectory this[string directoryPath] { get; }

        /// <summary>
        /// 当前文件夹是否存在
        /// </summary>
        /// <returns>文件夹是否存在</returns>
        bool Exists();

        /// <summary>
        /// 子文件夹是否存在
        /// </summary>
        /// <param name="directoryPath">子文件夹的相对路径</param>
        /// <returns>是否存在</returns>
        bool Exists(string directoryPath);

        /// <summary>
        /// 删除文件夹
        /// </summary>
        void Delete();

        /// <summary>
        /// 删除子目录文件夹
        /// </summary>
        /// <param name="directoryPath">子目录相对路径</param>
        void Delete(string directoryPath);

        /// <summary>
        /// 创建文件夹,如果文件夹已经存在那么不会进行任何操作
        /// </summary>
        void Create();

        /// <summary>
        /// 创建子目录文件夹,如果文件夹已经存在那么不会进行任何操作
        /// </summary>
        /// <param name="directoryPath">子目录相对路径</param>
        /// <returns>子文件夹实例</returns>
        IDirectory Create(string directoryPath);

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="targetDirectroy">目标文件夹</param>
        IDirectory CopyTo(string targetDirectroy);

        /// <summary>
        /// 获取文件夹内的指定文件
        /// </summary>
        /// <returns>文件</returns>
        IFile File(string path);

        /// <summary>
        /// 获取当前目录所有的文件
        /// </summary>
        /// <param name="option">搜索选项</param>
        /// <returns>文件数组</returns>
        IFile[] GetFiles(SearchOption option = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// 获取当前目录顶层所有的文件
        /// </summary>
        /// <param name="filter">筛选</param>
        /// <param name="option">搜索选项</param>
        /// <returns>文件数组</returns>
        IFile[] GetFiles(string filter, SearchOption option);

        /// <summary>
        /// 将当前目录移动到目标目录
        /// </summary>
        /// <param name="targetDirectory">目标文件夹</param>
        void MoveTo(string targetDirectory);

        /// <summary>
        /// 重命名当前文件夹
        /// </summary>
        /// <param name="newName">新的文件夹名字</param>
        void Rename(string newName);

        /// <summary>
        /// 查找文件并调用指定callback
        /// </summary>
        /// <param name="callBack">回调</param>
        /// <param name="option">搜索选项</param>
        void Walk(Action<IFile> callBack, SearchOption option);

        /// <summary>
        /// 查找文件并调用指定callback
        /// </summary>
        /// <param name="callBack">回调</param>
        /// <param name="filter">过滤</param>
        /// <param name="option">搜索选项</param>
        void Walk(Action<IFile> callBack, string filter = "*", SearchOption option = SearchOption.AllDirectories);
    }
}