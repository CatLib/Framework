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

namespace CatLib.Debugger.Log
{
    /// <summary>
    /// 日志处理器
    /// </summary>
    public interface ILogHandler
    {
        /// <summary>
        /// 日志处理器
        /// </summary>
        /// <param name="log">日志条目</param>
        void Handler(ILogEntry log);
    }
}
