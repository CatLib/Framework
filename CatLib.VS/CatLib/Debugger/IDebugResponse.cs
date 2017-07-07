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

namespace CatLib.Debugger
{
    /// <summary>
    /// 调试响应
    /// </summary>
    public interface IDebugResponse
    {
        /// <summary>
        /// 响应内容
        /// </summary>
        object Response { get; }
    }
}
