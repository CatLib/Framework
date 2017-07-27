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

namespace CatLib.Routing
{
    /// <summary>
    /// 路由事件
    /// </summary>
    public sealed class RouterEvents
    {
        /// <summary>
        /// 当属性路由编译之前
        /// </summary>
        public static readonly string OnBeforeRouterAttrCompiler = "Router.OnBeforeRouterAttrCompiler";

        /// <summary>
        /// 当路由调度之前
        /// </summary>
        public static readonly string OnDispatcher = "Router.OnDispatcher";
    }
}
