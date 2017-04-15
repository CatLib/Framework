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
    /// <typeparam name="TIn">进入参数</typeparam>
    public interface IFilter<TIn>
    {
        /// <summary>
        /// 执行过滤器
        /// </summary>
        /// <param name="inData">输入数据</param>
        /// <param name="next">下一层过滤器</param>
        void Do(TIn inData, IFilterChain<TIn> next);
    }

    /// <summary>
    /// 过滤器
    /// </summary>
    /// <typeparam name="TIn">进入参数</typeparam>
    /// <typeparam name="TOut">输出参数</typeparam>
    public interface IFilter<TIn, TOut>
    {
        /// <summary>
        /// 执行过滤器
        /// </summary>
        /// <param name="inData">输入参数</param>
        /// <param name="outData">输出参数</param>
        /// <param name="next">下一层过滤器</param>
        void Do(TIn inData, TOut outData, IFilterChain<TIn, TOut> next);
    }

    /// <summary>
    /// 过滤器
    /// </summary>
    /// <typeparam name="TIn">进入参数</typeparam>
    /// <typeparam name="TOut">输出参数</typeparam>
    /// <typeparam name="TException">输入异常</typeparam>
    public interface IFilter<TIn, TOut, TException>
    {
        /// <summary>
        /// 执行过滤器
        /// </summary>
        /// <param name="inData">输入参数</param>
        /// <param name="outData">输出参数</param>
        /// <param name="exception">输入异常</param>
        /// <param name="next">下一层过滤器</param>
        void Do(TIn inData, TOut outData, TException exception, IFilterChain<TIn, TOut, TException> next);
    }
}