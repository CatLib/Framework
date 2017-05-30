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
using CatLib.Event;
using CatLib.Time;
using CatLib.Config;
using CatLib.Core;
using CatLib.Routing;
using CatLib.Timer;

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
        public static Type[] ServiceProviders
        {
            get
            {
                return new[]
                {
                    typeof(EventProvider),
                    typeof(TimeProvider),
                    typeof(CoreProvider),
                    typeof(TimerProvider),
                    typeof(ConfigProvider),
                    typeof(RoutingProvider),
                    typeof(StartProvider)
                };
            }
        }
    }
}
