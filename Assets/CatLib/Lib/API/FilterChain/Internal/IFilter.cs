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
 
namespace CatLib.API.FilterChain
{

    /// <summary>
    /// 过滤器
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    public interface IFilter<TIn>
    {

        void Do(TIn inData, IFilterChain<TIn> chain);

    }

    /// <summary>
    /// 过滤器
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IFilter<TIn, TOut>
    {

        void Do(TIn inData, TOut outData, IFilterChain<TIn, TOut> chain);

    }

    /// <summary>
    /// 过滤器
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IFilter<TIn, TOut ,TException>
    {

        void Do(TIn inData, TOut outData , TException exception, IFilterChain<TIn, TOut , TException> chain);

    }
}