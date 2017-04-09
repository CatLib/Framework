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

namespace CatLib.API.FilterChain
{

    /// <summary>
    /// 过滤器链
    /// </summary>
    public interface IFilterChain
    {

        IFilterChain<TIn> Create<TIn>();

        IFilterChain<TIn, TOut> Create<TIn, TOut>();

        IFilterChain<TIn, TOut , TException> Create<TIn, TOut, TException>();
    }

    /// <summary>
    /// 过滤器链
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IFilterChain<TIn>
    {

        Action<TIn, Action<TIn>>[] FilterList { get; }

        IFilterChain<TIn> Add(Action<TIn, Action<TIn>> filter);

        void Do(TIn inData);

        void Do(TIn inData, Action<TIn> then);

    }

    /// <summary>
    /// 过滤器链
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IFilterChain<TIn, TOut>
    {

        Action<TIn, TOut, Action<TIn, TOut>>[] FilterList { get; }

        IFilterChain<TIn, TOut> Add(Action<TIn, TOut, Action<TIn, TOut>> filter);

        void Do(TIn inData , TOut outData);

        void Do(TIn inData , TOut outData, Action<TIn, TOut> then);

    }

    /// <summary>
    /// 过滤器链
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IFilterChain<TIn, TOut, TException>
    {

        Action<TIn, TOut , TException, Action<TIn, TOut , TException>>[] FilterList { get; }

        IFilterChain<TIn, TOut , TException> Add(Action<TIn, TOut, TException, Action<TIn, TOut , TException>> filter);

        void Do(TIn inData, TOut outData , TException exception);

        void Do(TIn inData, TOut outData , TException exception, Action<TIn, TOut , TException> then);

    }

}