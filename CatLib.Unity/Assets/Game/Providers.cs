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
using CatLib;
using CatLib.Event;
using CatLib.Resources;
using CatLib.Thread;
using CatLib.Time;
using CatLib.Crypt;
using CatLib.Hash;
using CatLib.TimeQueue;
using CatLib.Config;
using CatLib.LocalSetting;
using CatLib.FilterChain;
using CatLib.Routing;

public class Providers
{
    /// <summary>
    /// 服务提供者
    /// </summary>
	public static Type[] ServiceProviders
    {
        get
        {
            return new[] {
                typeof(ResourcesProvider),
                typeof(EventProvider),
                typeof(ThreadProvider),
                typeof(TimeProvider),
                typeof(CryptProvider),
                typeof(HashProvider),
                typeof(CoreProvider),
                typeof(TimeQueueProvider),
                typeof(ConfigProvider),
                typeof(LocalSettingProvider),
                typeof(FilterChainProvider),
                typeof(RoutingProvider),
            };
        }
    }
}
