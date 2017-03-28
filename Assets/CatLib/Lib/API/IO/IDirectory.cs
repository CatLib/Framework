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

        string Name { get; }

        bool IsEmpty { get; }

        IDirectory this[string directoryPath] { get; }

        bool Exists();

        bool Exists(string directoryPath);

        void Delete();

        void Delete(string directoryPath);

        void Create();

        IDirectory Create(string directoryPath);

        IDirectory CopyTo(string targetDirectroy);

        IFile File(string path);

        IFile[] GetFiles(SearchOption option = SearchOption.TopDirectoryOnly);

        IFile[] GetFiles(string filter, SearchOption option);

        void MoveTo(string targetDirectory);

        void Rename(string newName);

        void Walk(Action<IFile> callBack, SearchOption option);

        void Walk(Action<IFile> callBack, string filter = "*", SearchOption option = SearchOption.AllDirectories);

    }

}