using CatLib.API.IO;
using System.Text;

namespace CatLib.ResourcesSystem
{

    public class AssetFileStore
    {

        public const string FILE_NAME = "encryption.catlib";

        public AssetFile LoadFromBytes(byte[] request)
        {
            var file = new AssetFile();
            file.Parse(new UTF8Encoding(false).GetString(request));
            return file;
        }

        public AssetFile LoadFromPath(string path)
        {
            IFile file = IO.IO.MakeFile(path + IO.IO.PATH_SPLITTER + FILE_NAME);
            return LoadFromBytes(file.Read());
        }

        public void Save(string path, AssetFile assetFile)
        {
            IFile file = IO.IO.MakeFile(path + IO.IO.PATH_SPLITTER + FILE_NAME);
            file.Delete();
            file.Create(assetFile.Data.ToByte());
        }
    }


}