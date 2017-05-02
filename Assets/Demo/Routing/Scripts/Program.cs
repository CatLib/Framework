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

using System;
using CatLib.API;
using CatLib.Routing;
using CatLib.FilterChain;
using CatLib.Event;

namespace CatLib.Demo.Routing
{
    /**
     * 这个类提供了当前demo演示时用到的组件 
     */
    public class Bootstraps : IBootstrap
    {

        public void Bootstrap()
        {
            App.Instance.Register(typeof(RoutingProvider));
            App.Instance.Register(typeof(FilterChainProvider));
            App.Instance.Register(typeof(EventProvider));
            App.Instance.Register(typeof(RoutingDemo));
        }

    }

    /**
     * 这个类是入口类用于启动框架 
     */
    public class Program : UnityEngine.MonoBehaviour
    {
        public void Awake()
        {
            (new Application(this)).Bootstrap(new [] { typeof(Bootstraps) }).Init();
        }
    }

}