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

namespace CatLib.Stl
{
    /// <summary>
    /// 跳跃链表
    /// 这个跳跃表根据key进行跳跃
    /// </summary>
    public sealed class SkipList<TKey, TValue> : IEnumerable<TValue>
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
        private readonly SkipNode<TKey, TValue> header;

        /// <summary>
        /// 跳跃表空结点
        /// </summary>
        private readonly NullSkipNode<TKey, TValue> nullNode;

        /// <summary>
        /// 可能出现层数的概率
        /// </summary>
        private readonly double probability;

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
        public SkipList(int maxNode) :
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
            hasLevel = 1;
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
            while (node != null && !nullNode.Equals(node))
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

            var update = FindNearedUpdateNode(key);
            var cursor = header;

            //游标结点的下一个结点（需要插入的目标结点）
            cursor = cursor.Links[0];

            //如果结点已经存在
            if (!nullNode.Equals(cursor) && cursor.Key.CompareTo(key) == 0)
            {
                //将已经存在的结点使用新的值覆盖
                cursor.Value = value;
                return;
            }

            //随机获取层数
            var newLevel = GetRandomLevel();

            //如果随机出的层数大于现有层数，那么将新增的需要更新的结点初始化
            if (newLevel > hasLevel)
            {
                for (var i = hasLevel; i < newLevel; i++)
                {
                    update[i] = header;
                }
                hasLevel = newLevel;
            }

            //将游标指向为新的跳跃结点
            cursor = new SkipNode<TKey, TValue>(newLevel, key, value);
            for (var i = 0; i < newLevel; i++)
            {
                //新增结点的跳跃目标为更新结点的跳跃目标
                cursor.Links[i] = update[i].Links[i];
                //老的需要被更新的结点跳跃目标为新增结点
                update[i].Links[i] = cursor;
            }

            Count++;
        }

        /// <summary>
        /// 如果元素存在那么从跳跃链表中删除元素
        /// </summary>
        /// <param name="key">键</param>
        public bool Remove(TKey key)
        {
            Guard.Requires<ArgumentNullException>(key != null);

            var update = FindNearedUpdateNode(key, maxLevel + 1);

            //将游标指向为目标结点
            var cursor = update[0].Links[0];

            if (cursor.Key.CompareTo(key) != 0)
            {
                return false;
            }

            for (var i = 0; i < hasLevel; i++)
            {
                //如果下一个元素是需要删除的元素
                //将上一个结点指向要被删除元素的下一个结点
                if (update[i].Links[i] == cursor)
                {
                    update[i].Links[i] = cursor.Links[i];
                }
            }

            //如果顶层指向的是空跳跃层那么删除顶层跳跃层
            while ((hasLevel > 1) && header.Links[hasLevel - 1].Equals(nullNode))
            {
                hasLevel--;
            }

            Count--;
            return true;
        }

        /// <summary>
        /// 根据键查找结点
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public TValue this[TKey key]
        {
            get
            {
                var cursor = Find(key);
                return cursor != nullNode ? cursor.Value : default(TValue);
            }
        }

        /// <summary>
        /// 是否包含某个元素
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        public bool Contains(TKey key)
        {
            return Find(key) != null;
        }

        /// <summary>
        /// 查找结点的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        private SkipNode<TKey, TValue> Find(TKey key)
        {
            Guard.Requires<ArgumentNullException>(key != null);

            var cursor = header;
            for (var i = 0; i < hasLevel; i++)
            {
                var nextElement = cursor.Links[i];
                while (nextElement.Key.CompareTo(key) == -1)
                {
                    cursor = nextElement;
                    nextElement = cursor.Links[i];
                }
            }

            cursor = cursor.Links[0];
            return cursor.Key.Equals(key) ? cursor : null;
        }

        /// <summary>
        /// 搜索距离键临近的需要更新的结点
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="level">层数容量</param>
        /// <returns>需要更新的结点列表</returns>
        private SkipNode<TKey, TValue>[] FindNearedUpdateNode(TKey key, int level = 0)
        {
            if (level <= 0)
            {
                level = maxLevel;
            }
            var update = new SkipNode<TKey, TValue>[level];
            var cursor = header;
            //从跳跃层高到低的进行查找
            for (var i = hasLevel - 1; i >= 0; i--)
            {
                //查找比输入的key小的最后一个结点
                while (!nullNode.Equals(cursor.Links[i]) && cursor.Links[i].Key.CompareTo(key) == -1)
                {
                    cursor = cursor.Links[i];
                }

                //将找到的最后一个结点置入需要更新的结点
                update[i] = cursor;
            }

            return update;
        }

        /// <summary>
        /// 获取随机层
        /// </summary>
        /// <returns>随机的层数</returns>
        private int GetRandomLevel()
        {
            var newLevel = 1;
            var ran = random.NextDouble();
            while ((newLevel < maxLevel) && (ran < probability))
            {
                newLevel++;
            }
            return newLevel;
        }
    }
}

