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

namespace CatLib.API.Routing
{
    /// <summary>
    /// 路由配置异常
    /// </summary>
    public class RouterConfigException : CatLibException
    {
        /// <summary>
        /// 路由配置异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public RouterConfigException(string message) : base(message) { }
    }
}