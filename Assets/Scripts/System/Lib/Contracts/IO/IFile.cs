namespace CatLib.Contracts.IO
{

    public interface IFile
    {

        void Delete(string path);

        bool Exists(string path);

        void Create(string path, byte[] array, int offset, int count);

    }
}
