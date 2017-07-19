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

using CatLib.API;
using CatLib.Time;
using CatLib.Config;
using CatLib.Converters;
using CatLib.Debugger;
using CatLib.Environment;
using CatLib.Events;
using CatLib.FileSystem;
using CatLib.Json;
using CatLib.Routing;
using CatLib.Timer;
using CatLib.Translation;

namespace CatLib.Bootstrap
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    public class Providers
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        public static IServiceProvider[] ServiceProviders
        {
            get
            {
                return new IServiceProvider[]
                {
                    new ConvertersProvider(), 
                    new TimeProvider(),
                    new EnvironmentProvider(),
                    new TimerProvider(),
                    new ConfigProvider(),
                    new RoutingProvider(),
                    new FileSystemProvider(),
                    new TranslationProvider(), 
                    new DebuggerProvider(), 
                    new JsonProvider(),
                    new EventsProvider(), 
                };
            }
        }
    }
}
