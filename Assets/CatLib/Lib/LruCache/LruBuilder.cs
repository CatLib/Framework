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

using CatLib.API.LruCache;

namespace CatLib.LruCache
{
    /// <summary>
    /// lru构造器
    /// </summary>
    public class LruBuilder : ILruBuilder
    {
        /// <summary>
        /// 创建 lru 缓存
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TVal">值类型</typeparam>
        /// <param name="maxCapacity">最大容量</param>
        /// <returns>lru缓存</returns>
        public ILruCache<TKey, TVal> Create<TKey, TVal>(int maxCapacity)
        {
            return new LruCache<TKey, TVal>(maxCapacity);
        }
    }
}