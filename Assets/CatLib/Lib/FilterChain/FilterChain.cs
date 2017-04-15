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

using CatLib.API.FilterChain;
using System.Collections.Generic;
using System;

namespace CatLib.FilterChain
{
    /// <summary>
    /// 过滤器链
    /// </summary>
    public sealed class FilterChain : IFilterChain
    {
        /// <summary>
        /// 创建过滤器链
        /// </summary>
        /// <typeparam name="TIn">输入参数类型</typeparam>
        /// <returns>过滤器链</returns>
        public IFilterChain<TIn> Create<TIn>()
        {
            return new FilterChain<TIn>();
        }

        /// <summary>
        /// 创建过滤器链
        /// </summary>
        /// <typeparam name="TIn">输入参数类型</typeparam>
        /// <typeparam name="TOut">输出参数类型</typeparam>
        /// <returns>过滤器链</returns>
        public IFilterChain<TIn, TOut> Create<TIn, TOut>()
        {
            return new FilterChain<TIn, TOut>();
        }

        /// <summary>
        /// 创建过滤器链
        /// </summary>
        /// <typeparam name="TIn">输入参数类型</typeparam>
        /// <typeparam name="TOut">输出参数类型</typeparam>
        /// <typeparam name="TException">输入异常类型</typeparam>
        /// <returns>过滤器链</returns>
        public IFilterChain<TIn, TOut, TException> Create<TIn, TOut, TException>()
        {
            return new FilterChain<TIn, TOut, TException>();
        }
    }

    /// <summary>
    /// 过滤器链
    /// </summary>
    /// <typeparam name="TIn">输入参数</typeparam>
    public sealed class FilterChain<TIn> : IFilterChain<TIn>
    {
        /// <summary>
        /// 过滤器链
        /// </summary>
        private readonly List<Action<TIn, Action<TIn>>> filterList;

        /// <summary>
        /// 过滤器列表
        /// </summary>
        public Action<TIn, Action<TIn>>[] FilterList
        {
            get { return filterList.ToArray(); }
        }

        /// <summary>
        /// 堆栈 用于解决内部递归调用过滤器链所出现的问题
        /// </summary>
        private readonly Stack<int> stack;

        /// <summary>
        /// 构建一个过滤器链
        /// </summary>
        public FilterChain()
        {
            stack = new Stack<int>();
            filterList = new List<Action<TIn, Action<TIn>>>();
        }

        /// <summary>
        /// 增加一个过滤器
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns>过滤器链</returns>
        public IFilterChain<TIn> Add(Action<TIn, Action<TIn>> filter)
        {
            filterList.Add(filter);
            return this;
        }

        /// <summary>
        /// 执行过滤器链
        /// </summary>
        /// <param name="inData">输入数据</param>
        public void Do(TIn inData)
        {
            Do(inData, null);
        }

        /// <summary>
        /// 执行过滤器链
        /// </summary>
        /// <param name="inData">输入数据</param>
        /// <param name="then">当过滤器执行完成后执行的操作</param>
        public void Do(TIn inData, Action<TIn> then)
        {
            if (filterList.Count <= 0)
            {
                if (then != null)
                {
                    then.Invoke(inData);
                }
                return;
            }

            stack.Push(0);
            filterList[0].Invoke(inData, Next(then));
            stack.Pop();
        }

        /// <summary>
        /// 下一层过滤器链
        /// </summary>
        /// <param name="then">当过滤器执行完成后执行的操作</param>
        /// <returns>执行过滤器</returns>
        private Action<TIn> Next(Action<TIn> then)
        {
            return (inData) =>
            {
                var index = stack.Pop();
                stack.Push(++index);
                if (index >= filterList.Count)
                {
                    then.Invoke(inData);
                    return;
                }
                filterList[index].Invoke(inData, Next(then));
            };
        }
    }

    /// <summary>
    /// 过滤器链
    /// </summary>
    /// <typeparam name="TIn">输入参数</typeparam>
    /// <typeparam name="TOut">输出参数</typeparam>
    public class FilterChain<TIn, TOut> : IFilterChain<TIn, TOut>
    {
        /// <summary>
        /// 过滤器链
        /// </summary>
        private readonly List<Action<TIn, TOut, Action<TIn, TOut>>> filterList;

        /// <summary>
        /// 过滤器列表
        /// </summary>
        public Action<TIn, TOut, Action<TIn, TOut>>[] FilterList
        {
            get { return filterList.ToArray(); }
        }

        /// <summary>
        /// 堆栈 用于解决内部递归调用过滤器链所出现的问题
        /// </summary>
        private readonly Stack<int> stack;

        /// <summary>
        /// 构建一个过滤器链
        /// </summary>
        public FilterChain()
        {
            stack = new Stack<int>();
            filterList = new List<Action<TIn, TOut, Action<TIn, TOut>>>();
        }

