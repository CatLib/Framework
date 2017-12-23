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

using CatLib.API.Debugger;
using CatLib.API.Json;
using CatLib.API.Routing;
using CatLib.Debugger.WebConsole.Protocol;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CatLib.Debugger.WebConsole
{
    /// <summary>
    /// http调试控制台
    /// </summary>
    [Routed("debug://http-debugger-console")]
    public sealed class HttpDebuggerConsole
    {
        /// <summary>
        /// http监听器
        /// </summary>
        private HttpListener listener;

        /// <summary>
        /// 路由器
        /// </summary>
        private readonly IRouter router;

        /// <summary>
        /// json处理器
        /// </summary>
        private readonly IJson json;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// 当前唯一标识符
        /// </summary>
        private readonly string guid;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// http调试控制台
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="router">路由器</param>
        /// <param name="json">json解析器</param>
        public HttpDebuggerConsole([Inject(Required = true)]ILogger logger,
                                    [Inject(Required = true)]IRouter router,
                                     [Inject(Required = true)]IJson json)
        {
            Guard.Requires<AssertException>(logger != null);
            Guard.Requires<AssertException>(router != null);
            Guard.Requires<AssertException>(json != null);

            this.logger = logger;
            this.router = router;
            this.json = json;
            guid = Guid.NewGuid().ToString();
            RegisterNotFoundRouted();
        }

        /// <summary>
        /// 当析构时
        /// </summary>
        ~HttpDebuggerConsole()
        {
            Stop();
        }

        /// <summary>
        /// 获取控制台Guid
        /// </summary>
        /// <param name="response">响应</param>
        [Routed("get-guid")]
        public void GetGuid(IResponse response)
        {
            response.SetContext(new GetGuid(new Dictionary <string,string>
            {
                { "guid" , guid }
            }));
        }

        /// <summary>
        /// 开启控制台服务
        /// </summary>
        /// <param name="host">监听host</param>
        /// <param name="port">监听端口</param>
        public void Start(string host = "*", int port = 9478)
        {
            lock (syncRoot)
            {
                if (listener != null)
                {
                    listener.Dispose();
                }
                listener = new HttpListener(host, port);
                listener.OnRequest += OnRequest;
            }
        }

        /// <summary>
        /// 停止控制台服务
        /// </summary>
        public void Stop()
        {
            lock (syncRoot)
            {
                if (listener != null)
                {
                    listener.Dispose();
                }
            }
        }

        /// <summary>
        /// 当收到来自控制端的请求时
        /// </summary>
        /// <param name="context">请求上下文</param>
        private void OnRequest(HttpListenerContext context)
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            try
            {
                DispatchToRouted(context);
            }
            catch (NotFoundRouteException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.Response.OutputStream.Close();
        }

        /// <summary>
        /// 调度到目标路由
        /// </summary>
        /// <param name="context">请求上下文</param>
        private void DispatchToRouted(HttpListenerContext context)
        {
            var segments = context.Request.Url.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (segments.Length < 2)
            {
                return;
            }
            var scheme = segments[0];
            var path = string.Join("/", segments, 1, segments.Length - 1);
            var uri = string.Format("{0}://{1}", scheme, path);
            var response = router.Dispatch(uri);

            if (response == null)
            {
                return;
            }

            var whileCount = 0;
            while (response.GetContext() == null && whileCount++ < 1000)
            {
                System.Threading.Thread.Sleep(1);
            }

            RoutedResponseHandler(context, response);
        }

        /// <summary>
        /// 路由响应处理器
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <param name="response">路由响应</param>
        private void RoutedResponseHandler(HttpListenerContext context, IResponse response)
        {
            var consoleResponse = response.GetContext() as IWebConsoleResponse;
            if (consoleResponse == null)
            {
                return;
            }
            var data = json.Encode(new BaseProtocol(consoleResponse.Response));
            var bytes = Encoding.UTF8.GetBytes(data);
            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 注册路由不存在
        /// </summary>
        private void RegisterNotFoundRouted()
        {
            router.OnNotFound((request, next) =>
            {
                logger.Debug("can not find routed [{0}]", request.Uri.OriginalString);
                next(request);
            });
            router.OnError((request, response, exception, next) =>
            {
                logger.Error("routed trigger error ,request [{0}][{1}]", request.ToString(), exception.Message);
                next(request, response, exception);
            });
        }
    }
}
