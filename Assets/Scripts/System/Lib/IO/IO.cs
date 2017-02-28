
using CatLib.API.IO;

namespace CatLib.IO
{

    /// <summary>
    /// 文件服务
    /// </summary>
    public class IO : Component , IIO
    {

        [Dependency]
        public IIOCrypt IOCrypt { get; set; }

        private static IO io;
        public static IO Instance{ get{ return io; } }

        static readonly char[] INVALID_FILE_NAME_CHARS = new char[] { '/', '\\', '<', '>', ':', '|', '"' };

        public const char PATH_SPLITTER = '/';

        public IO()
        {
            io = this;
        }

        public static bool IsValidFileName(string name)
        {
            for (int i = 0; i < INVALID_FILE_NAME_CHARS.Length; i++)
            {
                if (name.IndexOf(INVALID_FILE_NAME_CHARS[i]) != -1)
                {
                    return false;
                }
            }
            return true;
        }

        public static void ValidatePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new System.IO.IOException("a path can not be null or empty when searching the project");
            }

            if (path[path.Length - 1] == IO.PATH_SPLITTER)
            {
                throw new System.IO.IOException("all directory paths are expected to not end with a leading slash. ( i.e. the '" + PATH_SPLITTER + "' character )");
            }
        }

        public static string NormalizePath(string path){

            if(path[0] != IO.PATH_SPLITTER){

                return IO.PATH_SPLITTER + path;

            }
            return path;
            
        }

        public static IFile MakeFile(string path){

            return new File(path);

        }

        public static IDirectory MakeDirectory(string path){

            return new Directory(path);

        }

        public IDirectory AssetPath
        {
            get
            {
                return new Directory(Env.AssetPath);
            }
        }

        public IDirectory DataPath
        {
            get
            {
                return new Directory(Env.DataPath);
            }
        }

        public char PathSpliter{

            get{ return PATH_SPLITTER; }

        }


        public IFile File(string path){

            return IO.MakeFile(path);

        }

        public IDirectory Directory(string path){

            return IO.MakeDirectory(path);

        }

    }

}