        /// <summary>
        /// 增加一个过滤器
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns>过滤器链</returns>
        public IFilterChain<TIn, TOut> Add(Action<TIn, TOut, Action<TIn, TOut>> filter)
        {
            filterList.Add(filter);
            return this;
        }

        /// <summary>
        /// 执行过滤器链
        /// </summary>
        /// <param name="inData">输入参数</param>
        /// <param name="outData">输出参数</param>
        public void Do(TIn inData, TOut outData)
        {
            Do(inData, outData, null);
        }

        /// <summary>
        /// 执行过滤器链
        /// </summary>
        /// <param name="inData">输入参数</param>
        /// <param name="outData">输出参数</param>
        /// <param name="then">当过滤器执行完成后执行的操作</param>
        public void Do(TIn inData, TOut outData, Action<TIn, TOut> then)
        {
            if (filterList.Count <= 0)
            {
                if (then != null)
                {
                    then.Invoke(inData, outData);
                }
                return;
            }

            stack.Push(0);
            filterList[0].Invoke(inData, outData, Next(then));
            stack.Pop();
        }

        /// <summary>
        /// 下一层过滤器链
        /// </summary>
        /// <param name="then">当过滤器执行完成后执行的操作</param>
        /// <returns>执行过滤器</returns>
        private Action<TIn, TOut> Next(Action<TIn, TOut> then)
        {
            return (inData, outData) =>
            {
                var index = stack.Pop();
                stack.Push(++index);
                if (index >= filterList.Count)
                {
                    then.Invoke(inData, outData);
                    return;
                }
                filterList[index].Invoke(inData, outData, Next(then));
            };
        }
    }

    /// <summary>
    /// 过滤器链
    /// </summary>
    /// <typeparam name="TIn">输入参数</typeparam>
    /// <typeparam name="TOut">输出参数</typeparam>
    /// <typeparam name="TException">输入异常</typeparam>
    public class FilterChain<TIn, TOut, TException> : IFilterChain<TIn, TOut, TException>
    {
        /// <summary>
        /// 过滤器链
        /// </summary>
        private readonly List<Action<TIn, TOut, TException, Action<TIn, TOut, TException>>> filterList;

        /// <summary>
        /// 过滤器列表
        /// </summary>
        public Action<TIn, TOut, TException, Action<TIn, TOut, TException>>[] FilterList
        {
            get { return filterList.ToArray(); }
        }

        /// <summary>
        /// 堆栈 用于解决内部递归调用过滤器链所出现的问题
        /// </summary>
        private readonly Stack<int> stack;

        /// <summary>
        /// 构建一个过滤器链
        /// </summary>
        public FilterChain()
        {
            stack = new Stack<int>();
            filterList = new List<Action<TIn, TOut, TException, Action<TIn, TOut, TException>>>();
        }

        /// <summary>
        /// 增加一个过滤器
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns>过滤器链</returns>
        public IFilterChain<TIn, TOut, TException> Add(Action<TIn, TOut, TException, Action<TIn, TOut, TException>> filter)
        {
            filterList.Add(filter);
            return this;
        }

        /// <summary>
        /// 执行过滤器链
        /// </summary>
        /// <param name="inData">输入参数</param>
        /// <param name="outData">输出参数</param>
        /// <param name="exception">输入异常</param>
        public void Do(TIn inData, TOut outData, TException exception)
        {
            Do(inData, outData, exception, null);
        }

        /// <summary>
        /// 执行过滤器链
        /// </summary>
        /// <param name="inData">输入参数</param>
        /// <param name="outData">输出参数</param>
        /// <param name="exception">输入异常</param>
        /// <param name="then">当过滤器执行完成后执行的操作</param>
        public void Do(TIn inData, TOut outData, TException exception, Action<TIn, TOut, TException> then)
        {
            if (filterList.Count <= 0)
            {
                if (then != null)
                {
                    then.Invoke(inData, outData, exception);
                }
                return;
            }

            stack.Push(0);
            filterList[0].Invoke(inData, outData, exception, Next(then));
            stack.Pop();
        }

        /// <summary>
        /// 下一层过滤器链
        /// </summary>
        /// <param name="then">当过滤器执行完成后执行的操作</param>
        /// <returns>执行过滤器</returns>
        private Action<TIn, TOut, TException> Next(Action<TIn, TOut, TException> then)
        {
            return (inData, outData, exception) =>
            {
                var index = stack.Pop();
                stack.Push(++index);
                if (index >= filterList.Count)
                {
                    then.Invoke(inData, outData, exception);
                    return;
                }
                filterList[index].Invoke(inData, outData, exception, Next(then));
            };
        }
    }
}
