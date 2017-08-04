﻿/*
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
        /// 启用的日志句柄
        /// </summary>
        public IDictionary<string, KeyValuePair<Type, bool>> LogHandlers { get; set; }

        /// <summary>
        /// 自动生成列表
        /// </summary>
        public IDictionary<string, KeyValuePair<Type, bool>> AutoMake { get; set; }

        /// <summary>
        /// 首页的日志显示
        /// </summary>
        public IList<string> IndexMonitor { get; set; }

        /// <summary>
        /// 控制台日志处理器
        /// </summary>
        public bool StdConsoleLoggerHandler { get; set; }

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
            LogHandlers = null;
            IndexMonitor = null;
            StdConsoleLoggerHandler = false;
            WebConsoleEnable = false;
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
            var config = App.Make<IConfig>();
            if (!config.SafeGet("DebuggerProvider.WebConsoleEnable", WebConsoleEnable))
            {
                return;
            }

            App.On(ApplicationEvents.OnStartCompleted, (payload) =>
            {
                App.Make<HttpDebuggerConsole>();
            });

            App.Make<LogStore>();
            App.Make<MonitorStore>();

            AutoMake = AutoMake ?? new Dictionary<string, KeyValuePair<Type, bool>>();
            foreach (var make in AutoMake)
            {
                if (config.SafeGet(make.Key, make.Value.Value))
                {
                    App.Make(App.Type2Service(make.Value.Key));
                }
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
        private IDictionary<string, KeyValuePair<Type, bool>> GetLogHandlers()
        {
            return new Dictionary<string, KeyValuePair<Type, bool>>(LogHandlers ?? new Dictionary<string, KeyValuePair<Type, bool>>())
            {
                { "DebuggerProvider.ConsoleLoggerHandler" ,
                    new KeyValuePair<Type, bool>(typeof(StdOutLogHandler) , StdConsoleLoggerHandler) }
            };
        }

        /// <summary>
        /// 注册日志系统
        /// </summary>
        private void RegisterLogger()
        {
            App.Singleton<Logger>().Alias<ILogger>().OnResolving((binder, obj) =>
            {
                var logger = (Logger)obj;
                var config = App.Make<IConfig>();

                foreach (var handler in GetLogHandlers())
                {
                    if (!config.SafeGet(handler.Key, handler.Value.Value))
                    {
                        continue;
                    }

                    var logHandler = App.Make<ILogHandler>(App.Type2Service(handler.Value.Key));
                    if (logHandler != null)
                    {
                        logger.AddLogHandler(logHandler);
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
                var config = App.Make<IConfig>();
                var host = config.SafeGet("DebuggerProvider.WebConsoleHost", WebConsoleHost);
                var port = config.SafeGet("DebuggerProvider.WebConsolePort", WebConsolePort);

                var httpDebuggerConsole = (HttpDebuggerConsole) obj;
                httpDebuggerConsole.Start(host, port);

                return obj;
            }).OnRelease((_, obj) =>
            {
                var httpDebuggerConsole = (HttpDebuggerConsole)obj;
                httpDebuggerConsole.Stop();
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
            App.Instance("DebuggerProvider.IndexMonitor", 
                new List<string>(IndexMonitor ?? new List<string>())
            {
            });
        }
    }
}
#endif