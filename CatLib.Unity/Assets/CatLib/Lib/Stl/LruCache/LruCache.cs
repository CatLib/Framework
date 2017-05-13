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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CatLib.API.Stl;

namespace CatLib.Stl
{
    /// <summary>
    /// 近期最少使用缓存
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    [ComVisible(false)]
    public sealed class LruCache<TKey, TVal> : ILruCache<TKey, TVal>
    {
        /// <summary>
        /// 最大容量
        /// </summary>
        private readonly int maxCapacity;

        /// <summary>
        /// Lru Cache
        /// </summary>
        private readonly Dictionary<TKey, CacheNode<TKey, TVal>> lruCache;

        /// <summary>
        /// 头节点
        /// </summary>
        private CacheNode<TKey, TVal> header;

        /// <summary>
        /// 尾节点
        /// </summary>
        private CacheNode<TKey, TVal> tail;

        /// <summary>
        /// 是否是向前的迭代方向
        /// </summary>
        private bool forward;

        /// <summary>
        /// 近期最少使用缓存迭代器
        /// </summary>
        private struct Enumerator : IEnumerable<KeyValuePair<TKey, TVal>>
        {
            /// <summary>
            /// 近期最少使用缓存
            /// </summary>
            private readonly LruCache<TKey, TVal> lruCache;

            /// <summary>
            /// 是否是向前遍历
            /// </summary>
            private readonly bool forward;

            /// <summary>
            /// 构造一个迭代器
            /// </summary>
            /// <param name="lruCache">近期最少使用缓存</param>
            internal Enumerator(LruCache<TKey, TVal> lruCache , bool forward)
            {
                this.lruCache = lruCache;
                this.forward = forward;
            }

            /// <summary>
            /// 迭代器
            /// </summary>
            /// <returns>元素迭代器</returns>
            public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator()
            {
                if (forward)
                {
                    var cursor = lruCache.header;
                    while (cursor != null)
                    {
                        yield return cursor.KeyValue;
                        cursor = cursor.Forward;
                    }
                }
                else
                {
                    var cursor = lruCache.tail;
                    while (cursor != null)
                    {
                        yield return cursor.KeyValue;
                        cursor = cursor.Backward;
                    }
                }
            }

            /// <summary>
            /// 获取迭代器
            /// </summary>
            /// <returns>迭代器</returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        /// <summary>
        /// 创建一个Lru缓存
        /// </summary>
        public LruCache(int maxCapacity)
        {
            this.maxCapacity = maxCapacity;
            lruCache = new Dictionary<TKey, CacheNode<TKey, TVal>>();
            forward = true;
        }

        /// <summary>
        /// 反转遍历顺序(并不是反转整个有序集)
        /// </summary>
        public void ReverseForeach()
        {
            forward = !forward;
        }

        /// <summary>
        /// 在lru缓存中增加一个元素,如果元素已经存在则会替换元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Add(TKey key, TVal value)
        {
            CacheNode<TKey, TVal> result;
            if (lruCache.TryGetValue(key , out result))
            {
                result.Replace(value);
                MakeUsed(result);
                return;
            }

            if (lruCache.Count >= maxCapacity)
            {
                RemoveLeastUsed();
            }

            var addedNode = new CacheNode<TKey, TVal>(key, value);

            if (header == null)
            {
                header = addedNode;
                tail = addedNode;
            }
            else
            {
                MakeUsed(addedNode);
            }

            lruCache.Add(key, addedNode);
        }

        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="key"></param>
        public void Remove(TKey key)
        {
            CacheNode<TKey, TVal> result;
            if (!lruCache.TryGetValue(key, out result))
            {
                return;
            }
            lruCache.Remove(key);
            if (result == tail)
            {
                tail = result.Backward;
            }
            if (result == header)
            {
                header = result.Forward;
            }
            if(result.Backward != null)
            {
                result.Backward.Forward = result.Forward;
            }
        }

        /// <summary>
        /// 根据key获取val，如果被淘汰则返回传入的默认值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>值</returns>
        public TVal Get(TKey key, TVal defaultValue = default(TVal))
        {
            CacheNode<TKey, TVal> result;
            if (!lruCache.TryGetValue(key , out result))
            {
                return defaultValue;
            }

            MakeUsed(result);

            return result.KeyValue.Value;
        }

        /// <summary>
        /// 获取Lru缓存中的元素数量
        /// </summary>
        public int Count
        {
            get { return lruCache.Count; }
        }

        /// <summary>
        /// 根据key获取val，如果被淘汰则返回默认值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public TVal this[TKey key]
        {
            get { return Get(key); }
            set { Add(key, value); }
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator()
        {
            return new Enumerator(this, forward).GetEnumerator();
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 移除最后一个元素
        /// </summary>
        private void RemoveLeastUsed()
        {
            lruCache.Remove(tail.KeyValue.Key);
            tail.Backward.Forward = null;
            tail = tail.Backward;
        }

        /// <summary>
        /// 激活指定节点为最近使用
        /// </summary>
        /// <param name="node">节点</param>
        private void MakeUsed(CacheNode<TKey, TVal> node)
        {
            if (node.Forward == null && node.Backward == null)
            {
                node.Forward = header;
                header.Backward = node;
                if (header.Forward == null)
                {
                    tail = header;
                }
                header = node;
            }
            else if (node.Forward == null && node.Backward != null)
            {
                node.Backward.Forward = null;
                tail = node.Backward;
                node.Forward = header;
                header.Backward = node;
                header = node;
            }
            else if (node.Forward != null && node.Backward != null)
            {
                node.Backward.Forward = node.Forward;
                node.Forward.Backward = node.Backward;
                node.Forward = header;
                header.Backward = node;
                header = node;
            }
        }
    }
}