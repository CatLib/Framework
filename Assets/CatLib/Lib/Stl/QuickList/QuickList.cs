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
    /// 快速列表
    /// </summary>
    /// <typeparam name="TElement">元素</typeparam>
    public sealed class QuickList<TElement> : IEnumerable<TElement>
    {
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
            /// 结点数量
            /// </summary> 
            internal long Count;

            /// <summary>
            /// 列表
            /// </summary>
            internal TElement[] List;
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
        public QuickList(int fill = 256)
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
                tail.List[tail.Count] = element;
            }
            else
            {
                var node = CreateNode();
                node.List[node.Count] = element;
                InsertNode(tail, node, true);
            }
            ++Count;
            ++tail.Count;
        }

        /// <summary>
        /// 将元素插入到列表头部
        /// </summary>
        /// <param name="element">元素</param>
        public void Unshift(TElement element)
        {
            if (AllowInsert(header))
            {
                Array.Copy(header.List, 0, header.List, 1, header.Count);
                header.List[0] = element;
            }
            else
            {
                var node = CreateNode();
                node.List[0] = element;
                InsertNode(header, node, false);
            }
            ++Count;
            ++header.Count;
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
        /// 正向迭代
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            var node = header;
            while (node != null)
            {
                for (var i = 0; i < node.Count; ++i)
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
                for (var i = node.Count - 1; i >= 0; --i)
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
        /// 列表弹出数据
        /// </summary>
        /// <param name="node">结点</param>
        /// <param name="head">是否是头部</param>
        private TElement ListPop(QuickListNode node, bool head)
        {
            TElement ele;
            --node.Count;
            if (head)
            {
                ele = node.List[0];
                node.List[0] = default(TElement);
                Array.Copy(node.List, 1, node.List, 0, node.Count);
            }
            else
            {
                ele = node.List[node.Count];
                node.List[node.Count] = default(TElement);
            }
            if (node.Count <= 0)
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
            Count -= node.Count;
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
                List = new TElement[fill]
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
            return node.Count < fill;
        }
    }
}
