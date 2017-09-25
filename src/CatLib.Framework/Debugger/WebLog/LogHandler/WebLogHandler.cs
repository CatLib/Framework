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

using CatLib.Debugger.Log;
using System;

namespace CatLib.Debugger.WebLog.LogHandler
{
    /// <summary>
    /// Web日志处理器
    /// </summary>
    public class WebLogHandler : ILogHandler
    {
        /// <summary>
        /// 日志存储
        /// </summary>
        private readonly LogStore store;

        /// <summary>
        /// 网络日志处理器
        /// </summary>
        /// <param name="store"></param>
        public WebLogHandler(LogStore store)
        {
            Guard.Requires<ArgumentNullException>(store != null);
            this.store = store;
        }

        /// <summary>
        /// 日志处理器
        /// </summary>
        /// <param name="log">日志条目</param>
        public void Handler(ILogEntry log)
        {
            store.Log(log);
        }
    }
}
