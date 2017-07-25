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
    /// 指令
    /// </summary>
    [Routed("debug://command", Group = "DebuggerMainThreadCall")]
    [ExcludeFromCodeCoverage]
    public sealed class Command
    {
        /// <summary>
        /// 陀螺仪命令
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="logger">日志</param>
        [Routed("input-gyro-enable/{enable}")]
        public void GyroEnable(IRequest request, ILogger logger)
        {
            if (request.GetBoolean("enable"))
            {
                logger.Debug("Input.gyro.enabled = true");
                Input.gyro.enabled = true;
            }
            else
            {
                logger.Debug("Input.gyro.enabled = false");
                Input.gyro.enabled = false;
            }
        }

        /// <summary>
        /// 罗盘命令
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="logger">日志</param>
        [Routed("input-compass-enable/{enable}")]
        public void CompassEnable(IRequest request, ILogger logger)
        {
            if (request.GetBoolean("enable"))
            {
                logger.Debug("Input.compass.enabled = true");
                Input.compass.enabled = true;
            }
            else
            {
                logger.Debug("Input.compass.enabled = false");
                Input.compass.enabled = false;
            }
        }

        /// <summary>
        /// 定位器命令
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="logger">日志</param>
        [Routed("input-location-enable/{enable}")]
        public void LocationEnable(IRequest request, ILogger logger)
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
