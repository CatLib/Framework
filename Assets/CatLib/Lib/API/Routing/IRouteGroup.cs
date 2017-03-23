using System;
using CatLib.API.FilterChain;

namespace CatLib.API.Routing{

	/// <summary>
	/// 路由组接口
	/// </summary>
	public interface IRouteGroup{

		/// <summary>
        /// 设定参数的默认值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="val">参数值</param>
        IRouteGroup Defaults(string name, string val);

        /// <summary>
        /// 约束指定参数必须符合正则表达式
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        IRouteGroup Where(string name, string pattern);

        /// <summary>
        /// 在路由中间件
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <returns></returns>
        IRouteGroup Middleware(Action<IRequest, IResponse, IFilterChain<IRequest, IResponse>> middleware);

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        IRouteGroup OnError(Action<IRequest, Exception, IFilterChain<IRequest, Exception>> onError);

	}

}