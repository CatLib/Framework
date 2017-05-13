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

using System.Collections;
using System.Collections.Generic;

namespace CatLib.API.Stl
{
    /// <summary>
    /// Lru缓存
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TVal">值的类型</typeparam>
    public interface ILruCache<TKey, TVal> : IEnumerable<KeyValuePair<TKey, TVal>>
    {
        /// <summary>
        /// 反转遍历顺序(并不是反转整个有序集)
        /// </summary>
        void ReverseForeach();

        /// <summary>
        /// 在lru缓存中增加一个元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Add(TKey key, TVal value);

        /// <summary>
        /// 根据key获取val，如果被淘汰则返回传入的默认值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>值</returns>
        TVal Get(TKey key, TVal defaultValue = default(TVal));

        /// <summary>
        /// 获取Lru缓存中的元素数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 根据key获取val，如果被淘汰则返回默认值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        TVal this[TKey key] { get; }
    }
}