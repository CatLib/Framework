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

using System.Collections;
using CatLib.API;
using CatLib.API.Config;
using CatLib.API.Debugger;
using CatLib.Debugger.WebConsole;

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
        [Priority(0)]
        public override IEnumerator Init()
        {
            var config = App.Make<IConfigManager>();
            if (config != null && config.Default.Get("debugger.webconsole.enable", false))
            {
                App.Make<HttpDebuggerConsole>();
            }
            return base.Init();
        }

        /// <summary>
        /// 注册调试服务
        /// </summary>
        public override void Register()
        {
            RegisterDebugger();
            RegisterLogger();
            RegisterMonitors();
            RegisterWebConsole();
        }

        /// <summary>
        /// 注册调试服务
        /// </summary>
        private void RegisterDebugger()
        {
            App.Singleton<Debugger>().Alias<IDebugger>();
        }

        /// <summary>
        /// 注册日志系统
        /// </summary>
        private void RegisterLogger()
        {
            App.Singleton<Logger>((binder, param) => new Logger()).Alias<ILogger>();
        }

        /// <summary>
        /// 注册监控器
        /// </summary>
        private void RegisterMonitors()
        {
            App.Singleton<Monitors>((binder, param) => new Monitors()).Alias<IMonitor>();
        }

        /// <summary>
        /// 注册web控制台
        /// </summary>
        private void RegisterWebConsole()
        {
            App.Singleton<HttpDebuggerConsole>();
        }
    }
}
