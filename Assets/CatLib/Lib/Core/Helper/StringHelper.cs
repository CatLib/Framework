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
 
namespace CatLib
{
    public static class StringHelper
    {
        
        /// <summary>
        /// 标准化路径
        /// </summary>
        public static string Standard(this string path)
        {
            return path.Replace("\\", "/");
        }

    }

}