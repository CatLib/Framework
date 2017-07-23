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
using UnityEngine;
using ILogger = CatLib.API.Debugger.ILogger;

namespace CatLib.Debugger.WebMonitorContent.Controller
{
    /// <summary>
    /// 定位器控制器
    /// </summary>
    [Routed("debug://input-location", Group = "MainThreadCall")]
    public sealed class InputLocation
    {
        /// <summary>
        /// 定位器命令
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="logger">日志</param>
        [Routed("enable/{enable}")]
        public void Enable(IRequest request, ILogger logger)
        {
            if (request.GetBoolean("enable"))
            {
                logger.Debug("Input.location.Start()");
                Input.location.Start();
            }
            else
            {
                logger.Debug("Input.location.Stop()");
                Input.location.Stop();
            }
        }
    }
}
