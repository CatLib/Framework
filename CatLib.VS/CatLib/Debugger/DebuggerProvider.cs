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

#if CATLIB
using CatLib.API.Config;
using CatLib.API.Debugger;
using CatLib.Debugger.Log;
using CatLib.Debugger.Log.Handler;
using CatLib.Debugger.WebConsole;
using CatLib.Debugger.WebLog;
using CatLib.Debugger.WebMonitor;
using System;
using System.Collections.Generic;

namespace CatLib.Debugger
{
    /// <summary>
    /// 调试服务
    /// </summary>
    public sealed class DebuggerProvider : IServiceProvider
    {
        /// <summary>
        /// 日志句柄
        /// </summary>
        public IDictionary<string , Type> LogHandlers { get; set; }

        /// <summary>
        /// 首页的日志显示
        /// </summary>
        public IList<string> IndexMonitor { get; set; }

        /// <summary>
        /// WebConsole是否启用
        /// </summary>
        public bool WebConsoleEnable { get; set; }

        /// <summary>
        /// Web控制器Host
        /// </summary>
        public string WebConsoleHost { get; set; }

        /// <summary>
        /// Web控制台端口
        /// </summary>
        public int WebConsolePort { get; set; }

        /// <summary>
        /// 构建一个调试服务提供者
        /// </summary>
        public DebuggerProvider()
        {
            WebConsoleEnable = true;
            WebConsoleHost = "*";
            WebConsolePort = 9478;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        [Priority(5)]
        public void Init()
        {
            InitWebConsole();
        }

        /// <summary>
        /// 初始化Web控制台
        /// </summary>
        private void InitWebConsole()
        {
            var config = App.Make<IConfigManager>();
            if (config == null || config.Default.Get("Debugger.WebConsole.Enable", WebConsoleEnable))
            {
                App.On(ApplicationEvents.OnStartCompleted, (payload) =>
                {
                    App.Make<HttpDebuggerConsole>();
                });

                App.Make<LogStore>();
                App.Make<MonitorStore>();
            }
        }

        /// <summary>
        /// 注册调试服务
        /// </summary>
        public void Register()
        {
            RegisterLogger();
            RegisterWebConsole();
            RegisterWebMonitor();
            RegisterWebLog();
        }

        /// <summary>
        /// 获取日志句柄
        /// </summary>
        /// <returns>句柄</returns>
        private IDictionary<string, Type> GetLogHandlers()
        {
            return new Dictionary<string, Type>(LogHandlers ?? new Dictionary<string, Type>())
            {
                { "Debugger.Logger.Handler.Console" , typeof(StdOutLogHandler) }
            };
        }

        /// <summary>
        /// 注册日志系统
        /// </summary>
        private void RegisterLogger()
        {
            App.Singleton<Logger>().Alias<ILogger>().OnResolving((binder, obj) =>
            {
                var logger = obj as Logger;

                var config = App.Make<IConfigManager>();

                foreach (var handler in GetLogHandlers())
                {
                    if (config == null || config.Default.Get(handler.Key, true))
                    {
                        logger.AddLogHandler(App.Make<ILogHandler>(App.Type2Service(handler.Value)));
                    }
                }

                return obj;
            });
        }

        /// <summary>
        /// 注册web控制台基础服务
        /// </summary>
        private void RegisterWebConsole()
        {
            App.Singleton<HttpDebuggerConsole>().OnResolving((binder, obj) =>
            {
                var config = App.Make<IConfigManager>();
                var host = "*";
                var port = 9478;
                if (config != null)
                {
                    host = config.Default.Get("Debugger.WebConsole.Host", WebConsoleHost);
                    port = config.Default.Get("Debugger.WebConsole.Port", WebConsolePort);
                }

                var httpDebuggerConsole = obj as HttpDebuggerConsole;
                httpDebuggerConsole.Start(host, port);

                return obj;
            });
        }

        /// <summary>
        /// 注册监控
        /// </summary>
        private void RegisterWebMonitor()
        {
            App.Singleton<MonitorStore>().Alias<IMonitor>();
        }

        /// <summary>
        /// 注册Web调试服务
        /// </summary>
        private void RegisterWebLog()
        {
            App.Singleton<LogStore>();
            App.Instance("Debugger.WebMonitor.Monitor.IndexMonitor", new List<string>(IndexMonitor ?? new List<string>())
            {
            });
        }
    }
}
#endif