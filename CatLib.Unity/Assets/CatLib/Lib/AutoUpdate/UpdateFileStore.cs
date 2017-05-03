/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System.Text;
using CatLib.API.IO;
using System.IO;

namespace CatLib.AutoUpdate
{
    /// <summary>
    /// 更新文件容器
    /// </summary>
    public sealed class UpdateFileStore
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public const string FILE_NAME = "update-list.catlib";

        /// <summary>
        /// 文件服务
        /// </summary>
        [Inject]
        public IIOFactory IO { get; set; }

        /// <summary>
        /// 磁盘
        /// </summary>
        private IDisk disk;

        /// <summary>
        /// 磁盘
        /// </summary>
        private IDisk Disk
        {
            get { return disk ?? (disk = IO.Disk()); }
        }

        /// <summary>
        /// 从字节流加载更新文件
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <returns>更新文件数据</returns>
        public UpdateFile LoadFromBytes(byte[] bytes)
        {
            var file = new UpdateFile();
            file.Parse(new UTF8Encoding(false).GetString(bytes));
            return file;
        }

        /// <summary>
        /// 从文件加载更新文件
        /// </summary>
        /// <param name="path">文件路径(绝对路径)</param>
        /// <returns>更新文件数据</returns>
        public UpdateFile LoadFromPath(string path)
        {
            var file = Disk.File(path + Path.AltDirectorySeparatorChar + FILE_NAME, PathTypes.Absolute);
            return LoadFromBytes(file.Read());
        }

        /// <summary>
        /// 保存一个更新文件
        /// </summary>
        /// <param name="path">存储路径(绝对路径)</param>
        /// <param name="updateFile">更新文件数据</param>
        public void Save(string path, UpdateFile updateFile)
        {
            var file = Disk.File(path + Path.AltDirectorySeparatorChar + FILE_NAME, PathTypes.Absolute);
            file.Delete();
            file.Create(Encoding.UTF8.GetBytes(updateFile.Data));
        }
    }
}
