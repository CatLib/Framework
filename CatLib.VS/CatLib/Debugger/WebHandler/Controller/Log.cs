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

namespace CatLib.Debugger.WebHandler.Controller
{
    /// <summary>
    /// 日志
    /// </summary>
    [Routed("debug://log")]
    public sealed class Log
    {
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="logger">日志</param>
        [Routed("get-logger/{lastId?}", Defaults = "lastId=>0")]
        public void GetLogger(IRequest request, IResponse response , Logger logger)
        {
            response.SetContext("hello world");
        }

        /// <summary>
        /// 获取分组信息
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="logger">日志</param>
        [Routed("get-catergroy")]
        public void GetCategroy(IRequest request, IResponse response, Logger logger)
        {
            response.SetContext("hello world");
        }
    }
}
