using System;
using CatLib;
using CatLib.API;
using CatLib.Routing;
using CatLib.FilterChain;
using CatLib.Event;

namespace CatLib.Demo.Routing
{
    /**
     * 这个类提供了当前demo演示时用到的组件 
     */
    public class RoutingBootstrap : IBootstrap
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
            IApplication application = gameObject.AddComponent<Application>();
            application.Bootstrap(new Type[] { typeof(RoutingBootstrap) }).Init();
        }
    }

}