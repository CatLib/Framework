
using CatLib.Contracts.IO;

namespace CatLib.IO
{

    /// <summary>
    /// 文件服务
    /// </summary>
    public class IO : Component
    {

        static readonly char[] INVALID_FILE_NAME_CHARS = new char[] { '/', '\\', '<', '>', ':', '|', '"' };

        public const char PATH_SPLITTER = '/';

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

            if (path[path.Length - 1] == '/')
            {
                throw new System.IO.IOException("all directory paths are expected to not end with a leading slash. ( i.e. the '" + PATH_SPLITTER + "' character )");
            }
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


    }

}