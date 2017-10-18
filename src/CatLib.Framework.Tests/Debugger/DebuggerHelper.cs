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

using CatLib.Debugger;
using CatLib.Json;
using CatLib.Routing;
using System;

namespace CatLib.Tests.Debugger
{
    public static class DebuggerHelper
    {
        public static Application GetApplication(bool enableWebConsole = true)
        {
            var app = new Application();
            app.OnFindType((str) => { return Type.GetType(str);});
            app.Bootstrap();
            app.Register(new RoutingProvider());
            app.Register(new JsonProvider());
            app.Register(new DebuggerProvider
            {
                WebConsoleEnable = enableWebConsole
            });
            app.Init();
            return app;
        }
    }
}
