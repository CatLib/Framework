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
using CatLib.API.LruCache;

namespace CatLib.LruCache
{
    /// <summary>
    /// 近期最少使用缓存
    /// </summary>
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
        private CacheNode<TKey, TVal> head;

        /// <summary>
        /// 尾节点
        /// </summary>
        private CacheNode<TKey, TVal> tail;

        /// <summary>
		/// 当前节点
		/// </summary>
        private CacheNode<TKey, TVal> current;

        /// <summary>
        /// 创建一个Lru缓存
        /// </summary>
        public LruCache(int maxCapacity)
        {
            this.maxCapacity = maxCapacity;
            lruCache = new Dictionary<TKey, CacheNode<TKey, TVal>>();
        }

        /// <summary>
        /// 在lru缓存中增加一个元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Add(TKey key, TVal value)
        {
            if (lruCache.ContainsKey(key))
            {
                MakeUsed(lruCache[key]);
            }

            if (lruCache.Count >= maxCapacity)
            {
                RemoveLeastUsed();
            }

            var addedNode = new CacheNode<TKey, TVal>(key, value);

            if (head == null)
            {
                head = addedNode;
                tail = addedNode;
            }
            else
            {
                MakeUsed(addedNode);
            }

            lruCache.Add(key, addedNode);

            Reset();
        }

        /// <summary>
        /// 根据key获取val，如果被淘汰则返回传入的默认值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>值</returns>
        public TVal Get(TKey key, TVal defaultValue = default(TVal))
        {
            if (!lruCache.ContainsKey(key))
            {
                return defaultValue;
            }

            MakeUsed(lruCache[key]);
            Reset();

            return lruCache[key].KeyValue.Value;
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
        }

        /// <summary>
		/// 下一个节点是否有数据
		/// </summary>
        public bool MoveNext()
        {
            if (current != null)
            {
                return (current = current.Next) != null;
            }
            if (head == null)
            {
                return false;
            }
            current = head;
            return true;
        }

        /// <summary>
		/// 重置
		/// </summary>
        public void Reset()
        {
            current = null;
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Reset();
        }

        /// <summary>
		/// 当前元素
		/// </summary>
        public object Current
        {
            get { return current.KeyValue; }
        }

        /// <summary>
        /// 当前元素
        /// </summary>
        KeyValuePair<TKey, TVal> IEnumerator<KeyValuePair<TKey, TVal>>.Current
        {
            get { return current.KeyValue; }
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator GetEnumerator()
        {
            return this;
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        IEnumerator<KeyValuePair<TKey, TVal>> IEnumerable<KeyValuePair<TKey, TVal>>.GetEnumerator()
        {
            return this;
        }

        /// <summary>
        /// 移除最后一个元素
        /// </summary>
        private void RemoveLeastUsed()
        {
            lruCache.Remove(tail.KeyValue.Key);
            tail.Previous.Next = null;
            tail = tail.Previous;
        }

        /// <summary>
        /// 激活指定节点为最近使用
        /// </summary>
        /// <param name="node">节点</param>
        private void MakeUsed(CacheNode<TKey, TVal> node)
        {
            if (node.Next == null && node.Previous == null)
            {
                node.Next = head;
                head.Previous = node;
                if (head.Next == null)
                {
                    tail = head;
                }
                head = node;
            }
            else if (node.Next == null && node.Previous != null)
            {
                node.Previous.Next = null;
                tail = node.Previous;
                node.Next = head;
                head.Previous = node;
                head = node;
            }
            else if (node.Next != null && node.Previous != null)
            {
                node.Previous.Next = node.Next;
                node.Next.Previous = node.Previous;
                node.Next = head;
                head.Previous = node;
                head = node;
            }
        }
    }
}