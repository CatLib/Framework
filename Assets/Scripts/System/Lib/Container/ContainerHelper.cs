using System;
using CatLib.Contracts.Container;

namespace CatLib
{
    ///<summary>容器拓展</summary>
    public static class ContainerHelper
    {

        public static IBindData Singleton<Service, Concrete>(this IContainer container) where Concrete : class
        {
            return container.Bind(typeof(Service).ToString(), typeof(Concrete).ToString(), true);
        }

        public static IBindData Singleton<Service>(this IContainer container) where Service : class
        {
            return container.Bind(typeof(Service).ToString(), typeof(Service).ToString() , true);
        }

        public static IBindData Bind<Service , Concrete>(this IContainer container) where Concrete : class
        {
            return container.Bind(typeof(Service).ToString(), typeof(Concrete).ToString(), false);
        }

        public static IBindData Bind<Service>(this IContainer container) where Service : class
        {
            return container.Bind(typeof(Service).ToString(), typeof(Service).ToString(), false);
        }

        public static T Make<T>(this IContainer container , object[] param = null)
        {
            return (T)container.Make(typeof(T).ToString(), param);                                                                      
        }

        public static T Make<T>(this IContainer container , Type service , object[] param = null)
        {
            return (T)container.Make(service.ToString(), param);
        }

        public static T Make<T>(this IContainer container, string service, object[] param = null)
        {
            return (T)container.Make(service, param);
        }

        public static IContainer Alias<AliasName, Service>(this IContainer container) where Service : class
        {
            return container.Alias(typeof(AliasName).ToString(), typeof(Service).ToString());
        }
    }
}