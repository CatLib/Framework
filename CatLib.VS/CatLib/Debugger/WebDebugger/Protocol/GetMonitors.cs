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
using CatLib.Debugger.Monitor;
using CatLib.Debugger.WebConsole;

namespace CatLib.Debugger.WebDebugger.Protocol
{
    /// <summary>
    /// 获取监控信息
    /// </summary>
    internal sealed class GetMonitors : IWebConsoleResponse
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
        private readonly IList<IDictionary<string, string>> outputs;

        /// <summary>
        /// 获取分组API
        /// </summary>
        public GetMonitors()
        {
            outputs = new List<IDictionary<string, string>>();
        }

        /// <summary>
        /// 写入一条监控信息
        /// </summary>
        /// <param name="handler">处理器</param>
        public void WriteLine(IMonitorHandler handler)
        {
            outputs.Add(new Dictionary<string, string>
            {
                { "name" , handler.Title },
                { "value" , handler.Value },
                { "unit" , handler.Unit }
            });
        }
    }
}
