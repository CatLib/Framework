using CatLib.Contracts.IO;

namespace CatLib.IO
{

    public class File : IFile
    {

        /// <summary>
		/// 删除指定文件
		/// </summary>
        public void Delete(string path)
        {
            System.IO.File.Delete(path);
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        public bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void Create(string path, byte[] array, int offset, int count)
        {
            using (System.IO.FileStream fs = System.IO.File.Create(path))
            {
                fs.Write(array, offset, count);
                fs.Close();
            }
        }

    }

}