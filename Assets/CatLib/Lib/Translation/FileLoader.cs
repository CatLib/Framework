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

using CatLib.API.INI;
using CatLib.API.IO;

namespace CatLib.Translation
{
    /// <summary>
    /// 文件加载器
    /// </summary>
    public sealed class FileLoader : IFileLoader
    {
        /// <summary>
        /// 磁盘
        /// </summary>
        private readonly IDisk disk;

        /// <summary>
        /// ini加载器
        /// </summary>
        private readonly IIniLoader iniLoader;

        /// <summary>
        /// 构建一个文件加载器
        /// </summary>
        /// <param name="disk">磁盘</param>
        /// <param name="iniLoader">ini加载器</param>
        public FileLoader(IDisk disk, IIniLoader iniLoader)
        {
            this.disk = disk;
            this.iniLoader = iniLoader;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="root">根目录</param>
        /// <param name="locale">语言</param>
        /// <param name="file">文件名</param>
        /// <returns>文件映射</returns>
        public IFileMapping Load(string root, string locale, string file)
        {
            return LoadIniPath(root, locale, file);
        }

        /// <summary>
        /// 加载ini文件
        /// </summary>
        /// <param name="root">根目录</param>
        /// <param name="locale">语言</param>
        /// <param name="file">文件名</param>
        /// <returns>文件映射</returns>
        private IFileMapping LoadIniPath(string root, string locale, string file)
        {
            var iniFile = disk.File(root + System.IO.Path.AltDirectorySeparatorChar + locale + System.IO.Path.AltDirectorySeparatorChar + file + ".ini");
            if (!iniFile.Exists)
            {
                return null;
            }
            var result = iniLoader.Load(iniFile);
            return new IniMapping(result);
        }
    }
}