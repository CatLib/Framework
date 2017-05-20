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

using CatLib.API.FileSystem;

namespace CatLib.FileSystem
{
    /// <summary>
    /// 文件系统
    /// </summary>
    public class FileSystem : IFileSystem
    {
        /// <summary>
        /// 文件系统适配器
        /// </summary>
        private IFileSystemAdapter adapter;

        public FileSystem(IFileSystemAdapter adapter)
        {
            this.adapter = adapter;
        }

        
    }
}
