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
    /// Web控制台响应
    /// </summary>
    public interface IWebConsoleResponse
    {
        /// <summary>
        /// 响应
        /// </summary>
        object Response { get; }
    }
}
