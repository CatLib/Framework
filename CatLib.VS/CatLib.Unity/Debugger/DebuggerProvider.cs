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
using CatLib.API.MonoDriver;
using CatLib.API.Routing;
using CatLib.Debugger.Log.Handler;
using CatLib.Debugger.WebConsole;
using CatLib.Debugger.WebLog;
using CatLib.Debugger.WebMonitor;
using CatLib.Debugger.WebMonitorContent;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using ILogger = CatLib.API.Debugger.ILogger;
using ILogHandler = CatLib.Debugger.Log.ILogHandler;
using Logger = CatLib.Debugger.Log.Logger;

namespace CatLib.Debugger
{
    /// <summary>
    /// 调试服务
    /// </summary>
    public sealed class UnityDebuggerProvider : MonoBehaviour , IServiceProvider
    {
        /// <summary>
        /// 基础服务提供者
        /// </summary>
        private readonly DebuggerProvider baseProvider;

        /// <summary>
        /// 启动性能监控
        /// </summary>
        public bool MonitorPerformance = true;

        /// <summary>
        /// 启动屏幕监控
        /// </summary>
        public bool MonitorScreen = true;

        /// <summary>
        /// 启动场景监控
        /// </summary>
        public bool MonitorScene = true;

        /// <summary>
        /// 启动系统信息监控
        /// </summary>
        public bool MonitorSystemInfo = true;

        /// <summary>
        /// 启动路径监控
        /// </summary>
        public bool MonitorPath = true;

        /// <summary>
        /// 启动输入监控
        /// </summary>
        public bool MonitorInput = true;

        /// <summary>
        /// 启动定位监控
        /// </summary>
        public bool MonitorInputLocation = true;

        /// <summary>
        /// 启动陀螺仪监控
        /// </summary>
        public bool MonitorInputGyroscope = true;

        /// <summary>
        /// 启动罗盘监控
        /// </summary>
        public bool MonitorInputCompass = true;

        /// <summary>
        /// 启动显卡监控
        /// </summary>
        public bool MonitorGraphics = true;

        /// <summary>
        /// Unity服务提供者
        /// </summary>
        public UnityDebuggerProvider()
        {
            baseProvider = new DebuggerProvider();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        [Priority(5)]
        public void Init()
        {
            baseProvider.Init();
            InitMainThreadGroup();
        }

        /// <summary>
        /// 初始化主线程组
        /// </summary>
        private void InitMainThreadGroup()
        {
            var router = App.Make<IRouter>();
            var driver = App.Make<IMonoDriver>();

            if (driver != null)
            {
                router.Group("Debugger.MainThreadCall").Middleware((request, response, next) =>
                {
                    var wait = new AutoResetEvent(false);
                    driver.MainThread(() =>
                    {
                        try
                        {
                            next(request, response);
                        }
                        finally
                        {
                            wait.Set();
                        }
                    });
                    wait.WaitOne();
                });
            }
        }

        /// <summary>
        /// 获取监控
        /// </summary>
        /// <returns></returns>
        private IList<KeyValuePair<Type, bool>> GetMonitors()
        {
            return new List<KeyValuePair<Type, bool>>
            {
                new KeyValuePair<Type, bool>(typeof(PerformanceMonitor) , MonitorPerformance),
                new KeyValuePair<Type, bool>(typeof(ScreenMonitor) , MonitorScreen),
                new KeyValuePair<Type, bool>(typeof(SceneMonitor) , MonitorScene),
                new KeyValuePair<Type, bool>(typeof(SystemInfoMonitor) , MonitorSystemInfo),
                new KeyValuePair<Type, bool>(typeof(PathMonitor) , MonitorPath),
                new KeyValuePair<Type, bool>(typeof(InputMonitor) , MonitorInput),
                new KeyValuePair<Type, bool>(typeof(InputLocationMonitor) ,MonitorInputLocation),
                new KeyValuePair<Type, bool>(typeof(InputGyroscopeMonitor) , MonitorInputGyroscope),
                new KeyValuePair<Type, bool>(typeof(InputCompassMonitor),MonitorInputCompass),
                new KeyValuePair<Type, bool>(typeof(GraphicsMonitor) , MonitorGraphics),
            };
        }

        /// <summary>
        /// 注册调试服务
        /// </summary>
        public void Register()
        {
            baseProvider.Register();
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
            App.Singleton<MonitorStore>().Alias<IMonitor>();
        }

        /// <summary>
        /// 注册Web调试服务
        /// </summary>
        private void RegisterWebLog()
        {
            App.Singleton<LogStore>();

            App.Instance("Debugger.WebMonitor.Monitor.IndexMonitor", new List<string>
            {
                "Profiler.GetMonoUsedSize@memory",
                "Profiler.GetTotalAllocatedMemory",
                "fps"
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