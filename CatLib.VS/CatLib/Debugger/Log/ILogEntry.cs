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

using System.Diagnostics;
using CatLib.API.Debugger;

namespace CatLib.Debugger.Log
{
    /// <summary>
    /// 日志条目
    /// </summary>
    public interface ILogEntry
    {
        /// <summary>
        /// 条目id
        /// </summary>
        long Id { get; }

        /// <summary>
        /// 日志等级
        /// </summary>
        LogLevels Level { get; }

        /// <summary>
        /// 调用堆栈
        /// </summary>
        StackTrace StackTrace { get; }

        /// <summary>
        /// 日志内容
        /// </summary>
        string Message { get; }

        /// <summary>
        /// 命名空间
        /// </summary>
        string Namespace { get; }
    }
}
