using System;
using System.IO;

namespace CatLib.API.IO
{

    public interface IDirectory
    {

        string Path{ get; }

        string Name { get; }

        bool IsEmpty{ get; }

        IDirectory this[string directoryPath]{ get; }

        bool Exists();

        bool Exists(string directoryPath);

        void Delete();

        void Delete(string directoryPath);

        void Create();

        IDirectory Create(string directoryPath);

        IDirectory Refresh();

        IDirectory CopyTo(string targetDirectroy);

        IFile[] GetFiles(SearchOption option = SearchOption.TopDirectoryOnly);

        IFile[] GetFiles(string filter , SearchOption option);

        void MoveTo(string targetDirectory);

        void Rename(string newName);

        void Walk(Action<IFile> callBack , SearchOption option);

        void Walk(Action<IFile> callBack , string filter = "*" , SearchOption option = SearchOption.AllDirectories);

    }

}