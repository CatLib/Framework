
using CatLib.API.FilterChain;

namespace CatLib.API.Routing
{

    /// <summary>
    /// 中间件
    /// </summary>
    public interface IMiddleware
    {

        /// <summary>
        /// 路由请求过滤链
        /// </summary>
        IFilterChain<IRequest, IResponse> Middleware{ get; }


    }

}