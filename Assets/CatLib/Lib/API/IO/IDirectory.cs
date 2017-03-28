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
    /// 目录操作器
    /// </summary>
    public interface IDirectory
    {

        /// <summary>
        /// 路径
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 目录名字
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取当前目录是否为空
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 返回此目录的子目录（如果存在，反之抛出一个异常）
        /// </summary>
        /// <param name="directoryPath">子目录路径</param>
        /// <returns></returns>
        IDirectory this[string directoryPath] { get; }

        /// <summary>
        /// 获取当前目录文件夹是否存在
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        /// 获取当前目录文件夹下的子文件夹是否存在
        /// </summary>
        /// <param name="directoryPath">子文件夹</param>
        /// <returns></returns>
        bool Exists(string directoryPath);
        /// <summary>
        /// 删除当前目录文件夹
        /// </summary>
        void Delete();

        /// <summary>
        /// 删除子目录文件夹
        /// </summary>
        /// <param name="directoryPath">子目录路径</param>
        void Delete(string directoryPath);
        /// <summary>
        /// 根据路径创建目录文件夹
        /// </summary>
        void Create();
        /// <summary>
        /// 创建子目录文件夹,如果文件夹已经存在那么不会进行任何操作
        /// </summary>
        /// <param name="directoryPath">子目录文件夹</param>
        /// <returns></returns>
        IDirectory Create(string directoryPath);

        /// <summary>
        /// 将当前的目录文件夹拷贝到目标目录文件夹
        /// </summary>
        /// <param name="targetDirectroy">目标目录文件夹</param>
        /// <returns></returns>
        IDirectory CopyTo(string targetDirectroy);

        /// <summary>
        /// 根据路径获取文件操作对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IFile File(string path);

        /// <summary>
        /// 获取当前目录下的文件操作对象
        /// </summary>
        /// <param name="option">目录搜索方式 默认为当前目录,不包含子目录</param>
        /// <returns></returns>
        IFile[] GetFiles(SearchOption option = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// 获取当前目录下的文件操作对象
        /// </summary>
        /// <param name="filter">文件过滤方式</param>
        /// <param name="option">目录搜索方式</param>
        /// <returns></returns>
        IFile[] GetFiles(string filter, SearchOption option);

        /// <summary>
        /// 将当前目录移动到目标目录
        /// </summary>
        /// <param name="targetDirectory">目标目录</param>
        void MoveTo(string targetDirectory);

        /// <summary>
        /// 重命名当前目录
        /// </summary>
        /// <param name="newName">新的目录名称</param>
        void Rename(string newName);

        void Walk(Action<IFile> callBack, SearchOption option);

        void Walk(Action<IFile> callBack, string filter = "*", SearchOption option = SearchOption.AllDirectories);

    }

}