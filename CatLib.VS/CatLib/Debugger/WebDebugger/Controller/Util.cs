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

using CatLib.API.Debugger;
using CatLib.API.Routing;
using CatLib.Debugger.WebDebugger.Protocol;

namespace CatLib.Debugger.WebDebugger.Controller
{
    /// <summary>
    /// 通用
    /// </summary>
    [Routed("debug://util")]
    public class Util
    {
        /// <summary>
        /// 回显
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="logger">日志系统</param>
        [Routed]
        public void Echo(IRequest request , IResponse response , Logger logger)
        {
            if (logger != null)
            {
                logger.Debug(request.Uri.OriginalString);
                return;
            }
            var logEntry = new LogEntry(LogLevels.Debug, request.Uri.OriginalString, 1);
            var outputs = new WebConsoleOutputs();
            outputs.WriteLine(logEntry);
            response.SetContext(outputs);
        }
    }
}
