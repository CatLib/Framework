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

using System.Collections.Generic;
using CatLib.Debugger.WebConsole;

namespace CatLib.Debugger.WebLog.Protocol
{
    /// <summary>
    /// 获取Guid
    /// </summary>
    internal sealed class GetGuid : IWebConsoleResponse
    {
        /// <summary>
        /// 响应
        /// </summary>
        public object Response
        {
            get { return outputs; }
        }

        /// <summary>
        /// 输出
        /// </summary>
        private readonly IDictionary<string, string> outputs;

        /// <summary>
        /// 获取分组API
        /// </summary>
        public GetGuid(string guid)
        {
            outputs = new Dictionary<string, string> {{"guid", guid}};
        }
    }
}
