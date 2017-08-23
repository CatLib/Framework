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

namespace CatLib
{
    /// <summary>
    /// 数组
    /// </summary>
    public static class Arr
    {
        /// <summary>
        /// 合并数组
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="sources">需要合并的数组</param>
        /// <returns>合并后的数组</returns>
        public static T[] Merge<T>(params T[][] sources)
        {
            var length = 0;
            foreach (var source in sources)
            {
                length += source.Length;
            }

            var merge = new T[length];
            var current = 0;
            foreach (var source in sources)
            {
                Array.Copy(source, 0, merge, current, source.Length);
                current += source.Length;
            }

            return merge;
        }
    }
}
