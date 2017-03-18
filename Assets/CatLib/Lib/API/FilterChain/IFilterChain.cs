
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
    }

    /// <summary>
    /// 过滤器链
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IFilterChain<TIn>
    {

        IFilterChain<TIn> Add(Action<TIn, IFilterChain<TIn>> filter);

        IFilterChain<TIn> Add(IFilter<TIn> filter);

        IFilterChain<TIn> Remove(IFilter<TIn> filter);

        void Do(TIn inData);

    }

    /// <summary>
    /// 过滤器链
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IFilterChain<TIn, TOut>
    {

        IFilterChain<TIn, TOut> Add(Action<TIn, TOut, IFilterChain<TIn, TOut>> filter);

        IFilterChain<TIn, TOut> Add(IFilter<TIn, TOut> filter);

        IFilterChain<TIn, TOut> Remove(IFilter<TIn, TOut> filter);

        void Do(TIn inData, TOut outData);

    }

}