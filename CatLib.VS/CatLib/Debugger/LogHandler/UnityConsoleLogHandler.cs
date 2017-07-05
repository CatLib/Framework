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

namespace CatLib.Debugger.LogHandler
{
    /// <summary>
    /// Unity控制台日志处理器
    /// </summary>
    class UnityConsoleLogHandler : ILogHandler
    {
        /// <summary>
        /// 日志处理器
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="message">日志内容</param>
        /// <param name="context">上下文,用于替换占位符</param>
        public void Handler(LogLevels level, object message, params object[] context)
        {
            
        }

        //private void
    }
}
