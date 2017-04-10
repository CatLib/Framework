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
    ///<summary>
    /// 容器拓展
    /// </summary>
    public static class ContainerHelper
    {
        public static IBindData Singleton(this IContainer container,string service , Func<IContainer, object[], object> concrete)
        {
            return container.Bind(service, concrete, true);
        }

        public static IBindData Singleton<TService, TConcrete>(this IContainer container) where TConcrete : class
        {
            return container.Bind(typeof(TService).ToString(), typeof(TConcrete).ToString(), true);
        }

        public static IBindData Singleton<TService>(this IContainer container) where TService : class
        {
            return container.Bind(typeof(TService).ToString(), typeof(TService).ToString() , true);
        }

        public static IBindData Singleton<TService>(this IContainer container , Func<IContainer, object[], object> concrete) where TService : class
        {
            return container.Bind(typeof(TService).ToString(), concrete , true);
        }

        public static IBindData Bind<TService , TConcrete>(this IContainer container) where TConcrete : class
        {
            return container.Bind(typeof(TService).ToString(), typeof(TConcrete).ToString(), false);
        }

        public static IBindData Bind<TService>(this IContainer container) where TService : class
        {
            return container.Bind(typeof(TService).ToString(), typeof(TService).ToString(), false);
        }

        public static IBindData Bind<TService>(this IContainer container , Func<IContainer, object[], object> concrete) where TService : class
        {
            return container.Bind(typeof(TService).ToString(), concrete, false);
        }

        public static IBindData Bind(this IContainer container ,string service , Func<IContainer, object[], object> concrete)
        {
            return container.Bind(service, concrete, false);
        }

        public static TService MakeParams<TService>(this IContainer container , params object[] param)
        {
            return (TService)container.Make(typeof(TService).ToString(), param);
        }

        public static TService Make<TService>(this IContainer container)
        {
            return (TService)container.Make(typeof(TService).ToString());
        }

        public static TService Make<TService>(this IContainer container , string service)
        {
            return (TService)container.Make(service);
        }

        public static TService Make<TService>(this IContainer container , Type service)
        {
            return (TService)container.Make(service.ToString());
        }

        public static object Make(this IContainer container , Type service)
        {
            return container.Make(service.ToString());
        }

        public static IContainer Alias<TAliasName, TService>(this IContainer container) where TService : class
        {
            return container.Alias(typeof(TAliasName).ToString(), typeof(TService).ToString());
        }
    }
}