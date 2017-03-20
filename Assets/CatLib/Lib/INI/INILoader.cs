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
            return new INIResult(file);
        }

    }

}