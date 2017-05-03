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
 
using CatLib.API.IO;

namespace CatLib.API.INI
{
    /// <summary>
    /// ini加载器
    /// </summary>
    public interface IIniLoader
    {
        /// <summary>
        /// 加载一个ini文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns>ini结果集</returns>
        IIniResult Load(IFile file);

        /// <summary>
        /// 加载Ini文件
        /// </summary>
        /// <param name="iniData">要被解析成ini结果的字符串</param>
        /// <returns>ini结果集</returns>
        IIniResult Load(string iniData);

        /// <summary>
        /// 加载ini文件
        /// </summary>
        /// <param name="iniData">要被解析成ini结果的字节数据</param>
        /// <returns>ini结果集</returns>
        IIniResult Load(byte[] iniData);
    }
}