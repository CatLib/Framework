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
using System.Reflection;
using CatLib.API.Events;

namespace CatLib
{
    /// <summary>
    /// CatLib实例
    /// </summary>
    public sealed class App
    {
        /// <summary>
        /// CatLib实例
        /// </summary>
        private static IApplication instance;

        /// <summary>
        /// CatLib实例
        /// </summary>
        public static IApplication Handler
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                throw new NullReferenceException("Application is not instance.");
            }
            set
            {
                instance = value;
            }
        }

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public static void Register(IServiceProvider provider)
        {
            Handler.Register(provider);
        }

        /// <summary>
        /// 获取运行时唯一Id
        /// </summary>
        /// <returns>运行时的唯一Id</returns>
        public static long GetRuntimeId()
        {
            return Handler.GetRuntimeId();
        }

        /// <summary>
        /// 是否是主线程
        /// </summary>
        public static bool IsMainThread
        {
            get
            {
                return Handler.IsMainThread;
            }
        }

        /// <summary>
        /// CatLib版本(遵循semver)
        /// </summary>
        public static string Version
        {
            get
            {
                return Handler.Version;
            }
        }

        /// <summary>
        /// 比较CatLib版本(遵循semver)
        /// <para>输入版本大于当前版本则返回<code>-1</code></para>
        /// <para>输入版本等于当前版本则返回<code>0</code></para>
        /// <para>输入版本小于当前版本则返回<code>1</code></para>
        /// </summary>
        /// <param name="major">主版本号</param>
        /// <param name="minor">次版本号</param>
        /// <param name="revised">修订版本号</param>
        /// <returns>比较结果</returns>
        public static int Compare(int major, int minor, int revised)
        {
            return Handler.Compare(major, minor, revised);
        }

        /// <summary>
        /// 比较CatLib版本(遵循semver)
        /// <para>输入版本大于当前版本则返回<code>-1</code></para>
        /// <para>输入版本等于当前版本则返回<code>0</code></para>
        /// <para>输入版本小于当前版本则返回<code>1</code></para>
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns>比较结果</returns>
        public static int Compare(string version)
        {
            return Handler.Compare(version);
        }

        /// <summary>
        /// 获取优先级，如果存在方法优先级定义那么优先返回方法的优先级
        /// 如果不存在优先级定义那么返回<c>int.MaxValue</c>
        /// </summary>
        /// <param name="type">获取优先级的类型</param>
        /// <param name="method">获取优先级的调用方法</param>
        /// <returns>优先级</returns>
        public static int GetPriority(Type type, string method = null)
        {
            return Handler.GetPriority(type, method);
        }

        /// <summary>
        /// 触发一个事件,并获取事件的返回结果
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="payload">载荷</param>
        /// <returns>事件结果</returns>
        public static object Trigger(string eventName, object payload = null)
        {
            return Handler.Trigger(eventName, payload);
        }

        /// <summary>
        /// 触发一个事件,遇到第一个事件存在处理结果后终止,并获取事件的返回结果
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="payload">载荷</param>
        /// <returns>事件结果</returns>
        public object TriggerHalt(string eventName, object payload = null)
        {
            return Handler.TriggerHalt(eventName, payload);
        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件句柄</param>
        /// <param name="life">在几次后事件会被自动释放</param>
        /// <returns>事件句柄</returns>
        public static IEventHandler On(string eventName, Action<object> handler, int life = 0)
        {
            return Handler.On(eventName, handler, life);
        }

        /// <summary>
        /// 注册一个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件句柄</param>
        /// <param name="life">在几次后事件会被自动释放</param>
        /// <returns>事件句柄</returns>
        public static IEventHandler On(string eventName, Func<object, object> handler, int life = 0)
        {
            return Handler.On(eventName, handler, life);
        }

        /// <summary>
        /// 获取服务的绑定数据,如果绑定不存在则返回null
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        /// <returns>服务绑定数据或者null</returns>
        public static IBindData GetBind(string service)
        {
            return Handler.GetBind(service);
        }

        /// <summary>
        /// 是否已经绑定了服务
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        /// <returns>返回一个bool值代表服务是否被绑定</returns>
        public static bool HasBind(string service)
        {
            return Handler.HasBind(service);
        }

        /// <summary>
        /// 服务是否是静态化的,如果服务不存在也将返回false
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        /// <returns>是否是静态化的</returns>
        public static bool IsStatic(string service)
        {
            return Handler.IsStatic(service);
        }

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否静态化</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind(string service, Func<IContainer, object[], object> concrete, bool isStatic)
        {
            return Handler.Bind(service, concrete, isStatic);
        }

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否静态化</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind(string service, Type concrete, bool isStatic)
        {
            return Handler.Bind(service, concrete, isStatic);
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData BindIf(string service, Func<IContainer, object[], object> concrete, bool isStatic)
        {
            return Handler.BindIf(service, concrete, isStatic);
        }

        /// <summary>
        /// 如果服务不存在那么则绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData BindIf(string service, Type concrete, bool isStatic)
        {
            return Handler.BindIf(service, concrete, isStatic);
        }

        /// <summary>
        /// 为一个及以上的服务定义一个标记
        /// </summary>
        /// <param name="tag">标记名</param>
        /// <param name="service">服务名</param>
        public static void Tag(string tag, params string[] service)
        {
            Handler.Tag(tag, service);
        }

        /// <summary>
        /// 根据标记名生成标记所对应的所有服务实例
        /// </summary>
        /// <param name="tag">标记名</param>
        /// <returns>将会返回标记所对应的所有服务实例</returns>
        public static object[] Tagged(string tag)
        {
            return Handler.Tagged(tag);
        }

        /// <summary>
        /// 静态化一个服务
        /// </summary>
        /// <param name="service">服务名或者别名</param>
        /// <param name="instance">服务实例</param>
        public static void Instance(string service, object instance)
        {
            Handler.Instance(service, instance);
        }

        /// <summary>
        /// 释放某个静态化实例
        /// </summary>
        /// <param name="service">服务名或别名</param>
        public static void Release(string service)
        {
            Handler.Release(service);
        }

        /// <summary>
        /// 当静态服务被释放时
        /// </summary>
        /// <param name="action">处理释放时的回调</param>
        public static IContainer OnRelease(Action<IBindData, object> action)
        {
            return Handler.OnRelease(action);
        }

        /// <summary>
        /// 以依赖注入形式调用一个方法
        /// </summary>
        /// <param name="instance">方法对象</param>
        /// <param name="method">方法名</param>
        /// <param name="param">方法参数</param>
        /// <returns>方法返回值</returns>
        public static object Call(object instance, string method, params object[] param)
        {
            return Handler.Call(instance, method, param);
        }

        /// <summary>
        /// 以依赖注入形式调用一个方法
        /// </summary>
        /// <param name="instance">方法对象</param>
        /// <param name="methodInfo">方法信息</param>
        /// <param name="param">方法参数</param>
        /// <returns>方法返回值</returns>
        public static object Call(object instance, MethodInfo methodInfo, params object[] param)
        {
            return Handler.Call(instance, methodInfo, param);
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <param name="param">构造参数</param>
        /// <returns>服务实例，如果构造失败那么返回null</returns>
        public static object MakeWith(string service, params object[] param)
        {
            return Handler.MakeWith(service, param);
        }

        /// <summary>
        /// 构造服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>服务实例，如果构造失败那么返回null</returns>
        public static object Make(string service)
        {
            return Handler.Make(service);
        }

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <param name="service">映射到的服务名</param>
        /// <returns>当前容器对象</returns>
        public static IContainer Alias(string alias, string service)
        {
            return Handler.Alias(alias, service);
        }

        /// <summary>
        /// 当服务被解决时触发的事件
        /// </summary>
        /// <param name="func">回调函数</param>
        /// <returns>当前容器实例</returns>
        public static IContainer OnResolving(Func<IBindData, object, object> func)
        {
            return Handler.OnResolving(func);
        }

        /// <summary>
        /// 当查找类型无法找到时会尝试去调用开发者提供的查找类型函数
        /// </summary>
        /// <param name="func">查找类型的回调</param>
        /// <param name="priority">查询优先级(值越小越优先)</param>
        /// <returns>当前容器实例</returns>
        public static IContainer OnFindType(Func<string, Type> func, int priority = int.MaxValue)
        {
            return Handler.OnFindType(func, priority);
        }

        /// <summary>
        /// 类型转为服务名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>转换后的服务名</returns>
        public static string Type2Service(Type type)
        {
            return Handler.Type2Service(type);
        }

        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton(string service, Func<IContainer, object[], object> concrete)
        {
            return Handler.Singleton(service, concrete);
        }

        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <typeparam name="TConcrete">服务实现</typeparam>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton<TService, TConcrete>() where TConcrete : class
        {
            return Handler.Singleton<TService, TConcrete>();
        }

        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名，同时也是服务实现</typeparam>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton<TService>() where TService : class
        {
            return Handler.Singleton<TService>();
        }

        /// <summary>
        /// 以单例的形式绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Singleton<TService>(Func<IContainer, object[], object> concrete) where TService : class
        {
            return Handler.Singleton<TService>(concrete);
        }

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <typeparam name="TConcrete">服务实现</typeparam>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind<TService, TConcrete>() where TConcrete : class
        {
            return Handler.Bind<TService, TConcrete>();
        }

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名，同时也是服务实现</typeparam>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind<TService>() where TService : class
        {
            return Handler.Bind<TService>();
        }

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind<TService>(Func<IContainer, object[], object> concrete)
            where TService : class
        {
            return Handler.Bind<TService>(concrete);
        }

        /// <summary>
        /// 常规绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实现</param>
        /// <returns>服务绑定数据</returns>
        public static IBindData Bind(string service,
            Func<IContainer, object[], object> concrete)
        {
            return Handler.Bind(service, concrete);
        }

        /// <summary>
        /// 构造一个服务，允许传入构造参数
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="param">构造参数</param>
        /// <returns>服务实例</returns>
        public static TService MakeWith<TService>(params object[] param)
        {
            return Handler.MakeWith<TService>(param);
        }

        /// <summary>
        /// 构造一个服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <returns>服务实例</returns>
        public static TService Make<TService>()
        {
            return Handler.Make<TService>();
        }

        /// <summary>
        /// 构造一个服务
        /// </summary>
        /// <typeparam name="TConvert">服务实例转换到的类型</typeparam>
        /// <param name="service">服务名</param>
        /// <returns>服务实例</returns>
        public static TConvert Make<TConvert>(string service)
        {
            return Handler.Make<TConvert>(service);
        }

        /// <summary>
        /// 释放服务
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        public static void Releases<TService>() where TService : class
        {
            Handler.Release<TService>();
        }

        /// <summary>
        /// 静态化一个服务,实例值会经过解决修饰器
        /// </summary>
        /// <typeparam name="TService">服务名</typeparam>
        /// <param name="instance">实例值</param>
        public static void Instance<TService>(object instance) where TService : class
        {
            Handler.Instance<TService>(instance);
        }
    }
}