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
using Random = System.Random;

namespace CatLib.Stl
{
    /// <summary>
    /// 跳跃链表
    /// 这个跳跃表优先使用评分进行跳跃，当评分相同则使用元素排序
    /// </summary>
    public class SkipList<TElement, TScore> : IEnumerable<TElement>
        where TElement : IComparable<TElement>, IEquatable<TElement>
        where TScore : IComparable<TScore>
    {
        /// <summary>
        /// 跳跃结点
        /// </summary>
        protected class SkipNode
        {
            internal struct SkipNodeLevel
            {
                /// <summary>
                /// 前一个结点
                /// </summary>
                internal SkipNode Forward;

                /// <summary>
                /// 层跨越的结点数量
                /// </summary>
                internal long Span;
            }

            /// <summary>
            /// 元素
            /// </summary>
            internal TElement Element;

            /// <summary>
            /// 分数
            /// </summary>
            internal TScore Score;

            /// <summary>
            /// 向后的结点
            /// </summary>
            internal SkipNode Backward;

            /// <summary>
            /// 层级
            /// </summary>
            internal SkipNodeLevel[] Level;
        }

        /// <summary>
        /// 可能出现层数的默认概率
        /// </summary>
        private const double PROBABILITY = 0.25;

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
        private int level;

        /// <summary>
        /// 跳跃表头结点
        /// </summary>
        protected readonly SkipNode header;

        /// <summary>
        /// 尾部结点
        /// </summary>
        protected SkipNode tail;

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
        /// <param name="probable">可能出现层数的概率系数(0-1之间的数)</param>
        /// <param name="maxLevel">最大层数</param>
        public SkipList(double probable = PROBABILITY, int maxLevel = 32)
        {
            Guard.Requires<ArgumentOutOfRangeException>(maxLevel > 0);
            Guard.Requires<ArgumentOutOfRangeException>(probable < 1);
            Guard.Requires<ArgumentOutOfRangeException>(probable > 0);

            probability = probable * 0xFFFF;
            this.maxLevel = maxLevel;
            level = 1;
            header = new SkipNode
            {
                Level = new SkipNode.SkipNodeLevel[maxLevel]
            };
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            var node = header.Level[0];
            while (node.Forward != null)
            {
                yield return node.Forward.Element;
                node = node.Forward.Level[0];
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
        /// <param name="element">元素</param>
        /// <param name="score">分数</param>
        public void Add(TElement element, TScore score)
        {
            Guard.Requires<ArgumentNullException>(element != null);

            int i;
            long[] rank;

            var update = FindNearedUpdateNode(element, score, out rank);

            //随机获取层数
            var newLevel = GetRandomLevel();

            //如果随机出的层数大于现有层数，那么将新增的需要更新的结点初始化
            if (newLevel > level)
            {
                for (i = level; i < newLevel; ++i)
                {
                    rank[i] = 0;
                    update[i] = header;
                    update[i].Level[i].Span = Count;
                }
                level = newLevel;
            }

            //将游标指向为新的跳跃结点
            var cursor = new SkipNode
            {
                Element = element,
                Score = score,
                Level = new SkipNode.SkipNodeLevel[newLevel]
            };

            for (i = 0; i < newLevel; ++i)
            {
                //新增结点的跳跃目标为更新结点的跳跃目标
                cursor.Level[i].Forward = update[i].Level[i].Forward;
                //老的需要被更新的结点跳跃目标为新增结点
                update[i].Level[i].Forward = cursor;
                //更新跨度
                cursor.Level[i].Span = update[i].Level[i].Span - (rank[0] - rank[i]);
                update[i].Level[i].Span = (rank[0] - rank[i]) + 1;
            }

            //将已有的层中的跨度 + 1
            for (i = newLevel; i < level; ++i)
            {
                ++update[i].Level[i].Span;
            }

            //给与上一个的结点
            cursor.Backward = (update[0] == header) ? null : update[0];

            if (cursor.Level[0].Forward != null)
            {
                cursor.Level[0].Forward.Backward = cursor;
            }
            else
            {
                tail = cursor;
            }

            Count++;
        }

        /// <summary>
        /// 如果元素存在那么从跳跃链表中删除元素
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="score">分数</param>
        public bool Remove(TElement element, TScore score)
        {
            Guard.Requires<ArgumentNullException>(element != null);

            long[] rank;
            var update = FindNearedUpdateNode(element, score, out rank);
            var cursor = update[0].Level[0].Forward;

            if (cursor == null || cursor.Score.CompareTo(score) != 0 || !cursor.Element.Equals(element))
            {
                return false;
            }

            DeleteNode(cursor, update);
            return true;
        }

        /// <summary>
        /// 删除结点关系
        /// </summary>
        /// <param name="cursor">结点</param>
        /// <param name="update">更新结点列表</param>
        protected void DeleteNode(SkipNode cursor, SkipNode[] update)
        {
            for (var i = 0; i < level; ++i)
            {
                if (update[i].Level[i].Forward == cursor)
                {
                    update[i].Level[i].Span += cursor.Level[i].Span - 1;
                    update[i].Level[i].Forward = cursor.Level[i].Forward;
                }
                else
                {
                    update[i].Level[i].Span -= 1;
                }
            }
            if (cursor.Level[0].Forward != null)
            {
                cursor.Level[0].Forward.Backward = cursor.Backward;
            }
            else
            {
                tail = cursor.Backward;
            }
            while (level > 1 && header.Level[level - 1].Forward == null)
            {
                --level;
            }
            --Count;
        }

        /// <summary>
        /// 获取元素排名
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="score">分数</param>
        /// <returns>排名</returns>
        protected long GetRank(TElement element, TScore score)
        {
            long rank = 0;
            var cursor = header;
            for (var i = level - 1; i >= 0; ++i)
            {
                while (cursor.Level[i].Forward != null &&
                       (cursor.Level[i].Forward.Score.CompareTo(score) < 0 ||
                        (cursor.Level[i].Forward.Score.CompareTo(score) == 0 &&
                         cursor.Level[i].Forward.Element.CompareTo(element) <= 0)))
                {
                    rank += cursor.Level[i].Span;
                    cursor = cursor.Level[i].Forward;
                }
                if (cursor.Element != null && cursor.Element.Equals(element))
                {
                    return rank;
                }
            }
            return 0;
        }

        /// <summary>
        /// 搜索距离键临近的需要更新的结点
        /// </summary>
        /// <param name="elemtent">元素</param>
        /// <param name="score">分数</param>
        /// <param name="rank">排名</param>
        /// <returns>需要更新的结点列表</returns>
        protected SkipNode[] FindNearedUpdateNode(TElement elemtent, TScore score, out long[] rank)
        {
            var update = new SkipNode[maxLevel];
            var cursor = header;
            rank = new long[maxLevel];
            //从跳跃层高到低的进行查找
            for (var i = level - 1; i >= 0; --i)
            {
                //rank为上一级结点的跨度数作为起点
                rank[i] = i == (level - 1) ? 0 : rank[i + 1];
                while (cursor.Level[i].Forward != null &&
                        (cursor.Level[i].Forward.Score.CompareTo(score) < 0 ||
                           cursor.Level[i].Forward.Score.CompareTo(score) == 0 &&
                            cursor.Level[i].Forward.Element.CompareTo(elemtent) < 0))
                {
                    rank[i] += cursor.Level[i].Span;
                    cursor = cursor.Level[i].Forward;
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
            while (random.Next(0, 0xFFFF) < probability)
            {
                newLevel += 1;
            }
            return (newLevel < maxLevel) ? newLevel : maxLevel;
        }
    }
}

