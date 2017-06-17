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

namespace CatLib.Translation
{
    /// <summary>
    /// 文件加载器
    /// </summary>
    public interface IFileLoader
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="root">根目录</param>
        /// <param name="locale">语言</param>
        /// <param name="file">文件名</param>
        /// <returns>文件映射</returns>
        IFileMapping Load(string root, string locale, string file);
    }
}