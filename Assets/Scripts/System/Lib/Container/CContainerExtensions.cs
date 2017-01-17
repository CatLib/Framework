using System;
using UnityEngine;
using System.Collections;

namespace CatLib.Container
{
    ///<summary>容器拓展</summary>
    public static class CContainerExtensions
    {

        /// <summary>绑定服务</summary>
        /// <typeparam name="TFrom">接口</typeparam>
        /// <param name="container">容器</param>
        /// <param name="func">实例的类</param>
        /// <param name="alias">别名</param>
        /// <param name="isStatic">是否是静态的</param>
        public static IContainer Bind<TFrom>(this IContainer container, Func<IContainer, object[], object> to, string alias, bool isStatic)
        {
            return container.Bind(typeof(TFrom), to, alias, isStatic);
        }

        ///<summary>绑定服务</summary>
        ///<typeparam name="T1">接口</typeparam>
        ///<typeparam name="T2">类</typeparam>
        public static IContainer Bind<TFrom, TTo>(this IContainer container) where TTo : class, TFrom, new()
        {
			return container.Bind<TFrom , TTo>(null);
        }

        ///<summary>绑定服务</summary>
        ///<param name="alias">名字</param>
        ///<typeparam name="T1">接口</typeparam>
        ///<typeparam name="T2">实例的类</typeparam>
        public static IContainer Bind<TFrom, TTo>(this IContainer container , string alias) where TTo : class, TFrom, new()
        {
			return container.Bind<TFrom>((c , p)=> { return new TTo(); } , alias);
        }

        /// <summary>绑定服务</summary>
        /// <typeparam name="TFrom">接口</typeparam>
        /// <param name="container">容器</param>
        /// <param name="to">实例的类</param>
        public static IContainer Bind<TFrom>(this IContainer container) where TFrom : new()
        {
            return container.Bind<TFrom>((c , p)=> { return new TFrom(); }, null, false);
        }

        /// <summary>绑定服务</summary>
        /// <typeparam name="TFrom">接口</typeparam>
        /// <param name="container">容器</param>
        /// <param name="to">实例的类</param>
        public static IContainer Bind<TFrom>(this IContainer container, Func<IContainer, object[], object> to)
        {
            return container.Bind<TFrom>(to, null, false);
        }

        /// <summary>绑定服务</summary>
        /// <typeparam name="TFrom">接口</typeparam>
        /// <param name="container">容器</param>
        /// <param name="to">实例的类</param>
        /// <param name="alias">别名</param>
        public static IContainer Bind<TFrom>(this IContainer container, Func<IContainer, object[], object> to, string alias)
        {
            return container.Bind<TFrom>(to, alias, false);
        }

        ///<summary>绑定服务</summary>
        ///<typeparam name="T1">接口</typeparam>
        ///<typeparam name="T2">类</typeparam>
        public static IContainer Singleton<TFrom, TTo>(this IContainer container) where TTo : class, TFrom, new()
        {
            return container.Singleton<TFrom, TTo>(null);
        }

        ///<summary>绑定服务</summary>
        ///<param name="alias">名字</param>
        ///<typeparam name="T1">接口</typeparam>
        ///<typeparam name="T2">实例的类</typeparam>
        public static IContainer Singleton<TFrom, TTo>(this IContainer container, string alias) where TTo : class , TFrom , new()
        {
            return container.Singleton<TFrom>((c , p) => { return new TTo(); }, alias);
        }

        /// <summary>绑定服务</summary>
        /// <typeparam name="TFrom">接口</typeparam>
        /// <param name="container">容器</param>
        /// <param name="to">实例的类</param>
        public static IContainer Singleton<TFrom>(this IContainer container) where TFrom : new()
        {
            return container.Bind<TFrom>((c , p) => { return new TFrom(); }, null, true);
        }

        /// <summary>绑定服务</summary>
        /// <typeparam name="TFrom">接口</typeparam>
        /// <param name="container">容器</param>
        /// <param name="to">实例的类</param>
        public static IContainer Singleton<TFrom>(this IContainer container, Func<IContainer, object[], object> to)
        {
            return container.Bind<TFrom>(to, null, true);
        }

        /// <summary>绑定服务</summary>
        /// <typeparam name="TFrom">接口</typeparam>
        /// <param name="container">容器</param>
        /// <param name="to">实例的类</param>
        /// <param name="alias">别名</param>
        public static IContainer Singleton<TFrom>(this IContainer container, Func<IContainer, object[], object> to, string alias)
        {
            return container.Bind<TFrom>(to, alias, true);
        }

        /// <summary>构造一个服务</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="alias"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T Make<T>(this IContainer container, string alias = null , params object[] param) where T : class
        {
            return container.Make(typeof(T), alias, param) as T;
        }

        public static T Make<T>(this IContainer container , Type type , string alias = null , params object[] param)
        {
            return (T)container.Make(type, alias, param);
        }

    }
}