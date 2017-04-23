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

namespace CatLib.Stl
{
    /// <summary>
    /// 跳跃链表
    /// </summary>
    public sealed class SkipList<TKey, TValue> : IEnumerable<TValue> , IDictionary<TKey, TValue>
            where TKey : IComparable<TKey>
    {
        /// <summary>
        /// 可能出现层数的默认概率
        /// </summary>
        private const double PROBABILITY = 0.5;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// 最大层数
        /// </summary>
        private readonly int maxLevel;

        /// <summary>
        /// 当前拥有的层
        /// </summary>
        private int hasLevel;

        /// <summary>
        /// 跳跃表头结点
        /// </summary>
        private SkipNode<TKey, TValue> header;

        /// <summary>
        /// 跳跃表空结点
        /// </summary>
        private readonly NullSkipNode<TKey, TValue> nullNode;

        /// <summary>
        /// 可能出现层数的概率
        /// </summary>
        private double probability;

        /// <summary>
        /// 随机数发生器
        /// </summary>
        private readonly Random random = new Random();

        /// <summary>
        /// 结点数量
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 同步锁
        /// </summary>
        public object SyncRoot
        {
            get { return syncRoot; }
        }

        /// <summary>
        /// 创建一个跳跃链表
        /// </summary>
        /// <param name="maxNode">规划最大结点</param>
        public SkipList(int maxNode):
            this(PROBABILITY, (int)(Math.Ceiling(Math.Log(maxNode) / Math.Log(1 / PROBABILITY) - 1)))
        {
            Guard.Requires<ArgumentOutOfRangeException>(maxNode > 0);
        }

        /// <summary>
        /// 创建一个跳跃链表
        /// </summary>
        /// <param name="probable">可能出现层数的概率</param>
        /// <param name="maxLevel">最大层数</param>
        public SkipList(double probable, int maxLevel)
        {
            Guard.Requires<ArgumentOutOfRangeException>(maxLevel > 0);
            Guard.Requires<ArgumentOutOfRangeException>(probable < 1);
            Guard.Requires<ArgumentOutOfRangeException>(probable > 0);

            probability = probable;
            this.maxLevel = maxLevel;
            hasLevel = 0;
            header = new SkipNode<TKey, TValue>(maxLevel);
            nullNode = new NullSkipNode<TKey, TValue>(maxLevel);

            for (var i = 0; i < maxLevel; i++)
            {
                header.Links[i] = nullNode;
            }
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator<TValue> GetEnumerator()
        {
            var node = header.Links[0];
            while (node != null)
            {
                yield return node.Value;
                node = node.Links[0];
            }
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
        /// 插入记录
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Add(TKey key, TValue value)
        {
            Guard.Requires<ArgumentNullException>(key != null);

            var update = new SkipNode<TKey, TValue>[maxLevel];
            var cursor = header;

            //从高到低的层进行查找
            for (var i = hasLevel; i >= 0; i--)
            {
                //查找比输入的key小的最后一个结点
                while (cursor.Links[i].Key.CompareTo(key) == -1)
                {
                    cursor = cursor.Links[i];
                }
                //将找到的最后一个结点置入更新结点
                update[i] = cursor;
            }

            //下一个底层结点
            cursor = cursor.Links[0];

            if (cursor.Key.CompareTo(key) == 0)
            {
                //分配新的值
                cursor.Value = value;
            }

            //随机获取层数
            var newLevel = GetRandomLevel();

            if (newLevel > hasLevel)
            {
                for (var i = hasLevel + 1; i < newLevel; i++)
                {
                    update[i] = header;
                }
                hasLevel = newLevel;
            }

            cursor = new SkipNode<TKey, TValue>(newLevel, key, value);

            for (var i = 0; i < newLevel; i++)
            {
                cursor.Links[i] = update[i].Links[i];
                update[i].Links[i] = cursor;
            }
            Count++;

        }

        /// <summary>
        /// 获取随机层
        /// </summary>
        /// <returns>随机的层数</returns>
        private int GetRandomLevel()
        {
            var newLevel = 0;
            var ran = random.NextDouble();
            while ((newLevel < maxLevel) && (ran < probability))
            {
                newLevel++;
            }
            return newLevel;
        }
    }
}

