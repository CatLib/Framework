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
using CatLib.Debugger.WebMonitorContent;
using System;
using System.Collections.Generic;

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
        private IList<KeyValuePair<string, Type>> GetMonitors()
        {
            return new List<KeyValuePair<string, Type>>
            {
                new KeyValuePair<string, Type>("debugger.monitor.performance" , typeof(PerformanceMonitor)),
                new KeyValuePair<string, Type>("debugger.monitor.screen" , typeof(ScreenMonitor)),
                new KeyValuePair<string, Type>("debugger.monitor.scene" , typeof(SceneMonitor)),
                new KeyValuePair<string, Type>("debugger.monitor.systeminfo" , typeof(SystemInfoMonitor)),
                new KeyValuePair<string, Type>("debugger.monitor.path" , typeof(PathMonitor)),
                new KeyValuePair<string, Type>("debugger.monitor.input",typeof(InputMonitor)),
                new KeyValuePair<string, Type>("debugger.monitor.input.location" , typeof(InputLocationMonitor)),
                new KeyValuePair<string, Type>("debugger.monitor.input.gyro" , typeof(InputGyroscopeMonitor)),
                new KeyValuePair<string, Type>("debugger.monitor.input.compass" , typeof(InputCompassMonitor)),
                new KeyValuePair<string, Type>("debugger.monitor.graphics" , typeof(GraphicsMonitor)),
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

            App.Instance("Debugger.WebMonitor.Monitor.IndexMonitor", new List<string>
            {
                "performance.memory.monoUsedSize",
                "performance.memory.totalAllocatedMemory",
                "performance.fps"
            });
        }

        /// <summary>
        /// 注册Web监控
        /// </summary>
        private void RegisterWebMonitorContent()
        {
            App.Singleton<PerformanceMonitor>();
            App.Singleton<ScreenMonitor>();
            App.Singleton<SceneMonitor>();
            App.Singleton<SystemInfoMonitor>();
            App.Singleton<PathMonitor>();
            App.Singleton<InputMonitor>();
            App.Singleton<InputLocationMonitor>();
            App.Singleton<InputGyroscopeMonitor>();
            App.Singleton<InputCompassMonitor>();
            App.Singleton<GraphicsMonitor>();
        }
    }
}
#endif