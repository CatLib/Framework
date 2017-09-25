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
using CatLib.Debugger.Log;
using CatLib.Debugger.WebConsole;
using System.Collections.Generic;

namespace CatLib.Debugger.WebLog.Protocol
{
    /// <summary>
    /// Web控制台输出
    /// </summary>
    internal sealed class WebConsoleOutputs : IWebConsoleResponse
    {
        /// <summary>
        /// 可信任程序集
        /// </summary>
        private readonly IDictionary<string, bool> credibleAssemblys = new Dictionary<string, bool>
        {
            {"mscorlib" , true },
            {"Microsoft.VisualStudio.TestPlatform.Extensions.VSTestIntegration" , true },
        };
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
        private readonly IList<IDictionary<string, object>> outputs;

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
        /// <param name="namespace">命名空间</param>
        /// <param name="message">消息标题</param>
        /// <param name="callStack">调用堆栈</param>
        /// <param name="time">记录时间</param>
        public void WriteLine(long id, LogLevels level, string @namespace, string message, IList<string> callStack, long time)
        {
            outputs.Add(new Dictionary<string, object>
            {
                { "id" , id },
                { "level" , (int)level},
                { "namespace" , @namespace},
                { "message" , message },
                { "callStack" , callStack },
                { "showStack" , false },
                { "time" , time }
            });
        }

        /// <summary>
        /// 向web控制台屏幕中输出一条消息
        /// </summary>
        /// <param name="entry"></param>
        public void WriteLine(ILogEntry entry)
        {
            WriteLine(entry.Id, entry.Level, entry.Namespace, entry.Message, entry.GetStackTrace(IsCredibleAssembly), entry.Time);
        }

        /// <summary>
        /// 是否是可信的程序集
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns>是否可信</returns>
        private bool IsCredibleAssembly(string assembly)
        {
            bool statu;
            return credibleAssemblys.TryGetValue(assembly, out statu) && statu;
        }
    }
}
