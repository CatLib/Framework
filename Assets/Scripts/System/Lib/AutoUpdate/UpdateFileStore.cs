using System.Text;
using CatLib.API.IO;

namespace CatLib.AutoUpdate{

	public class UpdateFileStore{

        public const string FILE_NAME = "update-list.catlib";

        public UpdateFile LoadFromBytes(byte[] request)
        {
            var file = new UpdateFile();
            file.Parse(new UTF8Encoding(false).GetString(request));
            return file;
        }

        public UpdateFile LoadFromPath(string path)
        {
            IFile file = IO.IO.MakeFile(path + IO.IO.PATH_SPLITTER + UpdateFileStore.FILE_NAME);
            return LoadFromBytes(file.Read());
        }
        
        public void Save(string path , UpdateFile updateFile){

            IFile file = IO.IO.MakeFile(path + IO.IO.PATH_SPLITTER + UpdateFileStore.FILE_NAME);
            file.Delete();
            file.Create(updateFile.Data.ToByte());

        }


	}

}
