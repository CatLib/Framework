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
using CatLib.API.INI;
using CatLib.API.IO;
using CatLib.API;

namespace CatLib.INI
{

    /// <summary>
    /// INI加载器
    /// </summary>
    public class INILoader : IINILoader
    {
        /// <summary>
        /// 加载一个INI文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns></returns>
        public IINIResult Load(IFile file)
        {

            if (!file.Exists)
            {
                throw new IOException("file is not exists:" + file.FullName);
            }

            if (file.Extension != ".ini")
            {
                throw new ArgumentException("ini file path is invalid", "path");
            }

            var result = new INIResult(file.Read());

            result.SetSaveCallback((data) =>{

                file.Delete();
                file.Create(data.ToByte());

            });

            return result;

        }

        public IINIResult Load(string iniData){

            var result = new INIResult(iniData);

            result.SetSaveCallback((data) =>{

                throw new CatLibException("not support save with Load(string) loaded result");

            });

            return result;

        }

        public IINIResult Load(byte[] iniData){

            var result = new INIResult(iniData);

            result.SetSaveCallback((data) =>{

                throw new CatLibException("not support save with Load(byte[]) loaded result");

            });

            return result;

        }

    }

}