﻿/*
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

namespace CatLib.API.FilterChain
{
    /// <summary>
    /// 过滤器链
    /// </summary>
    public interface IFilterChain
    {
        /// <summary>
        /// 创建过滤器链
        /// </summary>
        /// <typeparam name="TIn">输入参数类型</typeparam>
        /// <returns>过滤器链</returns>
        IFilterChain<TIn> Create<TIn>();

        /// <summary>
        /// 创建过滤器链
        /// </summary>
        /// <typeparam name="TIn">输入参数类型</typeparam>
        /// <typeparam name="TOut">输出参数类型</typeparam>
        /// <returns>过滤器链</returns>
        IFilterChain<TIn, TOut> Create<TIn, TOut>();

        /// <summary>
        /// 创建过滤器链
        /// </summary>
        /// <typeparam name="TIn">输入参数类型</typeparam>
        /// <typeparam name="TOut">输出参数类型</typeparam>
        /// <typeparam name="TException">异常参数类型</typeparam>
        /// <returns>过滤器链</returns>
        IFilterChain<TIn, TOut, TException> Create<TIn, TOut, TException>();
    }

    /// <summary>
    /// 过滤器链
    /// </summary>
    /// <typeparam name="TIn">输入参数</typeparam>
    public interface IFilterChain<TIn>
    {
        /// <summary>
        /// 过滤器列表
        /// </summary>
        Action<TIn, Action<TIn>>[] FilterList { get; }

        /// <summary>
        /// 增加过滤器
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns>过滤器链</returns>
        IFilterChain<TIn> Add(Action<TIn, Action<TIn>> filter);

        /// <summary>
        /// 执行过滤器链
        /// </summary>
        /// <param name="inData">输入数据</param>
        void Do(TIn inData);

        /// <summary>
        /// 执行过滤器链
        /// </summary>
        /// <param name="inData">输入数据</param>
        /// <param name="then">当过滤器执行完成后执行的操作</param>
        void Do(TIn inData, Action<TIn> then);
    }

    /// <summary>
    /// 过滤器链
    /// </summary>
    /// <typeparam name="TIn">输入参数</typeparam>
    /// <typeparam name="TOut">输出参数</typeparam>
    public interface IFilterChain<TIn, TOut>
    {
        /// <summary>
        /// 过滤器列表
        /// </summary>
        Action<TIn, TOut, Action<TIn, TOut>>[] FilterList { get; }

        /// <summary>
        /// 增加一个过滤器
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns>过滤器链</returns>
        IFilterChain<TIn, TOut> Add(Action<TIn, TOut, Action<TIn, TOut>> filter);

        /// <summary>
        /// 执行过滤器链
        /// </summary>
        /// <param name="inData">输入参数</param>
        /// <param name="outData">输出参数</param>
        void Do(TIn inData, TOut outData);

        /// <summary>
        /// 执行过滤器链
        /// </summary>
        /// <param name="inData">输入参数</param>
        /// <param name="outData">输出参数</param>
        /// <param name="then">当过滤器执行完成后执行的操作</param>
        void Do(TIn inData, TOut outData, Action<TIn, TOut> then);
    }

    /// <summary>
    /// 过滤器链
    /// </summary>
    /// <typeparam name="TIn">输入参数</typeparam>
    /// <typeparam name="TOut">输出参数</typeparam>
    /// <typeparam name="TException">输入异常</typeparam>
    public interface IFilterChain<TIn, TOut, TException>
    {
        /// <summary>
        /// 过滤器列表
        /// </summary>
        Action<TIn, TOut, TException, Action<TIn, TOut, TException>>[] FilterList { get; }

        /// <summary>
        /// 增加一个过滤器
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns>过滤器链</returns>
        IFilterChain<TIn, TOut, TException> Add(Action<TIn, TOut, TException, Action<TIn, TOut, TException>> filter);

        /// <summary>
        /// 执行过滤器链
        /// </summary>
        /// <param name="inData">输入参数</param>
        /// <param name="outData">输出参数</param>
        /// <param name="exception">输入异常</param>
        void Do(TIn inData, TOut outData, TException exception);

        /// <summary>
        /// 执行过滤器链
        /// </summary>
        /// <param name="inData">输入参数</param>
        /// <param name="outData">输出参数</param>
        /// <param name="exception">输入异常</param>
        /// <param name="then">当过滤器执行完成后执行的操作</param>
        void Do(TIn inData, TOut outData, TException exception, Action<TIn, TOut, TException> then);
    }
}