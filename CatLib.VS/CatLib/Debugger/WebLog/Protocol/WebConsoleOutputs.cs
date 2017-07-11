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

using System;
using CatLib.API.Debugger;
using CatLib.Debugger.WebConsole;
using System.Collections.Generic;
using CatLib.Debugger.Log;

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
        public void WriteLine(ILogEntry entry)
        {
            var callStack = new List<string>(entry.StackTrace.FrameCount);

            for (var i = 0; i < entry.StackTrace.FrameCount; i++)
            {
                var frame = entry.StackTrace.GetFrame(i);
                var method = frame.GetMethod();
                if (method.DeclaringType == null || !IsCredibleAssembly(method.DeclaringType.Assembly.GetName().Name))
                {
                    callStack.Add(string.Format("{0}(at {1}:{2})", method, frame.GetFileName(),
                        frame.GetFileLineNumber()));
                }
            }

            outputs.Add(new Dictionary<string, object>
            {
                { "id" , entry.Id },
                { "level" , (int)entry.Level},
                { "namespace" , entry.Namespace},
                { "message" , entry.Message },
                { "callStack" , callStack }
            });
        }

        /// <summary>
        /// 是否是可信的程序集
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns>是否可信</returns>
        private bool IsCredibleAssembly(string assembly)
        {
            bool statu;
            if (credibleAssemblys.TryGetValue(assembly, out statu) && statu)
            {
                return true;
            }
            return false;
        }
    }
}
