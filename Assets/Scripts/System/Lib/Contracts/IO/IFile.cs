namespace CatLib.Contracts.IO
{

    public interface IFile
    {

        System.IO.FileInfo FileInfo{ get; }

        string Extension{ get; }

        string FullName{ get; }

        string Name{ get; }

        string NameWithoutExtension{ get; }

        bool Exists{ get; }

        IDirectory Directory{ get; }

        long Length{ get; }

        void Delete();

        IFile CopyTo(string targetFileName);

        IFile CopyTo(IDirectory targetDirectory);

        void MoveTo(IDirectory targetDirectory);

        void MoveTo(string targetDirectory);

        void Rename(string newName);

        IFile Refresh();

    }
}
