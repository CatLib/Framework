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
using CatLib.API.Container;

namespace CatLib
{
    ///<summary>容器拓展</summary>
    public static class ContainerHelper
    {

        public static IBindData Singleton(this IContainer container,string service , Func<IContainer, object[], object> concrete)
        {
            return container.Bind(service, concrete, true);
        }

        public static IBindData Singleton<Service, Concrete>(this IContainer container) where Concrete : class
        {
            return container.Bind(typeof(Service).ToString(), typeof(Concrete).ToString(), true);
        }

        public static IBindData Singleton<Service>(this IContainer container) where Service : class
        {
            return container.Bind(typeof(Service).ToString(), typeof(Service).ToString() , true);
        }

        public static IBindData Singleton<Service>(this IContainer container , Func<IContainer, object[], object> concrete) where Service : class
        {
            return container.Bind(typeof(Service).ToString(), concrete , true);
        }

        public static IBindData Bind<Service , Concrete>(this IContainer container) where Concrete : class
        {
            return container.Bind(typeof(Service).ToString(), typeof(Concrete).ToString(), false);
        }

        public static IBindData Bind<Service>(this IContainer container) where Service : class
        {
            return container.Bind(typeof(Service).ToString(), typeof(Service).ToString(), false);
        }

        public static IBindData Bind<Service>(this IContainer container , Func<IContainer, object[], object> concrete) where Service : class
        {
            return container.Bind(typeof(Service).ToString(), concrete, false);
        }

        public static IBindData Bind(this IContainer container ,string service , Func<IContainer, object[], object> concrete)
        {
            return container.Bind(service, concrete, false);
        }

        public static To MakeParams<To>(this IContainer container , params object[] param)
        {
            return (To)container.Make(typeof(To).ToString(), param);
        }

        public static To Make<To>(this IContainer container)
        {
            return (To)container.Make(typeof(To).ToString());
        }

        public static To Make<To>(this IContainer container , string service)
        {
            return (To)container.Make(service.ToString());
        }

        public static To Make<To>(this IContainer container , Type service)
        {
            return (To)container.Make(service.ToString());
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