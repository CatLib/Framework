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

namespace CatLib.Stl
{
    /// <summary>
    /// 快速列表
    /// </summary>
    /// <typeparam name="TElement">元素</typeparam>
    public sealed class QuickList<TElement> : IEnumerable<TElement>
    {
        /// <summary>
        /// 合并系数
        /// </summary>
        private const float MERGE_PRO = 0.9f;

        /// <summary>
        /// 快速列表结点
        /// </summary>
        private class QuickListNode
        {
            /// <summary>
            /// 后置结点
            /// </summary>
            internal QuickListNode Backward;

            /// <summary>
            /// 前置结点
            /// </summary>
            internal QuickListNode Forward;

            /// <summary>
            /// 列表
            /// </summary>
            internal InternalList<TElement> List;
        }

        /// <summary>
        /// 每个快速列表结点最多的元素数量
        /// </summary>
        private readonly int fill;

        /// <summary>
        /// 列表头
        /// </summary>
        private QuickListNode header;

        /// <summary>
        /// 列表尾
        /// </summary>
        private QuickListNode tail;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// 同步锁
        /// </summary>
        public object SyncRoot
        {
            get { return syncRoot; }
        }

        /// <summary>
        /// 列表元素基数
        /// </summary>
        public long Count { get; private set; }

        /// <summary>
        /// 快速列表中的结点数量
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// 快速列表
        /// </summary>
        /// <param name="fill">每个结点中元素的最大数量</param>
        public QuickList(int fill = 128)
        {
            this.fill = fill;
        }

        /// <summary>
        /// 将元素插入到列表尾部
        /// </summary>
        /// <param name="element">元素</param>
        public void Push(TElement element)
        {
            if (AllowInsert(tail))
            {
                tail.List.InsertAt(element, tail.List.Count);
            }
            else
            {
                var node = CreateNode();
                node.List.InsertAt(element, 0);
                InsertNode(tail, node, true);
            }
            ++Count;
        }

        /// <summary>
        /// 将元素插入到列表头部
        /// </summary>
        /// <param name="element">元素</param>
        public void Unshift(TElement element)
        {
            if (AllowInsert(header))
            {
                header.List.UnShift(element);
            }
            else
            {
                var node = CreateNode();
                node.List.InsertAt(element, 0);
                InsertNode(header, node, false);
            }
            ++Count;
        }

        /// <summary>
        /// 移除并返回列表的尾部元素
        /// </summary>
        /// <returns>元素</returns>
        public TElement Pop()
        {
            if (tail != null)
            {
                return ListPop(tail, false);
            }
            return default(TElement);
        }

        /// <summary>
        /// 移除并返回列表头部的元素
        /// </summary>
        /// <returns>元素</returns>
        public TElement Shift()
        {
            if (header != null)
            {
                return ListPop(header, true);
            }
            return default(TElement);
        }

        /// <summary>
        /// 通过下标访问元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TElement this[long index]
        {
            get
            {
                int offset;
                var node = FindByIndex(index , out offset);
                return node.List[offset];
            }
            set
            {
                int offset;
                var node = FindByIndex(index, out offset);
                node.List.ReplaceAt(value, offset);
            }
        }

        /// <summary>
        /// 在指定元素之后插入
        /// </summary>
        /// <param name="finder">查找的元素</param>
        /// <param name="insert">要插入的元素</param>
        public void InsertAfter(TElement finder, TElement insert)
        {
            InsertByElement(finder, insert, true);
        }

        /// <summary>
        /// 在指定元素之前插入
        /// </summary>
        /// <param name="finder">查找的元素</param>
        /// <param name="insert">要插入的元素</param>
        public void InsertBefore(TElement finder, TElement insert)
        {
            InsertByElement(finder, insert, false);
        }

        /// <summary>
        /// 正向迭代
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            var node = header;
            while (node != null)
            {
                for (var i = 0; i < node.List.Count; ++i)
                {
                    yield return node.List[i];
                }
                node = node.Forward;
            }
        }

