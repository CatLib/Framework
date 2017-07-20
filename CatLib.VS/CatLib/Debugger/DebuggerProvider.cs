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
using System;
using System.Collections.Generic;
using CatLib.API.Config;
using CatLib.API.Debugger;
using CatLib.Debugger.Log;
using CatLib.Debugger.Log.Handler;
using CatLib.Debugger.WebConsole;
using CatLib.Debugger.WebLog;
using CatLib.Debugger.WebMonitor;
using CatLib.Debugger.WebMonitorContent;

namespace CatLib.Debugger
{
    /// <summary>
    /// 调试服务
    /// </summary>
    public sealed class DebuggerProvider : ServiceProvider
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>迭代器</returns>
        [Priority(5)]
        public override void Init()
        {
            var config = App.Make<IConfigManager>();
            if (config == null || config.Default.Get("debugger.webconsole.enable", true))
            {
                App.On(ApplicationEvents.OnStartComplete, (payload) =>
                {
                    App.Make<HttpDebuggerConsole>();
                });
                
                App.Make<LogStore>();
                App.Make<MonitorStore>();

                foreach (var monitor in GetMonitors())
                {
                    if (config == null || config.Default.Get(monitor.Key, true))
                    {
                        App.Make(App.Type2Service(monitor.Value));
                    }
                }
            }
        }

        /// <summary>
        /// 获取监控
        /// </summary>
        /// <returns>监控</returns>
        private IDictionary<string, Type> GetMonitors()
        {
            return new Dictionary<string, Type>
            {
                { "debugger.webconsole.monitor.performance.fps" , typeof(FpsMonitor) },
                { "debugger.webconsole.monitor.memory.heap" , typeof(HeapMemoryMonitor) },
                { "debugger.webconsole.monitor.memory.total" , typeof(TotalAllocatedMemoryMonitor) },
                { "debugger.webconsole.monitor.screen.width" , typeof(ScreenWidthMonitor) },
                { "debugger.webconsole.monitor.screen.height" , typeof(ScreenHeightMonitor) },
                { "debugger.webconsole.monitor.screen.dpi" , typeof(ScreenDpiMonitor) }
            };
        }

        /// <summary>
        /// 注册调试服务
        /// </summary>
        public override void Register()
        {
            RegisterLogger();
            RegisterWebConsole();
            RegisterWebMonitor();
            RegisterWebLog();
            RegisterWebMonitorContent();
        }

        /// <summary>
        /// 获取日志句柄
        /// </summary>
        /// <returns>句柄</returns>
        private IDictionary<string, Type> GetLogHandlers()
        {
            return new Dictionary<string, Type>
            {
                { "debugger.logger.handler.unity" , typeof(UnityConsoleLogHandler) },
                { "debugger.logger.handler.console" , typeof(StdOutLogHandler) }
            };
        }

        /// <summary>
        /// 注册日志系统
        /// </summary>
        private void RegisterLogger()
        {
            App.Singleton<Logger>().Alias<ILogger>().Alias("debugger.logger").OnResolving((binder, obj) =>
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
                    host = config.Default.Get("debugger.webconsole.host", "*");
                    port = config.Default.Get("debugger.webconsole.port", 9478);
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
            App.Singleton<MonitorStore>().Alias<IMonitor>().Alias("debugger.monitor");
        }

        /// <summary>
        /// 注册Web调试服务
        /// </summary>
        private void RegisterWebLog()
        {
            App.Singleton<LogStore>();
        }

        /// <summary>
        /// 注册Web监控
        /// </summary>
        private void RegisterWebMonitorContent()
        {
            App.Singleton<FpsMonitor>();
            App.Singleton<HeapMemoryMonitor>();
            App.Singleton<TotalAllocatedMemoryMonitor>();
            App.Singleton<ScreenDpiMonitor>();
            App.Singleton<ScreenHeightMonitor>();
            App.Singleton<ScreenWidthMonitor>();
        }
    }
}
#endif