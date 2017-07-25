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

namespace CatLib.API.Routing
{
    /// <summary>
    /// 调度事件
    /// </summary>
    public sealed class DispatchEventArgs : EventArgs
    {
        /// <summary>
        /// 路由器
        /// </summary>
        public IRouter Router { get; private set; }

        /// <summary>
        /// 异常
        /// </summary>
        public IRoute Route { get; private set; }

        /// <summary>
        /// 请求
        /// </summary>
        public IRequest Request { get; private set; }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="router">路由器</param>
        /// <param name="route">路由</param>
        /// <param name="request">请求</param>
        public DispatchEventArgs(IRouter router, IRoute route, IRequest request)
        {
            Router = router;
            Route = route;
            Request = request;
        }
    }
}
