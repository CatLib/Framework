using System;
using CatLib.API.Container;

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

        public static T Make<T>(this IContainer container , params object[] param)
        {
            return (T)container.Make(typeof(T).ToString(), param);                                                                      
        }

        public static object Make(this IContainer container , Type service)
        {
            return container.Make(service.ToString());
        }

        public static IContainer Alias<AliasName, Service>(this IContainer container) where Service : class
        {
            return container.Alias(typeof(AliasName).ToString(), typeof(Service).ToString());
        }
    }
}