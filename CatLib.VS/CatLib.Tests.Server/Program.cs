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
using CatLib.API.Config;
using CatLib.API.Routing;
using CatLib.Config;
using CatLib.Converters;
using CatLib.Debugger;
using CatLib.Events;
using CatLib.Json;
using CatLib.Routing;

namespace CatLib.Tests.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = GetApplication();
            Console.WriteLine("started console server!");
            Console.ReadLine();
        }

        public static Application GetApplication()
        {
            var app = new Application();
            app.OnFindType((str) => Type.GetType(str));
            app.Bootstrap();
            app.Register(new RoutingProvider());
            app.Register(new JsonProvider());
            app.Register(new DebuggerProvider());
            app.Register(new ConfigProvider());
            app.Register(new ConvertersProvider());
            app.Register(new EventsProvider());
            app.Make<IConfigManager>().Default.Set("debugger.logger.handler.unity", false);
            app.Make<IConfigManager>().Default.Set("debugger.webconsole.monitor.performance.fps", false);
            app.Make<IConfigManager>().Default.Set("debugger.webconsole.monitor.memory.heap", false);
            app.Make<IConfigManager>().Default.Set("debugger.webconsole.monitor.memory.total", false);
            app.Make<IConfigManager>().Default.Set("debugger.webconsole.monitor.screen.width", false);
            app.Make<IConfigManager>().Default.Set("debugger.webconsole.monitor.screen.height", false);
            app.Make<IConfigManager>().Default.Set("debugger.webconsole.monitor.screen.dpi", false);
            app.Init();

            app.Make<IRouter>().Middleware((request, response, next) =>
            {
                Console.Write("Request:" + request.Uri.OriginalString);
                next(request, response);
                Console.WriteLine("[done]");
            });

            return app;
        }
    }
}
