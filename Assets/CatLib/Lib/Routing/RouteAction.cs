
using System;
using CatLib.API.Routing;

namespace CatLib.Routing
{

    /// <summary>
    /// 路由行为
    /// </summary>
    public class RouteAction
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
        public string Controller { get; set; }

        /// <summary>
        /// 调度函数名
        /// </summary>
        public string Func { get; set; }

    }

}