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

namespace CatLib.Stl
{
    /// <summary>
    /// 空跳跃结点
    /// </summary>
    internal sealed class NullSkipNode<TKey, TValue> : SkipNode<TKey, TValue> , IEquatable<SkipNode<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        /// <summary>
        /// 空的跳跃结点
        /// </summary>
        /// <param name="level"></param>
        public NullSkipNode(int level)
                : base(level)
        {
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="otherNode">其他结点</param>
        /// <returns>是否相等</returns>
        public bool Equals(SkipNode<TKey, TValue> otherNode)
        {
            var otherNullNode = otherNode as NullSkipNode<TKey, TValue>;
            return otherNullNode != null;
        }
    }
}