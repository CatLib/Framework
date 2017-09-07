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

namespace CatLib.Debugger.WebConsole
{
    /// <summary>
    /// 基础协议
    /// </summary>
    internal sealed class BaseProtocol
    {
        /// <summary>
        /// 数据
        /// </summary>
        public object Response;

        /// <summary>
        /// 基础协议
        /// </summary>
        /// <param name="response">响应</param>
        public BaseProtocol(object response)
        {
            Response = response;
        }
    }
}
