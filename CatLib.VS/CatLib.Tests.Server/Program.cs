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
            var config = app.Make<IConfigManager>().Default;
            config.Set("debugger.logger.handler.unity", false);
            config.Set("debugger.monitor.performance", false);
            config.Set("debugger.monitor.screen", false);
            config.Set("debugger.monitor.scene", false);
            config.Set("debugger.monitor.systeminfo", false);
            config.Set("debugger.monitor.path", false);
            config.Set("debugger.monitor.input", false);
            config.Set("debugger.monitor.input.location", false);
            config.Set("debugger.monitor.input.gyro", false);
            config.Set("debugger.monitor.input.compass", false);
            config.Set("debugger.monitor.graphics", false);
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
