
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
}