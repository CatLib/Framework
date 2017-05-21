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
    /// 文件夹
    /// </summary>
    public class Directory : Handler , IDirectory
    {
        /// <summary>
        /// 文件夹
        /// </summary>
        /// <param name="fileSystem">文件系统</param>
        /// <param name="path">文件夹路径</param>
        public Directory(FileSystem fileSystem, string path) : 
            base(fileSystem , path)
        {
        }


    }
}
