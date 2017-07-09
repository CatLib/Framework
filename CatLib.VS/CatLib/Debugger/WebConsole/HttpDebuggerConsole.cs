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

using System.Net;
using CatLib.API.Routing;

namespace CatLib.Debugger.WebConsole
{
    /// <summary>
    /// http调试控制台
    /// </summary>
    internal sealed class HttpDebuggerConsole
    {
        /// <summary>
        /// Http监听器
        /// </summary>
        private HttpListener listener;

        /// <summary>
        /// 路由器
        /// </summary>
        private IRouter router;

        /// <summary>
        /// http调试控制台
        /// </summary>
        internal HttpDebuggerConsole(IRouter router)
        {
            if (router == null)
            {
                return;
            }
            listener = new HttpListener();
            listener.OnRequest += OnRequest;
            this.router = router;
        }

        private void OnRequest(HttpListenerContext context)
        {
            try
            {
                var segments = context.Request.Url.AbsolutePath.Split('/');
                
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Close();
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.OutputStream.Close();
            }
        }
    }
}