        /// <summary>
        /// 反向迭代
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator<TElement> ReversEnumerator()
        {
            var node = tail;
            while (node != null)
            {
                for (var i = node.List.Count - 1; i >= 0; --i)
                {
                    yield return node.List[i];
                }
                node = node.Backward;
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
        /// 根据下标查找元素
        /// </summary>
        /// <param name="index">下标</param>
        /// <returns>元素</returns>
        private QuickListNode FindByIndex(long index ,out int offset)
        {
            long sumIndex = 0;
            var node = header;
            while (node != null)
            {
                if ((sumIndex += node.List.Count) < index)
                {
                    node = node.Forward;
                    continue;
                }
                for (var i = 0; i < node.List.Count; ++i)
                {
                    if (index == sumIndex++)
                    {
                        offset = i;
                        return node;
                    }
                }
                node = node.Forward;
            }
            offset = -1;
            return null;
        }

        /// <summary>
        /// 从头往尾查找到指定元素并插入
        /// </summary>
        /// <param name="finder">用于查找的元素</param>
        /// <param name="insert">被插入的元素</param>
        /// <param name="after">是否在被查找的元素之后插入</param>
        private void InsertByElement(TElement finder, TElement insert, bool after)
        {
            int offset;
            //find获取的偏移量
            var node = FindNode(finder, out offset);
            if (node == null)
            {
                return;
            }

            bool full, fullNext, fullBackward, atTail, atHead;
            full = fullNext = fullBackward = atTail = atHead = false;
            QuickListNode newNode;

            //如果结点不能插入那么标记为满
            if (!AllowInsert(node))
            {
                full = true;
            }

            if (after && (offset + 1) == node.List.Count)
            {
                //标记为尾部的元素
                atTail = true;
                //同时如果后面的结点也不能插入那么标记后置结点已满
                if (!AllowInsert(node.Forward))
                {
                    fullNext = true;
                }
            }

            if (!after && (offset == 0))
            {
                //标记为头部元素
                atHead = true;
                //同时如果之前的结点也不能插入那么标记前置结点已满
                if (!AllowInsert(node.Backward))
                {
                    fullBackward = true;
                }
            }

            //如果结点没有满，且是后插式插入
            if (!full && after)
            {
                if (offset + 1 < node.List.Count)
                {
                    //如果偏移量的位置之后还存在元素
                    node.List.InsertAt(insert, offset + 1);
                }
                else
                {
                    //如果之后没有元素那么直接推入
                    node.List.Push(insert);
                }
            }
            else if (!full && !after)
            {
                //结点没有满，且是前插式
                node.List.InsertAt(insert, offset);
            }
            else if (full && atTail && node.Forward != null && !fullNext && after)
            {
                //如果当前结点满了，且是后插入尾部元素，并且下一个结点存在而且不是满的
                //那么就会插入到下一个结点中的头部
                newNode = node.Forward;
                newNode.List.UnShift(insert);
            }
            else if (full && atHead && node.Backward != null && !fullBackward && !after)
            {
                //如果当前结点满了，且是前插入头部元素，并且上一个结点存在而且不是满的
                //那么就会插入到上一个结点中的尾部
                newNode = node.Backward;
                newNode.List.Push(insert);
            }
            else if (full &&
                     ((atTail && node.Forward != null && fullNext && after) ||
                      (atHead && node.Backward != null && fullBackward && !after)))
            {
                //如果当前结点是满的，且前置结点和后置结点都是满的那么
                //就新建一个结点，插入在2个结点之间
                newNode = CreateNode();
                newNode.List.InsertAt(insert, 0);
                InsertNode(node, newNode, after);
            }
            else if (full)
            {
                //如果当前结点是满的，且插入的元素不处于头部或者尾部的位置
                //那么拆分数据
                newNode = SplitNode(node, offset, after);
                if (after)
                {
                    newNode.List.UnShift(insert);
                }
                else
                {
                    newNode.List.Push(insert);
                }
                InsertNode(node, newNode, after);
                AttemptMergeNode(node);
            }

            ++Count;
        }

        /// <summary>
        /// 尝试合并结点
        /// </summary>
        /// <param name="node">发起合并的结点</param>
        private void AttemptMergeNode(QuickListNode node)
        {
            QuickListNode backward, backwardBackward, forward, forwardForward;
            backward = backwardBackward = forward = forwardForward = null;

            if (node.Backward != null)
            {
                backward = node.Backward;
                if (backward.Backward != null)
                {
                    backwardBackward = backward.Backward;
                }
            }

            if (node.Forward != null)
            {
                forward = node.Forward;
                if (forward.Forward != null)
                {
                    forwardForward = forward.Forward;
                }
            }

            if (AllowMerge(backward, backwardBackward))
            {
                MergeNode(backward, backwardBackward, false);
                backward = backwardBackward = null;
            }

            if (AllowMerge(forward, forwardForward))
            {
                MergeNode(forward, forwardForward, true);
                forward = forwardForward = null;
            }

            if (AllowMerge(node, node.Backward))
            {
                MergeNode(node, node.Backward, false);
            }

            if (AllowMerge(node, node.Forward))
            {
                MergeNode(node, node.Forward, true);
            }
        }

        /// <summary>
        /// 将从结点合并进主节点
        /// </summary>
        /// <param name="master">主结点</param>
        /// <param name="slave">从结点</param>
        /// <param name="after">从结点将怎么合并</param>
        private void MergeNode(QuickListNode master, QuickListNode slave, bool after)
        {
            master.List.Merge(slave.List, after);
            DeleteNode(slave);
        }

        /// <summary>
        /// 是否允许进行合并
        /// </summary>
        /// <param name="a">结点</param>
        /// <param name="b">结点</param>
        /// <returns>是否可以合并</returns>
        private bool AllowMerge(QuickListNode a, QuickListNode b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            return a.List.Count + b.List.Count < (fill * MERGE_PRO);
        }

        /// <summary>
        /// 拆分结点
        /// </summary>
        /// <param name="node">要被拆分的结点</param>
        /// <param name="offset">拆分偏移量</param>
        /// <param name="after">前拆将会将offset之前的元素作为返回结点，后拆分则会将offset之后的元素作为返回结点</param>
        /// <returns>拆分出的结点</returns>
        private QuickListNode SplitNode(QuickListNode node, int offset, bool after)
        {
            var newNode = CreateNode();
            newNode.List.Init(node.List.Split(offset, after));
            return newNode;
        }

        /// <summary>
        /// 查找元素所在结点
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="offset">偏移量</param>
        /// <returns>所在结点，如果找不到结点则返回null</returns>
        private QuickListNode FindNode(TElement element, out int offset)
        {
            var node = header;
            while (node != null)
            {
                for (var i = 0; i < node.List.Count; ++i)
                {
                    if (node.List[i].Equals(element))
                    {
                        offset = i;
                        return node;
                    }
                }
                node = node.Forward;
            }
            offset = -1;
            return null;
        }

        /// <summary>
        /// 列表弹出数据
        /// </summary>
        /// <param name="node">结点</param>
        /// <param name="head">是否是头部</param>
        private TElement ListPop(QuickListNode node, bool head)
        {
            TElement ele;
            if (head)
            {
                ele = node.List.Shift();
            }
            else
            {
                ele = node.List.Pop();
            }
            if (node.List.Count <= 0)
            {
                DeleteNode(node);
            }
            --Count;
            return ele;
        }

        /// <summary>
        /// 删除结点
        /// </summary>
        /// <param name="node">结点</param>
        private void DeleteNode(QuickListNode node)
        {
            if (node.Forward != null)
            {
                node.Forward.Backward = node.Backward;
            }
            if (node.Backward != null)
            {
                node.Backward.Forward = node.Forward;
            }
            if (node == tail)
            {
                tail = node.Backward;
            }
            if (node == header)
            {
                header = node.Forward;
            }
            Count -= node.List.Count;
            --Length;
        }

        /// <summary>
        /// 插入结点
        /// </summary>
        /// <param name="oldNode">旧的结点</param>
        /// <param name="newNode">新的结点</param>
        /// <param name="after">在旧的结点之前还是之后</param>
        private void InsertNode(QuickListNode oldNode, QuickListNode newNode, bool after)
        {
            if (after)
            {
                newNode.Backward = oldNode;
                if (oldNode != null)
                {
                    newNode.Forward = oldNode.Forward;
                    if (oldNode.Forward != null)
                    {
                        oldNode.Forward.Backward = newNode;
                    }
                    oldNode.Forward = newNode;
                }
                if (tail == oldNode)
                {
                    tail = newNode;
                }
            }
            else
            {
                newNode.Forward = oldNode;
                if (oldNode != null)
                {
                    newNode.Backward = oldNode.Backward;
                    if (oldNode.Backward != null)
                    {
                        oldNode.Backward.Forward = newNode;
                    }
                    oldNode.Backward = newNode;
                }
                if (header == oldNode)
                {
                    header = newNode;
                }
            }
            if (Length == 0)
            {
                header = tail = newNode;
            }
            ++Length;
        }

        /// <summary>
        /// 创建结点
        /// </summary>
        /// <returns></returns>
        private QuickListNode CreateNode()
        {
            return new QuickListNode
            {
                List = new InternalList<TElement>(fill),
            };
        }

        /// <summary>
        /// 快速列表结点是否允许插入
        /// </summary>
        /// <param name="node">结点</param>
        /// <returns>是否可以插入</returns>
        private bool AllowInsert(QuickListNode node)
        {
            if (node == null)
            {
                return false;
            }
            return node.List.Count < fill;
        }
    }
}
