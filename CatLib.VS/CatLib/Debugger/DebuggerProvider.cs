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

                if (config == null || config.Default.Get("debugger.webconsole.monitor.fps", true))
                {
                    App.Make<FpsMonitor>();
                }

                if (config == null || config.Default.Get("debugger.webconsole.monitor.heap", true))
                {
                    App.Make<HeapMemoryMonitor>();
                }

                if (config == null || config.Default.Get("debugger.webconsole.monitor.total_memory", true))
                {
                    App.Make<TotalAllocatedMemoryMonitor>();
                }
            }
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
        /// 注册日志系统
        /// </summary>
        private void RegisterLogger()
        {
            App.Singleton<Logger>().Alias<ILogger>().Alias("debugger.logger").OnResolving((binder, obj) =>
            {
                var logger = obj as Logger;

                var config = App.Make<IConfigManager>();
                if (config == null || config.Default.Get("debugger.logger.handler.unity", true))
                {
                    logger.AddLogHandler(new UnityConsoleLogHandler());
                }

                if (config == null || config.Default.Get("debugger.logger.handler.console", true))
                {
                    logger.AddLogHandler(new StdOutLogHandler());
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
        }
    }
}
#endif