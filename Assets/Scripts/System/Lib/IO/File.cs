using System;
using CatLib.Contracts.IO;

namespace CatLib.IO
{

    public class File : IFile
    {

        private string directory;
        
        private string extension;

        private string fileName;

        private System.IO.FileInfo fileInfo;
        

        /// <summary>
		/// 文件
		/// </summary>
        public File(string path)
        {
            ParseFile(path);
        }

        /// <summary>
		/// 原始文件信息
		/// </summary>
        public System.IO.FileInfo FileInfo{

            get{

                if(fileInfo == null){
                    fileInfo = new System.IO.FileInfo(FullName);
                }
                return fileInfo;
            }

        }

        /// <summary>
		/// 文件拓展名
		/// </summary>
        public string Extension
        {
            get { return extension; }
        }

        /// <summary>
		/// 文件路径
		/// </summary>
        public string FullName
        {
            get { return directory + IO.PATH_SPLITTER + fileName + extension; }
        }

        /// <summary>
		/// 文件名
		/// </summary>
        public string Name
        {
            get { return fileName + extension; }
        }

        /// <summary>
		/// 文件名不包含路径
		/// </summary>
        public string NameWithoutExtension
        {
            get { return fileName; }
        }

        /// <summary>
		/// 文件长度
		/// </summary>
        public long Length{

            get{

                return FileInfo.Length;

            }

        }

        /// <summary>
		/// 是否存在
		/// </summary>
        public bool Exists{

            get{
                
                return FileInfo.Exists;
            
            }

        }

        /// <summary>
		/// 文件夹
		/// </summary>
        public IDirectory Directory
        {
            get
            {
                return new Directory(directory);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        public void Delete()
        {
            FileInfo.Delete();
            Refresh();
        }

        /// <summary>
		/// 复制文件
		/// </summary>
        public IFile CopyTo(string targetFileName)
        {
            IFile destFile = new File(targetFileName);
            destFile.Directory.Create();
            FileInfo.CopyTo(targetFileName);
            return destFile.Refresh();
        }

        /// <summary>
		/// 复制文件
		/// </summary>
        public IFile CopyTo(IDirectory targetDirectory){

            return CopyTo(targetDirectory + IO.PATH_SPLITTER.ToString() + Name);

        }

        /// <summary>
		/// 移动到指定文件
		/// </summary>
        public void MoveTo(IDirectory targetDirectory)
        {
            MoveTo(targetDirectory.Path);
        }

        /// <summary>
		/// 移动到指定文件
		/// </summary>
        public void MoveTo(string targetDirectory){

            IO.ValidatePath(targetDirectory);
            IO.MakeDirectory(targetDirectory).Create();
            FileInfo.MoveTo(targetDirectory + IO.PATH_SPLITTER + fileName + extension);
            directory = targetDirectory;
            Refresh();

        }
        
        /// <summary>
		/// 移动到指定文件
		/// </summary>
        public void Rename(string newName)
        {

            IO.IsValidFileName(newName);

            if (newName.Contains(IO.PATH_SPLITTER.ToString()))
            {
                throw new ArgumentException("rename can't be used to change a files location use Move(string newPath) instead.");
            }

            string newExtension = System.IO.Path.GetExtension(newName);
            string newFileName = System.IO.Path.GetFileNameWithoutExtension(newName);

            IFile targetFile = new File(directory + IO.PATH_SPLITTER + newFileName + newExtension);
            if(targetFile.Exists)
            {
                throw new ArgumentException("duplicate file name:" + newName);
            }
            FileInfo.MoveTo(directory + IO.PATH_SPLITTER + newFileName + newExtension);
            fileName = newFileName;
            extension = newExtension;

            Refresh();

        }

        public IFile Refresh()
        {
            fileInfo = null;
            return this;
        }

        private void ParseFile(string path){

            extension = System.IO.Path.GetExtension(path);
            fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            directory = System.IO.Path.GetDirectoryName(path);

        }

    }

}