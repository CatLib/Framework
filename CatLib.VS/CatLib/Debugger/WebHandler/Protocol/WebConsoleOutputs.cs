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
using CatLib.Debugger.WebConsole;
using System.Collections.Generic;

namespace CatLib.Debugger.WebHandler.Protocol
{
    /// <summary>
    /// Web控制台输出
    /// </summary>
    internal sealed class WebConsoleOutputs : IWebConsoleResponse
    {
        /// <summary>
        /// 响应
        /// </summary>
        public object Response
        {
            get { return outputs; }
        }

        /// <summary>
        /// 输出
        /// </summary>
        private IList<IDictionary<string, object>> outputs;

        /// <summary>
        /// Web控制台输出
        /// </summary>
        public WebConsoleOutputs()
        {
            outputs = new List<IDictionary<string, object>>();
        }

        /// <summary>
        /// 向Web控制台屏幕中输出一条消息
        /// </summary>
        /// <param name="id">消息id</param>
        /// <param name="level">消息等级</param>
        /// <param name="categroy">消息分组</param>
        /// <param name="message">消息标题</param>
        /// <param name="callStack">调用堆栈</param>
        public void WriteLine(long id, LogLevels level, string categroy, string message, string[] callStack)
        {
            outputs.Add(new Dictionary<string, object>
            {
                { "id" , id },
                { "level" , (int)level},
                { "categroy" , categroy},
                { "message" , message },
                { "callStack" , callStack }
            });
        }

        /// <summary>
        /// 向web控制台屏幕中输出一条消息
        /// </summary>
        /// <param name="entry"></param>
        public void WriteLine(LogEntry entry)
        {
            var callStack = new string[entry.StackTrace.FrameCount];

            for (var i = 0; i < entry.StackTrace.FrameCount; i++)
            {
                var frame = entry.StackTrace.GetFrame(i);
                callStack[i] = string.Format("{0}(at {1}:{2})", frame.GetMethod(), frame.GetFileName(),
                    frame.GetFileLineNumber());
            }

            outputs.Add(new Dictionary<string, object>
            {
                { "id" , entry.Id },
                { "level" , (int)entry.Level},
                { "categroy" , entry.Categroy},
                { "message" , entry.Message },
                { "callStack" , callStack }
            });
        }
    }
}
