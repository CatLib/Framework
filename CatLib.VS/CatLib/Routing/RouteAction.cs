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

using CatLib.API.Routing;
using System;

namespace CatLib.Routing
{
    /// <summary>
    /// 路由行为
    /// </summary>
    internal sealed class RouteAction
    {
        /// <summary>
        /// 路由行为类型
        /// </summary>
        public enum RouteTypes
        {
            /// <summary>
            /// 回调形路由
            /// </summary>
            CallBack,

            /// <summary>
            /// 控制器调用
            /// </summary>
            ControllerCall,
        }

        /// <summary>
        /// 类型
        /// </summary>
        public RouteTypes Type { get; set; }

        /// <summary>
        /// 回调行为
        /// </summary>
        public Action<IRequest, IResponse> Action { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public Type Controller { get; set; }

        /// <summary>
        /// 调度函数名
        /// </summary>
        public string Func { get; set; }
    }
}