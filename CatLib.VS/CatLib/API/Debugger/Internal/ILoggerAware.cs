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

namespace CatLib.API.Debugger
{
    /// <summary>
    /// 记录器实例接口
    /// </summary>
    public interface ILoggerAware
    {
        /// <summary>
        /// 设定记录器实例接口
        /// </summary>
        /// <param name="logger">记录器</param>
        void SetLogger(ILogger logger);
    }
}
