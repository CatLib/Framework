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

using CatLib.Config;
using CatLib.Converters;
using CatLib.Events;
using CatLib.Routing;

namespace CatLib
{
    /// <summary>
    /// 框架默认的服务提供者(这里的提供者在框架启动时必定会被加载)
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class Providers
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
                    new ConfigProvider(), 
                    new EventsProvider(),
                    new RoutingProvider(), 
                };
            }
        }
    }
}
