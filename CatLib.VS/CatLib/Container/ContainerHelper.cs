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
        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton(this IContainer container,string service , Func<IContainer, object[], object> concrete)
        {
            return container.Bind(service, concrete, true);
        }

        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <typeparam name="TConcrete">服务实现</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton<TService, TConcrete>(this IContainer container) where TConcrete : class
        {
            return container.Bind(typeof(TService).ToString(), typeof(TConcrete), true);
        }

        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名，同时也是服务实现</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton<TService>(this IContainer container) where TService : class
        {
            return container.Bind(typeof(TService).ToString(), typeof(TService) , true);
        }

        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton<TService>(this IContainer container , Func<IContainer, object[], object> concrete) where TService : class
        {
            return container.Bind(typeof(TService).ToString(), concrete , true);
        }

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <typeparam name="TConcrete">服务实现</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind<TService , TConcrete>(this IContainer container) where TConcrete : class
        {
            return container.Bind(typeof(TService).ToString(), typeof(TConcrete), false);
        }

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名，同时也是服务实现</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind<TService>(this IContainer container) where TService : class
        {
            return container.Bind(typeof(TService).ToString(), typeof(TService), false);
        }

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind<TService>(this IContainer container , Func<IContainer, object[], object> concrete) where TService : class
        {
            return container.Bind(typeof(TService).ToString(), concrete, false);
        }

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind(this IContainer container ,string service , Func<IContainer, object[], object> concrete)
        {
            return container.Bind(service, concrete, false);
        }

        /// <summary>
        /// 构造一个服务，允许传入构造参数
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="param">构造参数</param>
        /// <returns>服务实例</returns>
        public static TService MakeParams<TService>(this IContainer container , params object[] param)
        {
            return (TService)container.Make(typeof(TService).ToString(), param);
        }

        /// <summary>
        /// 构造一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务实例</returns>
        public static TService Make<TService>(this IContainer container)
        {
            return (TService)container.Make(typeof(TService).ToString());
        }

        /// <summary>
        /// 构造一个服务
        /// </summary>
        /// <typeparam name="TConvert">服务实例转换到的类型</typeparam>
        /// <param name="container">服务容器</param>
        /// <param name="service">服务名</param>
        /// <returns>服务实例</returns>
        public static TConvert Make<TConvert>(this IContainer container , string service)
        {
            return (TConvert)container.Make(service);
        }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <typeparam name="TAliasName">别名</typeparam>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="container">服务容器</param>
        /// <returns>服务容器</returns>
        public static IContainer Alias<TAliasName, TService>(this IContainer container) where TService : class
        {
            return container.Alias(typeof(TAliasName).ToString(), typeof(TService).ToString());
        }
    }
}