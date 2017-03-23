
using System;
using CatLib.API.FilterChain;
using CatLib.API.Routing;

namespace CatLib.Routing{
	
	/// <summary>
	/// 路由组
	/// </summary>
	public class RouteGroup : IRouteGroup{

		/// <summary>
        /// 设定参数的默认值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="val">参数值</param>
        public IRouteGroup Defaults(string name, string val){

			return this;

		}

        /// <summary>
        /// 约束指定参数必须符合正则表达式
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IRouteGroup Where(string name, string pattern){

			return this;

		}

        /// <summary>
        /// 在路由中间件
        /// </summary>
        /// <param name="middleware">中间件</param>
        /// <returns></returns>
        public IRouteGroup Middleware(Action<IRequest, IResponse, IFilterChain<IRequest, IResponse>> middleware){

			return this;

		}

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public IRouteGroup OnError(Action<IRequest, Exception, IFilterChain<IRequest, Exception>> onError){

			return this;

		}



	}

}