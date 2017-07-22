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

namespace CatLib.Debugger.WebConsole.Protocol
{
    /// <summary>
    /// GetGuid API
    /// </summary>
    internal sealed class GetGuid : IWebConsoleResponse
    {
        /// <summary>
        /// 响应
        /// </summary>
        public object Response { get; private set; }

        /// <summary>
        /// GetGuid API
        /// </summary>
        /// <param name="response">响应</param>
        public GetGuid(object response)
        {
            Response = response;
        }
    }
}
