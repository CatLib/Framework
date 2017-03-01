using System;
using System.IO;
using CatLib.API.IO;

namespace CatLib.IO
{

    public class File : IFile
    {

        private LocalDisk disk;

        private string directory;
        
        private string extension;

        private string fileName;

        private System.IO.FileInfo fileInfo;

        private System.IO.FileInfo FileInfo{

            get{

                if(fileInfo == null){

                    fileInfo = new System.IO.FileInfo(FullName);

                }
                return fileInfo;

            }
        }
        
        /// <summary>
		/// 文件
		/// </summary>
        public File(string path , LocalDisk disk) 
        {
            this.disk = disk;
            ParseFile(path);
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
            get { return directory + Path.AltDirectorySeparatorChar + fileName + extension; }
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
                return new Directory(directory , disk);
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
            IFile destFile = new File(targetFileName, disk);
            destFile.Directory.Create();
            FileInfo.CopyTo(targetFileName);
            return destFile.Refresh();
        }

        /// <summary>
		/// 复制文件
		/// </summary>
        public IFile CopyTo(IDirectory targetDirectory){

            return CopyTo(targetDirectory + Path.AltDirectorySeparatorChar.ToString() + Name);

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

            LocalDisk.ValidatePath(targetDirectory);
            (new Directory(targetDirectory , disk)).Create();
            FileInfo.MoveTo(targetDirectory + Path.AltDirectorySeparatorChar + fileName + extension);
            directory = targetDirectory;
            Refresh();

        }
        
        /// <summary>
		/// 移动到指定文件
		/// </summary>
        public void Rename(string newName)
        {

            LocalDisk.IsValidFileName(newName);

            if (newName.Contains(Path.AltDirectorySeparatorChar.ToString()))
            {
                throw new ArgumentException("rename can't be used to change a files location use Move(string newPath) instead.");
            }

            string newExtension = System.IO.Path.GetExtension(newName);
            string newFileName = System.IO.Path.GetFileNameWithoutExtension(newName);

            IFile targetFile = new File(directory + Path.AltDirectorySeparatorChar + newFileName + newExtension , disk);
            if(targetFile.Exists)
            {
                throw new ArgumentException("duplicate file name:" + newName);
            }
            FileInfo.MoveTo(directory + Path.AltDirectorySeparatorChar + newFileName + newExtension);
            fileName = newFileName;
            extension = newExtension;

            Refresh();

        }

        /// <summary>
        /// 创建文件
        /// </summary>
        public void Create(byte[] data)
        {
            if(Exists){

                throw new System.IO.IOException("file is exists");

            }
            data = Encrypted(data);
            using (System.IO.FileStream fs = System.IO.File.Create(FullName))
            {
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }

        /// <summary>
        /// 异步创建文件
        /// </summary>
        public void CreateAsync(byte[] data, Action callback)
        {
            if(Exists){

                throw new System.IO.IOException("file is exists");

            }

            data = Encrypted(data);

            System.IO.FileStream fs = System.IO.File.Create(FullName);
            //todo:需要测试
            fs.BeginWrite(data, 0, data.Length , (result)=>{

                fs.EndRead(result);
                fs.Close();
                fs.Dispose();

                if(callback != null){

                    callback.Invoke();

                }

            }, null);

        }

        /// <summary>
        /// 读取文件
        /// </summary>
        public byte[] Read(){

            if(!Exists){

                throw new System.IO.IOException("file is not exists : " + FullName);

            }

            using (System.IO.FileStream fs = new System.IO.FileStream(FullName , System.IO.FileMode.Open))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                data = Decrypted(data);
                return data;
            }

        }

        public void ReadAsync(Action<byte[]> callback){

            if(!Exists){

                throw new System.IO.IOException("file is not exists");

            }

            //todo:需要测试
            System.IO.FileStream fs = new System.IO.FileStream( FullName , 
                                                                System.IO.FileMode.Open, 
                                                                System.IO.FileAccess.Read,
                                                                System.IO.FileShare.None, 
                                                                bufferSize : 1024,
                                                                useAsync   : true
                                                                );
                                                
            byte[] data = new byte[fs.Length];
            fs.BeginRead(data , 0 , data.Length, (result)=>{
                
                fs.EndRead(result);
                data = Decrypted(data);
                fs.Close();
                fs.Dispose();
                callback.Invoke(data);

            },null);

        }

        public IFile Refresh()
        {
            fileInfo = null;
            return this;
        }

        private byte[] Encrypted(byte[] data)
        {
            if (disk.IOCrypt != null)
            {
                data = disk.IOCrypt.Encrypted(FullName, data);
            }
            return data;
        }

        private byte[] Decrypted(byte[] data)
        {
            if (disk.IOCrypt != null)
            {
                data = disk.IOCrypt.Decrypted(FullName, data);
            }
            return data;
        }

        private void ParseFile(string path){

            extension = System.IO.Path.GetExtension(path);
            fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            directory = System.IO.Path.GetDirectoryName(path);

        }

    }

}