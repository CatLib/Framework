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
using System.Net;
using System.Text;
using CatLib.API;
using CatLib.API.Debugger;
using CatLib.API.Json;
using CatLib.API.Routing;

namespace CatLib.Debugger.WebConsole
{
    /// <summary>
    /// http调试控制台
    /// </summary>
    internal sealed class HttpDebuggerConsole : IDestroy
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
        /// http调试控制台
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="router">路由器</param>
        /// <param name="json">json解析器</param>
        public HttpDebuggerConsole(ILogger logger, IRouter router, IJson json)
        {
            if (router == null || json == null)
            {
                return;
            }

            this.logger = logger;
            this.router = router;
            this.json = json;
        }

        /// <summary>
        /// 开启控制台服务
        /// </summary>
        /// <param name="host">监听host</param>
        /// <param name="port">监听端口</param>
        public void Start(string host = "*", ushort port = 9478)
        {
            if (listener != null)
            {
                listener.Dispose();
            }
            listener = new HttpListener(host, port);
            listener.OnRequest += OnRequest;
        }

        /// <summary>
        /// 停止控制台服务
        /// </summary>
        public void Stop()
        {
            if (listener != null)
            {
                listener.Dispose();
            }
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public void OnDestroy()
        {
            Stop();
        }

        /// <summary>
        /// 当收到来自控制端的请求时
        /// </summary>
        /// <param name="context">请求上下文</param>
        private void OnRequest(HttpListenerContext context)
        {
            try
            {
                DispatchToRouted(context);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Close();
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.OutputStream.Close();
                logger.Debug("request has error [" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 调度到目标路由
        /// </summary>
        /// <param name="context">请求上下文</param>
        private void DispatchToRouted(HttpListenerContext context)
        {
            var segments = context.Request.Url.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (segments.Length >= 2)
            {
                var scheme = segments[0];
                var path = string.Join("/", segments, 1, segments.Length - 1);
                var uri = string.Format("{0}://{1}", scheme, path);
                var response = router.Dispatch(uri);
                RoutedResponseHandler(context, response);
            }
        }

        /// <summary>
        /// 路由响应处理器
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <param name="response">路由响应</param>
        private void RoutedResponseHandler(HttpListenerContext context, IResponse response)
        {
            var consoleResponse = response.GetContext() as IWebConsoleResponse;
            if (consoleResponse != null)
            {
                var data = json.Encode(new BaseProtocol(consoleResponse.Response));
                var bytes = Encoding.UTF8.GetBytes(data);
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
