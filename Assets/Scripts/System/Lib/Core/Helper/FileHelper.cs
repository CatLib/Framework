using System.IO;

namespace CatLib
{
    public static class FileHelper
    {

        /// <summary>
        /// 文件大小
        /// </summary>
        public static long Length(this FileSystemInfo file)
        {

            if (file is DirectoryInfo)
            {

                return 0;

            }

            return (new FileInfo(file.FullName)).Length;

        }

    }

}