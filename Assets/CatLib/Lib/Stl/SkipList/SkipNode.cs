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
using System.Collections.Generic;

namespace CatLib.Stl
{
    /// <summary>
    /// 跳跃结点
    /// </summary>
    internal class SkipNode<TKey, TValue> 
        where TKey : IComparable<TKey>
    {
        /// <summary>
        /// 键
        /// </summary>
        public TKey Key { get; private set; }

        /// <summary>
        /// 值
        /// </summary>
        public TValue Value { get; internal set; }

        /// <summary>
        /// 链接的结点
        /// </summary>
        public IList<SkipNode<TKey, TValue>> Links { get; private set; }

        /// <summary>
        /// 跳跃结点
        /// </summary>
        /// <param name="level">层数</param>
        internal SkipNode(int level)
        {
            Guard.Requires<ArgumentOutOfRangeException>(level > 0);

            Links = new List<SkipNode<TKey, TValue>>(level);
        }

        /// <summary>
        /// 新建一个跳跃结点
        /// </summary>
        /// <param name="level">拥有的层数</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        internal SkipNode(int level, TKey key, TValue value)
        {
            Guard.Requires<ArgumentNullException>(key != null);
            Guard.Requires<ArgumentOutOfRangeException>(level > 0);

            Key = key;
            Value = value;
            Links = new List<SkipNode<TKey, TValue>>(level);
        }
    }
}