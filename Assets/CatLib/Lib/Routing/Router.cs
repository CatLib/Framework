
using System;
using System.Collections.Generic;
using CatLib.API.Event;
using CatLib.API.Container;
using CatLib.API.Routing;
using CatLib.API.FilterChain;

namespace CatLib.Routing
{

    /// <summary>
    /// 路由服务
    /// </summary>
    public class Router : IRouter
    {

        public const char SEPARATOR = '/';

        /// <summary>
        /// 全局调度器
        /// </summary>
        protected IEvent events;

        /// <summary>
        /// 容器
        /// </summary>
        protected IContainer container;

        /// <summary>
        /// 协议方案
        /// </summary>
        protected Dictionary<string, Scheme> schemes;

        /// <summary>
        /// 路由请求过滤链
        /// </summary>
        protected IFilterChain middleware;

        /// <summary>
        /// 创建一个新的路由器
        /// </summary>
        /// <param name="events"></param>
        /// <param name="container"></param>
        public Router(IEvent events , IContainer container)
        {
            this.events = events;
            this.container = container;
            schemes = new Dictionary<string, Scheme>();
        }

        /// <summary>
        /// 注册一个路由方案
        /// </summary>
        /// <param name="scheme">方案</param>
        /// <param name="url">统一资源定位符</param>
        /// <param name="action">行为</param>
        /// <returns></returns>
        public IRoutingBind Reg(string scheme, string url, Action<IRequest, IResponse> action)
        {

            if (!schemes.ContainsKey(scheme)) { throw new NotFoundSchemeException("scheme: [" + scheme + "] is not exists"); }

            var route = new Route(Prefix(url), action);
            schemes[scheme].AddRoute(route);

            return route;

        }

        /// <summary>
        /// 当路由没有找到时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public IRouter OnNotFound(Func<IRequest, bool> middleware)
        {
            return this;
        }

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public IRouter OnError(Func<IRequest, Exception, bool> middleware)
        {
            return this;
        }

        /// <summary>
        /// 调度路由
        /// </summary>
        /// <param name="uri">路由地址</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public IResponse Dispatch(string uri, object context = null)
        {
            Request request = CreateRequest(uri, context);

            if (!schemes.ContainsKey(request.Scheme))
            {
                throw new NotFoundSchemeException("scheme: [" + request.Scheme + "] is not exists");
            }

            Route route = FindRoute(request);
            request.SetRoute(route);
            
            //todo: dispatch event
            //events.Event.Trigger();



            return null;
        }

        /// <summary>
        /// 执行请求路由
        /// </summary>
        /// <returns></returns>
        protected IResponse RunRouteWithinStack(Route route, Request request)
        {

            return null;

        }

        /// <summary>
        /// 准备响应的内容
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <returns></returns>
        protected IResponse PrepareResponse(IRequest request, IResponse response)
        {
            return response;
        }

        /// <summary>
        /// 查找一个合适的路由
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected Route FindRoute(Request request)
        {
            Route route = schemes[request.Scheme].Match(request);
            container.Instance(typeof(Route).ToString(), route);
            return route;
        }

        /// <summary>
        /// 创建请求
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected Request CreateRequest(string uri , object context)
        {
            return new Request(uri, context);
        }

        /// <summary>
        /// 处理uri为符合规则的url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected string Prefix(string url)
        {
            return (GetLastGroupPrefix().Trim(SEPARATOR) + SEPARATOR + url.Trim(SEPARATOR)).Trim(SEPARATOR) ?? SEPARATOR.ToString();
        }

        /// <summary>
        /// 获取最后的分组信息
        /// </summary>
        /// <returns></returns>
        protected string GetLastGroupPrefix()
        {
            return string.Empty;
        }



    }

}
