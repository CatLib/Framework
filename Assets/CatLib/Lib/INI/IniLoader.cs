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

using System;
using System.IO;
using System.Text;
using CatLib.API.INI;
using CatLib.API.IO;
using CatLib.API;

namespace CatLib.INI
{
    /// <summary>
    /// ini加载器
    /// </summary>
    public sealed class IniLoader : IIniLoader
    {
        /// <summary>
        /// 加载一个ini文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns>ini结果集</returns>
        public IIniResult Load(IFile file)
        {
            if (!file.Exists)
            {
                throw new IOException("file is not exists:" + file.FullName);
            }
            if (file.Extension != ".ini")
            {
                throw new ArgumentException("ini file path is invalid", "file");
            }

            var result = new IniResult(file.Read());

            result.OnSave += (data) =>
            {
                file.Delete();
                file.Create(Encoding.UTF8.GetBytes(data));
            };

            return result;
        }

        /// <summary>
        /// 加载Ini文件
        /// </summary>
        /// <param name="iniData">要被解析成ini结果的字符串</param>
        /// <returns>ini结果集</returns>
        public IIniResult Load(string iniData)
        {
            var result = new IniResult(iniData);

            result.OnSave += (data) =>
            {

                throw new CatLibException("not support save with Load(string) loaded result");

            };

            return result;
        }

        /// <summary>
        /// 加载ini文件
        /// </summary>
        /// <param name="iniData">要被解析成ini结果的字节数据</param>
        /// <returns>ini结果集</returns>
        public IIniResult Load(byte[] iniData)
        {
            var result = new IniResult(iniData);

            result.OnSave += (data) =>
            {

                throw new CatLibException("not support save with Load(byte[]) loaded result");

            };

            return result;
        }
    }
}