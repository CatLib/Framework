using System;
using UnityEngine;
using System.Collections;

namespace CatLib.Container
{
    ///<summary>容器拓展</summary>
    public static class CContainerExtensions
    {

        public static CBindData Singleton<Service, Concrete>(this IContainer container) where Concrete : class
        {
            return container.Bind(typeof(Service).ToString(), typeof(Concrete).ToString(), true);
        }

        public static CBindData Singleton<Service>(this IContainer container) where Service : class
        {
            return container.Bind(typeof(Service).ToString(), typeof(Service).ToString() , true);
        }

        public static CBindData Bind<Service , Concrete>(this IContainer container) where Concrete : class
        {
            return container.Bind(typeof(Service).ToString(), typeof(Concrete).ToString(), false);
        }

        public static CBindData Bind<Service>(this IContainer container) where Service : class
        {
            return container.Bind(typeof(Service).ToString(), typeof(Service).ToString(), false);
        }

        public static T Make<T>(this IContainer container , object[] param = null) where T : class
        {
            return container.Make(typeof(T).ToString(), param) as T;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
        }

        public static T Make<T>(this IContainer container , Type service , object[] param = null) where T : class
        {
            return container.Make(service.ToString(), param) as T;
        }

        public static T Make<T>(this IContainer container, string service, object[] param = null) where T : class
        {
            return container.Make(service, param) as T;
        }

        public static IContainer Alias<Alias, Service>(this IContainer container) where Service : class
        {
            return container.Alias(typeof(Alias).ToString(), typeof(Service).ToString());
        }
    }
}